using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Light.GuardClauses.FrameworkExtensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class SourceFileMerger
{
    public static void CreateSingleSourceFile(SourceFileMergeOptions options)
    {
        options.MustNotBeNull(nameof(options));

        // Prepare the target syntax
        var stringBuilder = new StringBuilder();

        if (options.IncludeVersionComment)
        {
            Console.WriteLine("Appending version header...");
            stringBuilder.AppendLine("/* ------------------------------")
                         .AppendLine($"   Light.GuardClauses {typeof(SourceFileMerger).Assembly.GetName().Version!.ToString(3)}")
                         .AppendLine("   ------------------------------")
                         .AppendLine();
        }

        Console.WriteLine("Creating default file layout...");
        stringBuilder.AppendLineIf(!options.IncludeVersionComment, "/*")
                     .AppendLine($@"License information for Light.GuardClauses

The MIT License (MIT)
Copyright (c) 2016, 2021 Kenny Pflug mailto:kenny.pflug@live.de

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
{(options.IncludeJetBrainsAnnotationsUsing ? "using JetBrains.Annotations;" + Environment.NewLine : string.Empty)}using {options.BaseNamespace}.Exceptions;
using {options.BaseNamespace}.FrameworkExtensions;

#nullable enable annotations

namespace {options.BaseNamespace}
{{
    
}}

namespace {options.BaseNamespace}.Exceptions
{{
}}

namespace {options.BaseNamespace}.FrameworkExtensions
{{
}}");
        if (options.IncludeJetBrainsAnnotations)
        {
            stringBuilder.AppendLine().AppendLine(@"/* 
License information for JetBrains.Annotations

MIT License
Copyright (c) 2016 JetBrains http://www.jetbrains.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

namespace JetBrains.Annotations
{
}");
        }

        if (options.IncludeCodeAnalysisNullableAttributes)
        {
            stringBuilder.AppendLine().AppendLine(@"
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that <see langword=""null""/> is allowed as an input even if the
    /// corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=""AllowNullAttribute""/> class.
        /// </summary>
        public AllowNullAttribute() { }
    }

    /// <summary>
    /// Specifies that <see langword=""null""/> is disallowed as an input even if the
    /// corresponding type allows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=""DisallowNullAttribute""/> class.
        /// </summary>
        public DisallowNullAttribute() { }
    }

    /// <summary>
    /// Specifies that a method that will never return under any circumstance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=""DoesNotReturnAttribute""/> class.
        /// </summary>
        public DoesNotReturnAttribute() { }
    }

    /// <summary>
    /// Specifies that the method will not return if the associated <see cref=""Boolean""/>
    /// parameter is passed the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        /// <summary>
        /// Gets the condition parameter value.
        /// Code after the method is considered unreachable by diagnostics if the argument
        /// to the associated parameter matches this value.
        /// </summary>
        public bool ParameterValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref=""DoesNotReturnIfAttribute""/>
        /// class with the specified parameter value.
        /// </summary>
        /// <param name=""parameterValue"">
        /// The condition parameter value.
        /// Code after the method is considered unreachable by diagnostics if the argument
        /// to the associated parameter matches this value.
        /// </param>
        public DoesNotReturnIfAttribute(bool parameterValue)
        {
            ParameterValue = parameterValue;
        }
    }

    /// <summary>
    /// Specifies that an output may be <see langword=""null""/> even if the
    /// corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=""MaybeNullAttribute""/> class.
        /// </summary>
        public MaybeNullAttribute() { }
    }

    /// <summary>
    /// Specifies that when a method returns <see cref=""ReturnValue""/>, 
    /// the parameter may be <see langword=""null""/> even if the corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Gets the return value condition.
        /// If the method returns this value, the associated parameter may be <see langword=""null""/>.
        /// </summary>
        public bool ReturnValue { get; }

        /// <summary>
        /// Initializes the attribute with the specified return value condition.
        /// </summary>
        /// <param name=""returnValue"">
        /// The return value condition.
        /// If the method returns this value, the associated parameter may be <see langword=""null""/>.
        /// </param>
        public MaybeNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }

    /// <summary>
    /// Specifies that an output is not <see langword=""null""/> even if the
    /// corresponding type allows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=""NotNullAttribute""/> class.
        /// </summary>
        public NotNullAttribute() { }
    }

    /// <summary>
    /// Specifies that the output will be non-<see langword=""null""/> if the
    /// named parameter is non-<see langword=""null""/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        /// <summary>
        /// Gets the associated parameter name.
        /// The output will be non-<see langword=""null""/> if the argument to the
        /// parameter specified is non-<see langword=""null""/>.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes the attribute with the associated parameter name.
        /// </summary>
        /// <param name=""parameterName"">
        /// The associated parameter name.
        /// The output will be non-<see langword=""null""/> if the argument to the
        /// parameter specified is non-<see langword=""null""/>.
        /// </param>
        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }

    /// <summary>
    /// Specifies that when a method returns <see cref=""ReturnValue""/>,
    /// the parameter will not be <see langword=""null""/> even if the corresponding type allows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Gets the return value condition.
        /// If the method returns this value, the associated parameter will not be <see langword=""null""/>.
        /// </summary>
        public bool ReturnValue { get; }

        /// <summary>
        /// Initializes the attribute with the specified return value condition.
        /// </summary>
        /// <param name=""returnValue"">
        /// The return value condition.
        /// If the method returns this value, the associated parameter will not be <see langword=""null""/>.
        /// </param>
        public NotNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }
}");
        }

        if (options.IncludeCallerArgumentExpressionAttribute)
        {
            stringBuilder.AppendLine().AppendLine(@"
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    
        public string ParameterName { get; }
    }
}");
        }

        var csharpParseOptions = new CSharpParseOptions(LanguageVersion.CSharp10);
        var targetSyntaxTree = CSharpSyntaxTree.ParseText(stringBuilder.ToString(), csharpParseOptions);

        var targetRoot = (CompilationUnitSyntax) targetSyntaxTree.GetRoot();

        var namespaces = targetRoot.Members
                                   .OfType<NamespaceDeclarationSyntax>()
                                   .ToList();
        var defaultNamespace = namespaces.First(@namespace => @namespace.Name.ToString() == $"{options.BaseNamespace}");
        var exceptionsNamespace = namespaces.First(@namespace => @namespace.Name.ToString() == $"{options.BaseNamespace}.Exceptions");
        var extensionsNamespace = namespaces.First(@namespace => @namespace.Name.ToString() == $"{options.BaseNamespace}.FrameworkExtensions");
        var jetBrainsNamespace = namespaces.FirstOrDefault(@namespace => @namespace.Name.ToString() == "JetBrains.Annotations");
        var replacedNodes = new Dictionary<NamespaceDeclarationSyntax, NamespaceDeclarationSyntax>
        {
            [defaultNamespace] = defaultNamespace,
            [exceptionsNamespace] = exceptionsNamespace,
            [extensionsNamespace] = extensionsNamespace
        };
        if (options.IncludeJetBrainsAnnotations && jetBrainsNamespace != null)
            replacedNodes.Add(jetBrainsNamespace, jetBrainsNamespace);

        var allSourceFiles = new DirectoryInfo(options.SourceFolder).GetFiles("*.cs", SearchOption.AllDirectories)
                                                                    .Where(f => !f.FullName.Contains("obj") &&
                                                                                !f.FullName.Contains("bin"))
                                                                    .ToDictionary(f => f.Name);

        // Start with Check.CommonAssertions before all other files to prepare the Check class
        Console.WriteLine("Merging CommonAssertions of the Check class...");
        var currentFile = allSourceFiles["Check.CommonAssertions.cs"];

        var sourceSyntaxTree = CSharpSyntaxTree.ParseText(currentFile.ReadContent(), csharpParseOptions);
        var checkClassDeclaration = (ClassDeclarationSyntax) sourceSyntaxTree.GetRoot()
                                                                             .DescendantNodes()
                                                                             .First(node => node.IsKind(SyntaxKind.ClassDeclaration));
        checkClassDeclaration = checkClassDeclaration.WithModifiers(checkClassDeclaration.Modifiers.Remove(checkClassDeclaration.Modifiers.First(token => token.IsKind(SyntaxKind.PartialKeyword))));

        // Process all other files
        Console.WriteLine("Merging remaining files...");
        foreach (var fileName in allSourceFiles.Keys)
        {
            if (!CheckIfFileShouldBeProcessed(options, fileName))
                continue;

            currentFile = allSourceFiles[fileName];
            sourceSyntaxTree = CSharpSyntaxTree.ParseText(currentFile.ReadContent(), csharpParseOptions);
            var originalNamespace = DetermineOriginalNamespace(options,
                                                               defaultNamespace,
                                                               currentFile,
                                                               extensionsNamespace,
                                                               exceptionsNamespace,
                                                               jetBrainsNamespace);

            // If the file contains assertions, add it to the existing Check class declaration
            if (originalNamespace == defaultNamespace && currentFile.Name.StartsWith("Check."))
            {
                var classDeclaration = (ClassDeclarationSyntax) sourceSyntaxTree.GetRoot()
                                                                                .DescendantNodes()
                                                                                .First(node => node.IsKind(SyntaxKind.ClassDeclaration));
                checkClassDeclaration = checkClassDeclaration.WithMembers(checkClassDeclaration.Members.AddRange(classDeclaration.Members));
                continue;
            }

            // Else just get the members of the first namespace and add them to the corresponding one
            var sourceCompilationUnit = (CompilationUnitSyntax) sourceSyntaxTree.GetRoot();
            if (sourceCompilationUnit.Members.IsNullOrEmpty())
                continue;

            var membersToAdd = ((FileScopedNamespaceDeclarationSyntax) sourceCompilationUnit.Members[0]).Members;

            var currentlyEditedNamespace = replacedNodes[originalNamespace];
            replacedNodes[originalNamespace] =
                currentlyEditedNamespace
                   .WithMembers(
                        currentlyEditedNamespace.Members.AddRange(membersToAdd));
        }

        // After the Check class declaration is finished, insert it into the default namespace
        var currentDefaultNamespace = replacedNodes[defaultNamespace];
        replacedNodes[defaultNamespace] = currentDefaultNamespace.WithMembers(currentDefaultNamespace.Members.Insert(0, checkClassDeclaration));

        // Update the target compilation unit
        targetRoot = targetRoot.ReplaceNodes(replacedNodes.Keys, (originalNode, _) => replacedNodes[originalNode]).NormalizeWhitespace();

        // Make types internal if necessary
        if (options.ChangePublicTypesToInternalTypes)
        {
            Console.WriteLine("Types are changed from public to internal...");
            var changedTypeDeclarations = new Dictionary<MemberDeclarationSyntax, MemberDeclarationSyntax>();

            foreach (var typeDeclaration in targetRoot.DescendantNodes().Where(node => node.IsKind(SyntaxKind.ClassDeclaration) ||
                                                                                       node.IsKind(SyntaxKind.StructDeclaration) ||
                                                                                       node.IsKind(SyntaxKind.EnumDeclaration) ||
                                                                                       node.IsKind(SyntaxKind.DelegateDeclaration)))
            {
                if (typeDeclaration is BaseTypeDeclarationSyntax typeDeclarationSyntax)
                {
                    var publicModifier = typeDeclarationSyntax.Modifiers[0];
                    var adjustedModifiers = typeDeclarationSyntax.Modifiers
                                                                 .RemoveAt(0)
                                                                 .Insert(0, Token(SyntaxKind.InternalKeyword).WithTriviaFrom(publicModifier));

                    if (typeDeclarationSyntax is ClassDeclarationSyntax classDeclaration)
                        changedTypeDeclarations[classDeclaration] = classDeclaration.WithModifiers(adjustedModifiers);

                    else if (typeDeclarationSyntax is StructDeclarationSyntax structDeclaration)
                        changedTypeDeclarations[structDeclaration] = structDeclaration.WithModifiers(adjustedModifiers);

                    else if (typeDeclarationSyntax is EnumDeclarationSyntax enumDeclaration)
                        changedTypeDeclarations[enumDeclaration] = enumDeclaration.WithModifiers(adjustedModifiers);
                }
                else if (typeDeclaration is DelegateDeclarationSyntax delegateDeclaration)
                {
                    var publicModifier = delegateDeclaration.Modifiers[0];
                    var adjustedModifiers = delegateDeclaration.Modifiers
                                                               .RemoveAt(0)
                                                               .Insert(0, Token(SyntaxKind.InternalKeyword).WithTriviaFrom(publicModifier));
                    changedTypeDeclarations[delegateDeclaration] = delegateDeclaration.WithModifiers(adjustedModifiers);
                }
            }

            targetRoot = targetRoot.ReplaceNodes(changedTypeDeclarations.Keys, (originalNode, _) => changedTypeDeclarations[originalNode]);
        }

        // Remove assertion overloads that incorporate an exception factory if necessary
        if (options.RemoveOverloadsWithExceptionFactory)
        {
            Console.WriteLine("Removing overloads with exception factory...");

            var checkClass = (ClassDeclarationSyntax) targetRoot.DescendantNodes()
                                                                .First(node => node.IsKind(SyntaxKind.ClassDeclaration) &&
                                                                               node is ClassDeclarationSyntax { Identifier.Text: "Check" });

            var membersWithoutExceptionFactory =
                checkClass.Members
                          .Where(member => member is not MethodDeclarationSyntax method || method.ParameterList.Parameters.All(parameter => parameter.Identifier.Text != "exceptionFactory"));
            targetRoot = targetRoot.ReplaceNode(checkClass, checkClass.WithMembers(new SyntaxList<MemberDeclarationSyntax>(membersWithoutExceptionFactory)));

            // Remove members from Throw class that use exception factories
            var throwClass = (ClassDeclarationSyntax) targetRoot.DescendantNodes()
                                                                .First(node => node.IsKind(SyntaxKind.ClassDeclaration) &&
                                                                               node is ClassDeclarationSyntax { Identifier.Text: "Throw" });

            membersWithoutExceptionFactory =
                throwClass.Members
                          .Where(member => member is not MethodDeclarationSyntax method || method.ParameterList.Parameters.All(parameter => parameter.Identifier.Text != "exceptionFactory"));
            targetRoot = targetRoot.ReplaceNode(throwClass, throwClass.WithMembers(new SyntaxList<MemberDeclarationSyntax>(membersWithoutExceptionFactory)));
        }

        var targetFileContent = targetRoot.ToFullString();

        Console.WriteLine("File is cleaned up...");
        targetFileContent = CleanupStep.Cleanup(targetFileContent, options).ToString();

        // Write the target file 
        Console.WriteLine("File is written to disk...");
        File.WriteAllText(options.TargetFile, targetFileContent);
    }

    private static bool CheckIfFileShouldBeProcessed(SourceFileMergeOptions options, string fileName) =>
        fileName != "Check.CommonAssertions.cs" &&
        fileName != "CallerArgumentExpressionAttribute.cs" &&
        (fileName != "ReSharperAnnotations.cs" || options.IncludeJetBrainsAnnotations) &&
        (fileName != "ValidatedNotNullAttribute.cs" || options.IncludeValidatedNotNullAttribute);

    private static NamespaceDeclarationSyntax DetermineOriginalNamespace(SourceFileMergeOptions options,
                                                                         NamespaceDeclarationSyntax defaultNamespace,
                                                                         FileInfo currentFile,
                                                                         NamespaceDeclarationSyntax extensionsNamespace,
                                                                         NamespaceDeclarationSyntax exceptionsNamespace,
                                                                         NamespaceDeclarationSyntax? jetBrainsNamespace)
    {
        var originalNamespace = defaultNamespace;
        switch (currentFile.Directory?.Name)
        {
            case "FrameworkExtensions":
                originalNamespace = extensionsNamespace;
                break;
            case "Exceptions":
                originalNamespace = exceptionsNamespace;
                break;
            default:
            {
                if (options.IncludeJetBrainsAnnotations && currentFile.Name == "ReSharperAnnotations.cs")
                    originalNamespace = jetBrainsNamespace!;
                break;
            }
        }

        return originalNamespace;
    }
}