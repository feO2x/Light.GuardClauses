using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class IsNullOrWhiteSpaceTests
    {
        [Fact(DisplayName = "IsNullOrWhiteSpace must return true when the specified string is null.")]
        public void StringNull()
        {
            string @string = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = @string.IsNullOrWhiteSpace();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsNullOrWhiteSpace must return true when the specified string is an empty one.")]
        public void StringEmpty()
        {
            var result = string.Empty.IsNullOrWhiteSpace();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsNullOrWhiteSpace must return true when the specified string contains only white space characters.")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        [InlineData("\u2028")]
        public void StringWhiteSpace(string whiteSpaceString)
        {
            var result = whiteSpaceString.IsNullOrWhiteSpace();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsNullOrWhiteSpace must return false when the specified string is not empty and contains characters other than white space.")]
        [InlineData("Foo")]
        [InlineData(" BAR")]
        [InlineData("\tBaz")]
        public void StringNotWhiteSpaceOrEmpty(string @string)
        {
            var result = @string.IsNullOrWhiteSpace();

            result.Should().BeFalse();
        }
    }
}