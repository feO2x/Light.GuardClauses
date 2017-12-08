using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class IsNullOrEmptyTests
    {
        [Fact(DisplayName = "IsNullOrEmpty must return true when the specified string is null.")]
        public void StringNull()
        {
            string @string = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = @string.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsNullOrEmpty must return true when the specified string is an empty one.")]
        public void StringEmpty()
        {
            var result = string.Empty.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsNullOrEmpty must return false when the specified string is not empty.")]
        [InlineData("abcdef")]
        [InlineData("\t")]
        [InlineData("  ")]
        [InlineData("\u2028")]
        public void StringNotEmpty(string @string)
        {
            var result = @string.IsNullOrEmpty();

            result.Should().BeFalse();
        }
    }
}