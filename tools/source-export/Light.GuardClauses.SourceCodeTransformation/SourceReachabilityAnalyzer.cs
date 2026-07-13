using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Light.GuardClauses.SourceCodeTransformation;

internal static class SourceReachabilityAnalyzer
{
    public static CSharpParseOptions CreateParseOptions(SourceTargetFramework targetFramework) =>
        new (
            LanguageVersion.CSharp14,
            preprocessorSymbols: targetFramework == SourceTargetFramework.Net10_0
                ?
                [
                    "NET",
                    "NETCOREAPP",
                    "NET10_0",
                    "NET10_0_OR_GREATER",
                    "NET9_0_OR_GREATER",
                    "NET8_0_OR_GREATER",
                ]
                : ["NETSTANDARD", "NETSTANDARD2_0"]
        );

    public static SourceReachabilityAnalysis Analyze(
        SourceFileMergeOptions options,
        IEnumerable<FileInfo> sourceFiles
    )
    {
        var catalog = SourceCatalog.Create(sourceFiles, CreateParseOptions(options.TargetFramework));
        var analyzer = new Analyzer(options, catalog);
        return analyzer.Analyze();
    }

    private static bool TryGetAssertionName(string fileName, out string assertionName)
    {
        const string prefix = "Check.";
        const string suffix = ".cs";

        if (fileName.StartsWith(prefix, StringComparison.Ordinal) &&
            fileName.EndsWith(suffix, StringComparison.Ordinal))
        {
            assertionName = fileName.Substring(prefix.Length, fileName.Length - prefix.Length - suffix.Length);
            return true;
        }

        assertionName = string.Empty;
        return false;
    }

    private sealed class Analyzer
    {
        private readonly SourceCatalog _catalog;
        private readonly Queue<SourceDeclaration> _declarationsToScan = new ();
        private readonly SourceFileMergeOptions _options;
        private readonly HashSet<SourceDeclarationKey> _reachableCheckAndThrowMembers = new ();
        private readonly HashSet<SourceDeclarationKey> _reachableTopLevelDeclarations = new ();
        private readonly IReadOnlyDictionary<string, AssertionWhitelist.AssertionEntry> _whitelistEntries;

        public Analyzer(SourceFileMergeOptions options, SourceCatalog catalog)
        {
            _options = options;
            _catalog = catalog;
            _whitelistEntries = options.AssertionWhitelist.GetEntriesByAssertionName();
        }

        public SourceReachabilityAnalysis Analyze()
        {
            EnqueueRootCheckMethods();

            while (_declarationsToScan.TryDequeue(out var declaration))
            {
                ScanDeclaration(declaration);
            }

            return new (
                _reachableCheckAndThrowMembers,
                _reachableTopLevelDeclarations
            );
        }

        private void EnqueueRootCheckMethods()
        {
            foreach (var declaration in _catalog.CheckMembers)
            {
                if (!TryGetAssertionName(declaration.File.Name, out var assertionName))
                {
                    continue;
                }

                if (!_whitelistEntries.TryGetValue(assertionName, out var entry))
                {
                    throw new InvalidOperationException(
                        $"The assertion whitelist does not expose an entry named \"{assertionName}\" for file \"{declaration.File.Name}\"."
                    );
                }

                if (!entry.Include)
                {
                    continue;
                }

                if (ShouldExcludeDeclaration(declaration))
                {
                    continue;
                }

                EnqueueDeclaration(declaration);
            }
        }

        private void ScanDeclaration(SourceDeclaration declaration)
        {
            var semanticModel = _catalog.GetSemanticModel(declaration.Syntax.SyntaxTree);

            foreach (var node in declaration.Syntax.DescendantNodesAndSelf())
            {
                var foundSourceSymbol = MarkSymbols(node, semanticModel);
                if (!foundSourceSymbol)
                {
                    MarkFallbackSourceSymbols(node);
                }
            }
        }

        private bool MarkSymbols(SyntaxNode node, SemanticModel semanticModel)
        {
            var foundSourceSymbol = false;

            var symbolInfo = GetSymbolInfo(node, semanticModel);
            foundSourceSymbol |= MarkSymbol(symbolInfo.Symbol);
            foreach (var candidateSymbol in symbolInfo.CandidateSymbols)
            {
                foundSourceSymbol |= MarkSymbol(candidateSymbol);
            }

            var typeInfo = GetTypeInfo(node, semanticModel);
            foundSourceSymbol |= MarkType(typeInfo.Type);
            foundSourceSymbol |= MarkType(typeInfo.ConvertedType);

            return foundSourceSymbol;
        }

        private bool MarkSymbol(ISymbol? symbol)
        {
            if (symbol == null)
            {
                return false;
            }

            if (symbol is IMethodSymbol { ReducedFrom: { } reducedFrom })
            {
                return MarkSymbol(reducedFrom);
            }

            switch (symbol)
            {
                case IMethodSymbol methodSymbol:
                    if (_catalog.TryGetCheckOrThrowMember(methodSymbol, out var declaration))
                    {
                        if (_options.RemoveOverloadsWithExceptionFactory &&
                            declaration.HasExceptionFactoryParameter)
                        {
                            return true;
                        }

                        EnqueueDeclaration(declaration);
                        return true;
                    }

                    var foundMethodSourceSymbol = MarkSymbol(methodSymbol.AssociatedSymbol);
                    foundMethodSourceSymbol |= MarkType(methodSymbol.ContainingType);
                    foundMethodSourceSymbol |= MarkType(methodSymbol.ReturnType);
                    foreach (var parameter in methodSymbol.Parameters)
                    {
                        foundMethodSourceSymbol |= MarkType(parameter.Type);
                    }

                    foreach (var typeArgument in methodSymbol.TypeArguments)
                    {
                        foundMethodSourceSymbol |= MarkType(typeArgument);
                    }

                    return foundMethodSourceSymbol;

                case INamedTypeSymbol namedTypeSymbol: return MarkType(namedTypeSymbol);

                case IFieldSymbol fieldSymbol: return MarkType(fieldSymbol.ContainingType) | MarkType(fieldSymbol.Type);

                case IPropertySymbol propertySymbol:
                    return MarkType(propertySymbol.ContainingType) | MarkType(propertySymbol.Type);

                case IEventSymbol eventSymbol: return MarkType(eventSymbol.ContainingType) | MarkType(eventSymbol.Type);

                case IParameterSymbol parameterSymbol: return MarkType(parameterSymbol.Type);

                case ILocalSymbol localSymbol: return MarkType(localSymbol.Type);
            }

            return MarkType(symbol.ContainingType);
        }

        private bool MarkType(ITypeSymbol? typeSymbol) =>
            MarkType(typeSymbol, new (SymbolEqualityComparer.Default));

        private bool MarkType(ITypeSymbol? typeSymbol, HashSet<ITypeSymbol> visitedTypes)
        {
            if (typeSymbol == null)
            {
                return false;
            }

            if (!visitedTypes.Add(typeSymbol))
            {
                return false;
            }

            switch (typeSymbol)
            {
                case IArrayTypeSymbol arrayTypeSymbol: return MarkType(arrayTypeSymbol.ElementType, visitedTypes);

                case IPointerTypeSymbol pointerTypeSymbol:
                    return MarkType(pointerTypeSymbol.PointedAtType, visitedTypes);

                case ITypeParameterSymbol typeParameterSymbol:
                    var foundConstraintSourceSymbol = false;
                    foreach (var constraintType in typeParameterSymbol.ConstraintTypes)
                    {
                        foundConstraintSourceSymbol |= MarkType(constraintType, visitedTypes);
                    }

                    return foundConstraintSourceSymbol;

                case INamedTypeSymbol namedTypeSymbol:
                    var foundSourceSymbol = false;
                    if (_catalog.TryGetTopLevelDeclarationForType(namedTypeSymbol, out var declaration))
                    {
                        EnqueueDeclaration(declaration);
                        foundSourceSymbol = true;
                    }

                    foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                    {
                        foundSourceSymbol |= MarkType(typeArgument, visitedTypes);
                    }

                    if (namedTypeSymbol.ContainingType != null)
                    {
                        foundSourceSymbol |= MarkType(namedTypeSymbol.ContainingType, visitedTypes);
                    }

                    return foundSourceSymbol;
            }

            return false;
        }

        private void MarkFallbackSourceSymbols(SyntaxNode node)
        {
            var name = GetSimpleName(node);
            if (name == null)
            {
                return;
            }

            if (_catalog.TopLevelDeclarationsByName.TryGetValue(name, out var declarations))
            {
                foreach (var declaration in declarations)
                {
                    EnqueueDeclaration(declaration);
                }
            }

            if (!IsInvocationName(node))
            {
                return;
            }

            if (_catalog.CheckMembersByName.TryGetValue(name, out var checkMembers))
            {
                foreach (var checkMember in checkMembers)
                {
                    EnqueueDeclaration(checkMember);
                }
            }

            if (_catalog.ThrowMembersByName.TryGetValue(name, out var throwMembers))
            {
                foreach (var throwMember in throwMembers)
                {
                    EnqueueDeclaration(throwMember);
                }
            }
        }

        private void EnqueueDeclaration(SourceDeclaration declaration)
        {
            if (ShouldExcludeDeclaration(declaration))
            {
                return;
            }

            var reachableSet = declaration.IsCheckOrThrowMember ?
                _reachableCheckAndThrowMembers :
                _reachableTopLevelDeclarations;

            if (reachableSet.Add(declaration.Key))
            {
                _declarationsToScan.Enqueue(declaration);
            }
        }

        private bool ShouldExcludeDeclaration(SourceDeclaration declaration)
        {
            if (!declaration.IsCheckOrThrowMember || !declaration.HasExceptionFactoryParameter)
            {
                return false;
            }

            if (_options.RemoveOverloadsWithExceptionFactory)
            {
                return true;
            }

            if (declaration.Kind != SourceDeclarationKind.CheckMember ||
                !TryGetAssertionName(declaration.File.Name, out var assertionName) ||
                !_whitelistEntries.TryGetValue(assertionName, out var entry))
            {
                return false;
            }

            return !entry.IncludeExceptionFactoryOverload;
        }

        private static SymbolInfo GetSymbolInfo(SyntaxNode node, SemanticModel semanticModel) =>
            node switch
            {
                AttributeSyntax attribute => semanticModel.GetSymbolInfo(attribute),
                ConstructorInitializerSyntax constructorInitializer => semanticModel.GetSymbolInfo(
                    constructorInitializer
                ),
                ExpressionSyntax expression => semanticModel.GetSymbolInfo(expression),
                OrderingSyntax ordering => semanticModel.GetSymbolInfo(ordering),
                SelectOrGroupClauseSyntax selectOrGroupClause => semanticModel.GetSymbolInfo(selectOrGroupClause),
                _ => default,
            };

        private static TypeInfo GetTypeInfo(SyntaxNode node, SemanticModel semanticModel) =>
            node switch
            {
                TypeSyntax type => semanticModel.GetTypeInfo(type),
                ExpressionSyntax expression => semanticModel.GetTypeInfo(expression),
                _ => default,
            };

        private static string? GetSimpleName(SyntaxNode node) =>
            node switch
            {
                IdentifierNameSyntax identifierName => identifierName.Identifier.ValueText,
                GenericNameSyntax genericName => genericName.Identifier.ValueText,
                MemberAccessExpressionSyntax memberAccess => GetSimpleName(memberAccess.Name),
                QualifiedNameSyntax qualifiedName => GetSimpleName(qualifiedName.Right),
                AliasQualifiedNameSyntax aliasQualifiedName => GetSimpleName(aliasQualifiedName.Name),
                AttributeSyntax attribute => GetSimpleName(attribute.Name),
                _ => null,
            };

        private static bool IsInvocationName(SyntaxNode node)
        {
            if (node is not SimpleNameSyntax)
            {
                return false;
            }

            return node.Parent switch
            {
                InvocationExpressionSyntax => true,
                MemberAccessExpressionSyntax { Parent: InvocationExpressionSyntax } => true,
                MemberBindingExpressionSyntax { Parent: InvocationExpressionSyntax } => true,
                _ => false,
            };
        }
    }

    private sealed class SourceCatalog
    {
        private readonly Dictionary<ISymbol, SourceDeclaration> _checkAndThrowMembers =
            new (SymbolEqualityComparer.Default);

        private readonly Dictionary<SyntaxTree, SemanticModel> _semanticModels = new ();

        private readonly Dictionary<ISymbol, SourceDeclaration> _topLevelDeclarationsByType =
            new (SymbolEqualityComparer.Default);

        private SourceCatalog(CSharpCompilation compilation, IReadOnlyList<SourceFile> sourceFiles)
        {
            Compilation = compilation;
            SourceFiles = sourceFiles;
        }

        public CSharpCompilation Compilation { get; }

        public IReadOnlyList<SourceFile> SourceFiles { get; }

        public List<SourceDeclaration> CheckMembers { get; } = new ();

        public Dictionary<string, List<SourceDeclaration>> CheckMembersByName { get; } = new (StringComparer.Ordinal);

        public Dictionary<string, List<SourceDeclaration>> ThrowMembersByName { get; } = new (StringComparer.Ordinal);

        public Dictionary<string, List<SourceDeclaration>> TopLevelDeclarationsByName { get; } =
            new (StringComparer.Ordinal);

        public static SourceCatalog Create(IEnumerable<FileInfo> files, CSharpParseOptions parseOptions)
        {
            var sourceFiles = files.Select(file => SourceFile.Parse(file, parseOptions)).ToArray();
            var compilation = CSharpCompilation.Create(
                "Light.GuardClauses.SourceExportReachability",
                sourceFiles.Select(sourceFile => sourceFile.SyntaxTree),
                CreateMetadataReferences(),
                new (
                    OutputKind.DynamicallyLinkedLibrary,
                    nullableContextOptions: NullableContextOptions.Enable,
                    allowUnsafe: true
                )
            );

            var catalog = new SourceCatalog(compilation, sourceFiles);
            catalog.IndexDeclarations();
            return catalog;
        }

        public SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
        {
            if (!_semanticModels.TryGetValue(syntaxTree, out var semanticModel))
            {
                semanticModel = Compilation.GetSemanticModel(syntaxTree, true);
                _semanticModels.Add(syntaxTree, semanticModel);
            }

            return semanticModel;
        }

        public bool TryGetCheckOrThrowMember(IMethodSymbol methodSymbol, out SourceDeclaration declaration)
        {
            if (methodSymbol.ReducedFrom != null &&
                TryGetCheckOrThrowMember(methodSymbol.ReducedFrom, out declaration!))
            {
                return true;
            }

            if (_checkAndThrowMembers.TryGetValue(methodSymbol, out declaration!))
            {
                return true;
            }

            return _checkAndThrowMembers.TryGetValue(methodSymbol.OriginalDefinition, out declaration!);
        }

        public bool TryGetTopLevelDeclarationForType(
            INamedTypeSymbol typeSymbol,
            out SourceDeclaration declaration
        )
        {
            if (_topLevelDeclarationsByType.TryGetValue(typeSymbol, out declaration!))
            {
                return true;
            }

            return _topLevelDeclarationsByType.TryGetValue(typeSymbol.OriginalDefinition, out declaration!);
        }

        private void IndexDeclarations()
        {
            foreach (var sourceFile in SourceFiles)
            {
                var semanticModel = GetSemanticModel(sourceFile.SyntaxTree);

                foreach (var member in GetNamespaceMembers(sourceFile.Root))
                {
                    if (member is ClassDeclarationSyntax { Identifier.ValueText: "Check" } checkClass)
                    {
                        IndexCheckOrThrowMembers(sourceFile, checkClass, semanticModel, true);
                        continue;
                    }

                    if (member is ClassDeclarationSyntax { Identifier.ValueText: "Throw" } throwClass)
                    {
                        IndexCheckOrThrowMembers(sourceFile, throwClass, semanticModel, false);
                        continue;
                    }

                    if (IsTypeDeclaration(member))
                    {
                        IndexTopLevelType(sourceFile, member, semanticModel);
                    }
                }
            }
        }

        private void IndexCheckOrThrowMembers(
            SourceFile sourceFile,
            ClassDeclarationSyntax classDeclaration,
            SemanticModel semanticModel,
            bool isCheck
        )
        {
            foreach (var member in classDeclaration.Members)
            {
                if (GetDeclaredSymbol(semanticModel, member) is not IMethodSymbol methodSymbol)
                {
                    continue;
                }

                var declaration = new SourceDeclaration(
                    sourceFile.File,
                    member,
                    isCheck ? SourceDeclarationKind.CheckMember : SourceDeclarationKind.ThrowMember,
                    methodSymbol.Name,
                    HasExceptionFactoryParameter(member)
                );
                AddCheckOrThrowMember(methodSymbol, declaration);
                AddCheckOrThrowMember(methodSymbol.OriginalDefinition, declaration);
                AddByName(isCheck ? CheckMembersByName : ThrowMembersByName, methodSymbol.Name, declaration);

                if (isCheck)
                {
                    CheckMembers.Add(declaration);
                }
            }
        }

        private void IndexTopLevelType(
            SourceFile sourceFile,
            MemberDeclarationSyntax topLevelMember,
            SemanticModel semanticModel
        )
        {
            if (GetDeclaredSymbol(semanticModel, topLevelMember) is not INamedTypeSymbol topLevelSymbol)
            {
                return;
            }

            var declaration = new SourceDeclaration(
                sourceFile.File,
                topLevelMember,
                SourceDeclarationKind.TopLevelType,
                topLevelSymbol.Name,
                false
            );
            AddByName(TopLevelDeclarationsByName, topLevelSymbol.Name, declaration);

            foreach (var typeMember in topLevelMember.DescendantNodesAndSelf().Where(IsTypeDeclarationNode))
            {
                if (GetDeclaredSymbol(semanticModel, typeMember) is INamedTypeSymbol typeSymbol)
                {
                    AddTopLevelDeclarationForType(typeSymbol, declaration);
                    AddTopLevelDeclarationForType(typeSymbol.OriginalDefinition, declaration);
                }
            }
        }

        private void AddCheckOrThrowMember(ISymbol symbol, SourceDeclaration declaration) =>
            _checkAndThrowMembers[symbol] = declaration;

        private void AddTopLevelDeclarationForType(ISymbol symbol, SourceDeclaration declaration) =>
            _topLevelDeclarationsByType[symbol] = declaration;

        private static void AddByName(
            Dictionary<string, List<SourceDeclaration>> declarationsByName,
            string name,
            SourceDeclaration declaration
        )
        {
            if (!declarationsByName.TryGetValue(name, out var declarations))
            {
                declarations = new ();
                declarationsByName.Add(name, declarations);
            }

            declarations.Add(declaration);
        }

        private static IEnumerable<MemberDeclarationSyntax> GetNamespaceMembers(CompilationUnitSyntax root)
        {
            foreach (var member in root.Members)
            {
                switch (member)
                {
                    case FileScopedNamespaceDeclarationSyntax fileScopedNamespace:
                        foreach (var namespaceMember in fileScopedNamespace.Members)
                        {
                            yield return namespaceMember;
                        }

                        break;

                    case NamespaceDeclarationSyntax namespaceDeclaration:
                        foreach (var namespaceMember in namespaceDeclaration.Members)
                        {
                            yield return namespaceMember;
                        }

                        break;
                }
            }
        }

        private static bool IsTypeDeclaration(MemberDeclarationSyntax member) =>
            member is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax;

        private static bool IsTypeDeclarationNode(SyntaxNode node) =>
            node is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax;

        private static ISymbol? GetDeclaredSymbol(SemanticModel semanticModel, SyntaxNode declaration) =>
            declaration switch
            {
                BaseMethodDeclarationSyntax method => semanticModel.GetDeclaredSymbol(method),
                BasePropertyDeclarationSyntax property => semanticModel.GetDeclaredSymbol(property),
                BaseTypeDeclarationSyntax type => semanticModel.GetDeclaredSymbol(type),
                DelegateDeclarationSyntax @delegate => semanticModel.GetDeclaredSymbol(@delegate),
                _ => null,
            };

        private static bool HasExceptionFactoryParameter(MemberDeclarationSyntax member) =>
            member is BaseMethodDeclarationSyntax method &&
            method.ParameterList.Parameters.Any(parameter => parameter.Identifier.ValueText == "exceptionFactory");

        private static IReadOnlyList<MetadataReference> CreateMetadataReferences()
        {
            var referencesByPath = new Dictionary<string, MetadataReference>(StringComparer.OrdinalIgnoreCase);

            if (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string trustedPlatformAssemblies)
            {
                foreach (var assemblyPath in trustedPlatformAssemblies.Split(Path.PathSeparator))
                {
                    AddReference(referencesByPath, assemblyPath);
                }
            }

            AddReference(referencesByPath, typeof(object).Assembly.Location);
            AddReference(referencesByPath, typeof(Enumerable).Assembly.Location);
            AddReference(referencesByPath, typeof(ImmutableArray).Assembly.Location);

            return referencesByPath.Values.ToArray();
        }

        private static void AddReference(
            Dictionary<string, MetadataReference> referencesByPath,
            string assemblyPath
        )
        {
            if (string.IsNullOrWhiteSpace(assemblyPath) ||
                !File.Exists(assemblyPath) ||
                referencesByPath.ContainsKey(assemblyPath))
            {
                return;
            }

            referencesByPath.Add(assemblyPath, MetadataReference.CreateFromFile(assemblyPath));
        }
    }

    private sealed record SourceFile(FileInfo File, SyntaxTree SyntaxTree, CompilationUnitSyntax Root)
    {
        public static SourceFile Parse(FileInfo file, CSharpParseOptions parseOptions)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(
                file.ReadContent(),
                parseOptions,
                file.FullName
            );
            return new (file, syntaxTree, (CompilationUnitSyntax) syntaxTree.GetRoot());
        }
    }

    private sealed record SourceDeclaration(
        FileInfo File,
        MemberDeclarationSyntax Syntax,
        SourceDeclarationKind Kind,
        string Name,
        bool HasExceptionFactoryParameter
    )
    {
        public SourceDeclarationKey Key => SourceDeclarationKey.From(File, Syntax);

        public bool IsCheckOrThrowMember =>
            Kind is SourceDeclarationKind.CheckMember or SourceDeclarationKind.ThrowMember;
    }

    private enum SourceDeclarationKind
    {
        CheckMember,
        ThrowMember,
        TopLevelType,
    }
}

internal sealed class SourceReachabilityAnalysis
{
    private readonly HashSet<SourceDeclarationKey> _reachableCheckAndThrowMembers;
    private readonly HashSet<SourceDeclarationKey> _reachableTopLevelDeclarations;

    public SourceReachabilityAnalysis(
        HashSet<SourceDeclarationKey> reachableCheckAndThrowMembers,
        HashSet<SourceDeclarationKey> reachableTopLevelDeclarations
    )
    {
        _reachableCheckAndThrowMembers = reachableCheckAndThrowMembers;
        _reachableTopLevelDeclarations = reachableTopLevelDeclarations;
    }

    public bool ShouldIncludeCheckOrThrowMember(FileInfo file, MemberDeclarationSyntax member) =>
        _reachableCheckAndThrowMembers.Contains(SourceDeclarationKey.From(file, member));

    public bool ShouldIncludeTopLevelDeclaration(FileInfo file, MemberDeclarationSyntax member) =>
        _reachableTopLevelDeclarations.Contains(SourceDeclarationKey.From(file, member));
}

internal readonly record struct SourceDeclarationKey(string FilePath, int SpanStart)
{
    public static SourceDeclarationKey From(FileInfo file, SyntaxNode syntax) =>
        new (file.FullName, syntax.SpanStart);
}
