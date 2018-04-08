using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Light.GuardClauses.InternalRoslynAnalyzers.Tests
{
    public static class CustomExceptionOverloadTests
    {
        private static readonly DiagnosticAnalyzer Analyzer = new OverloadAnalyzer();

        private const string MissingOverload = @"
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    public static class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise throws an <see cref=""ArgumentNullException"" />.
        /// </summary>
        /// <param name=""parameter"">The reference to be checked.</param>
        /// <param name=""parameterName"">The name of the parameter (optional).</param>
        /// <param name=""message"">The message that will be passed to the <see cref=""ArgumentNullException"" /> (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
    }
}";

        private const string FixedOverload = @"
using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    public static class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise throws an <see cref=""ArgumentNullException"" />.
        /// </summary>
        /// <param name=""parameter"">The reference to be checked.</param>
        /// <param name=""parameterName"">The name of the parameter (optional).</param>
        /// <param name=""message"">The message that will be passed to the <see cref=""ArgumentNullException"" /> (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null) where T : class
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise throws your custom exception.
        /// </summary>
        /// <param name=""parameter"">The reference to be checked.</param>
        /// <param name=""exceptionFactory"">The delegate that creates your custom exception.</param>
        /// <exception cref=""Exception"">Your custom exception thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, Func<Exception> exceptionFactory) where T : class
        {
            if (parameter == null)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }
    }
}";

        [Fact]
        public static async Task AnalyzeMissingOverload()
        {
            var diagnostics = await Analyzer.AnalyzeAsync(MissingOverload);

            diagnostics.Should().HaveCount(1);
            diagnostics[0].Descriptor.Should().BeSameAs(Descriptors.CustomExceptionOverload);
        }

        [Fact]
        public static async Task AnalyzePresentOverload()
        {
            var diagnostics = await Analyzer.AnalyzeAsync(FixedOverload);

            diagnostics.Should().BeEmpty();
        }

        [Fact]
        public static async Task TypeSymbols()
        {
            const string someCode = "namespace Foo { public class Bar { } }";
            var (_, compilation) = await RoslynExtensions.CreateLightGuardClausesDllInMemory("FooProject", someCode);
            var exception = compilation.GetTypeByMetadataName(typeof(Exception).FullName);
            exception.Should().NotBeNull();
            var funcOfT = compilation.GetTypeByMetadataName(typeof(Func<>).FullName);
            funcOfT.Should().NotBeNull();
        }

        [Fact]
        public static async Task CompareTwoTypes()
        {
            const string someCode = "namespace System { public class Object { } }";
            var (document, compilation) = await RoslynExtensions.CreateLightGuardClausesDllInMemory("FooProject", someCode);

            var syntaxTree = await document.GetSyntaxTreeAsync();
            var classDeclaration = syntaxTree.GetRoot()
                                  .DescendantNodes()
                                  .OfType<ClassDeclarationSyntax>()
                                  .Single();
            var localObjectSymbol = compilation.GetSemanticModel(syntaxTree)
                                               .GetDeclaredSymbol(classDeclaration);


            var generalObjectType = typeof(object);
            localObjectSymbol.MetadataName.Should().Be(generalObjectType.Name);
            localObjectSymbol.ContainingNamespace.MetadataName.Should().Be(generalObjectType.Namespace);
            localObjectSymbol.ContainingAssembly.MetadataName.Should().NotBe(generalObjectType.Assembly.FullName);
        }


    }
}