using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Light.GuardClauses.InternalRoslynAnalyzers.Tests
{
    public static class ParameterNameAndMessageCommentsTests
    {
        private static readonly DiagnosticAnalyzer Analyzer = new DefaultParameterNameAndMessageXmlComments();
        private static readonly CodeFixProvider ParameterNameCodeFix = new FixParameterNameXmlComment();

        private const string MissingParameterNameCode = @"
using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses
{
    public static class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise throws an <see cref=""ArgumentNullException""/>.
        /// </summary>
        /// <param name=""parameter"">The reference to be checked.</param>
        /// <param name=""parameterName""></param>
        /// <param name=""message"">The message that will be injected in the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter""/> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                throw new ArgumentNullException();
            return parameter;
        }
    }
}";

        private const string FixedParameterNameCode = @"
using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses
{
    public static class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise throws an <see cref=""ArgumentNullException""/>.
        /// </summary>
        /// <param name=""parameter"">The reference to be checked.</param>
        /// <param name=""parameterName"">The name of the parameter (optional).</param>
        /// <param name=""message"">The message that will be injected in the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter""/> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                throw new ArgumentNullException();
            return parameter;
        }
    }
}";

        [Fact]
        public static async Task AnalyzeParameterName()
        {
            var result = await Analyzer.AnalyzeAsync(MissingParameterNameCode);

            result.Should().ContainSingle(diagnostic => ReferenceEquals(diagnostic.Descriptor, DiagnosticDescriptors.NonDefaultXmlCommentForParameterName));
        }

        [Fact]
        public static async Task FixParameterName()
        {
            var resultingCode = await ParameterNameCodeFix.ApplyFixAsync(MissingParameterNameCode, Analyzer);

            resultingCode.Should().Be(FixedParameterNameCode);
        }

        [Fact]
        public static async Task AnalyzeMessage()
        {
            const string code = @"
using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses
{
    public static class CommonAssertions
    {
        /// <summary>
        /// Ensures that the specified reference is not null, or otherwise throws an <see cref=""ArgumentNullException""/>.
        /// </summary>
        /// <param name=""parameter"">The reference to be checked.</param>
        /// <param name=""parameterName"">The name of the parameter (optional).</param>
        /// <param name=""message""></param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter""/> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                throw new ArgumentNullException();
            return parameter;
        }
    }
}";
            var result = await Analyzer.AnalyzeAsync(code);

            result.Should().ContainSingle(diagnostic => ReferenceEquals(diagnostic.Descriptor, DiagnosticDescriptors.NonDefaultXmlCommentForMessage));
        }
    }
}