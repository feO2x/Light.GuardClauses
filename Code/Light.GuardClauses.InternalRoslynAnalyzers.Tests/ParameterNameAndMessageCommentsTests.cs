using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace Light.GuardClauses.InternalRoslynAnalyzers.Tests
{
    public static class ParameterNameAndMessageCommentsTests
    {
        private static readonly DiagnosticAnalyzer Analyzer = new XmlCommentAnalyzer();

        private const string MissingParameterNameComment = @"
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
        /// <param name=""parameterName""></param>
        /// <param name=""message"">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
    }
}";

        private const string FixedParameterNameComment = @"
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
        /// <param name=""message"">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
    }
}";

        [Fact]
        public static async Task AnalyzeParameterName()
        {
            var diagnostics = await Analyzer.AnalyzeAsync(MissingParameterNameComment);

            diagnostics.Should().HaveCount(1);
            diagnostics[0].Descriptor.Should().BeSameAs(Descriptors.ParameterNameComment);
        }

        [Fact]
        public static async Task FixParameterName()
        {
            var codeFix = new ParameterNameXmlCommentFix();
            var resultingCode = await codeFix.ApplyFixAsync(MissingParameterNameComment, Analyzer);

            resultingCode.Should().Be(FixedParameterNameComment);
        }

        private const string MissingMessageWithExistingException = @"
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
        /// <param name=""message""></param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
    }
}";

        private const string FixedMessageCommentWithExistingException = @"
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
        /// <param name=""message"">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
#if !(NET40 || NET35 || NET35_CF)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.MustNotBeNull(parameterName, message);
            return parameter;
        }
    }
}";

        [Fact]
        public static async Task AnalyzeMissingMessage()
        {
            var diagnostics = await Analyzer.AnalyzeAsync(MissingMessageWithExistingException);

            diagnostics.Should().HaveCount(1);
            diagnostics[0].Descriptor.Should().BeSameAs(Descriptors.MessageComment);
        }

        [Fact]
        public static async Task FixMissingMessageWithExistingExceptionComment()
        {
            var codeFix = new MessageXmlCommentFix();
            var resultingCode = await codeFix.ApplyFixAsync(MissingMessageWithExistingException, Analyzer);

            resultingCode.Should().Be(FixedMessageCommentWithExistingException);
        }

        [Fact]
        public static async Task AnalyzeFixedMessageCode()
        {
            var diagnostics = await Analyzer.AnalyzeAsync(FixedMessageCommentWithExistingException);

            diagnostics.Should().BeEmpty();
        }
    }
}