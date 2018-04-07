using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Light.GuardClauses.InternalRoslynAnalyzers.Tests
{
    public static class CustomExceptionOverloadTests
    {
        private static readonly DiagnosticAnalyzer Analyzer = new OverloadAnalyzer();

        private const string MissingOverload = @"
using System;
using System.Runtime.CompilerServices;

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
                throw new ArgumentNullException();
            return parameter;
        }
    }
}";

        [Fact]
        public static async Task AnalyzeMissingOverload()
        {
            var result = await Analyzer.AnalyzeAsync(MissingOverload);

            result.Should().HaveCount(1);
            result[0].Descriptor.Should().BeSameAs(Descriptors.CustomExceptionOverload);
        }
    }
}