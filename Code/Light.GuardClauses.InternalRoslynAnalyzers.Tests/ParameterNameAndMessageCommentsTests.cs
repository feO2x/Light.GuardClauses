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
        /// <param name=""parameterName""></param>
        /// <param name=""message"">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
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

        private const string FixedParameterNameComment = @"
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
        /// <param name=""message"">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
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
            var result = await Analyzer.AnalyzeAsync(MissingParameterNameComment);

            result.Should().ContainSingle(diagnostic => ReferenceEquals(diagnostic.Descriptor, Descriptors.ParameterNameComment));
        }

        [Fact]
        public static async Task FixParameterName()
        {
            var codeFix = new FixParameterNameXmlComment();
            var resultingCode = await codeFix.ApplyFixAsync(MissingParameterNameComment, Analyzer);

            resultingCode.Should().Be(FixedParameterNameComment);
        }

        private const string MissingMessageWithExistingException = @"
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
        /// <param name=""message""></param>
        /// <exception cref=""ArgumentNullException"">Thrown when <paramref name=""parameter"" /> is null.</exception>
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

        private const string FixedMessageCommentWithExistingException = @"
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
        public static T MustNotBeNull<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                throw new ArgumentNullException();
            return parameter;
        }
    }
}";

        [Fact]
        public static async Task AnalyzeMissingMessage()
        {
            var result = await Analyzer.AnalyzeAsync(MissingMessageWithExistingException);

            result.Should().ContainSingle(diagnostic => ReferenceEquals(diagnostic.Descriptor, Descriptors.MessageComment));
        }

        [Fact]
        public static async Task FixMissingMessageWithExistingExceptionComment()
        {
            var codeFix = new FixMessageXmlComment();
            var resultingCode = await codeFix.ApplyFixAsync(MissingMessageWithExistingException, Analyzer);

            resultingCode.Should().Be(FixedMessageCommentWithExistingException);
        }

        [Fact]
        public static async Task AnalyzeFixedMessageCode()
        {
            var result = await Analyzer.AnalyzeAsync(FixedMessageCommentWithExistingException);

            result.Should().BeEmpty();
        }
    }
}