using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class OverloadAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptors.CustomExceptionOverload);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSymbolAction(CheckIfOverloadsExist, SymbolKind.Method);

        private static void CheckIfOverloadsExist(SymbolAnalysisContext context)
        {
            var assertionMethod = (IMethodSymbol) context.Symbol;

            if (!assertionMethod.IsStandardExceptionAssertion())
                return;

            if (assertionMethod.DeclaringSyntaxReferences.Length != 1 ||
                !(assertionMethod.DeclaringSyntaxReferences[0].GetSyntax() is MethodDeclarationSyntax methodDeclarationSyntax))
                return;

            var semanticModel = context.Compilation.GetSemanticModel(methodDeclarationSyntax.SyntaxTree);

            var assertionOverloads = methodDeclarationSyntax.SyntaxTree
                                                            .GetRoot()
                                                            .DescendantNodes(node => !node.IsKind(SyntaxKind.MethodDeclaration))
                                                            .OfType<MethodDeclarationSyntax>()
                                                            .Select(methodDeclaration => semanticModel.GetDeclaredSymbol(methodDeclaration))
                                                            .Where(methodSymbol => methodSymbol.Name == assertionMethod.Name &&
                                                                                   methodSymbol.Parameters.ContainsParameterNameAndMessage() == false)
                                                            .ToList();

            if (assertionOverloads.Count == 0 ||
                assertionOverloads.ContainsNonParameterizedExceptionFactoryOverload() == false)
                context.ReportDiagnostic(Diagnostic.Create(Descriptors.CustomExceptionOverload, methodDeclarationSyntax.GetLocation()));
        }
    }
}