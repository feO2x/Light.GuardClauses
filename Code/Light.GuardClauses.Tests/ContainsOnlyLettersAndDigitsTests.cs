using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class ContainsOnlyLettersAndDigitsTests
    {
        [Theory(DisplayName = "ContainsOnlyLettersAndDigits must return true when the specified string contains only letters and digit characters.")]
        [InlineData("abcd")]
        [InlineData("1234")]
        [InlineData("ZUFK455OPP")]
        [InlineData("UIUIUI636363")]
        public void OnlyLettersAndDigits(string @string)
        {
            var result = @string.ContainsOnlyLettersAndDigits();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "ContainsOnlyLettersAndDigits must return false when the specified string contains other characters like white space or special characters.")]
        [InlineData("abcde!")]
        [InlineData("This sentence contains white spac3.")]
        [InlineData("How $pecial is th!s?")]
        [InlineData("")]
        public void OtherCharacters(string @string)
        {
            var result = @string.ContainsOnlyLettersAndDigits();

            result.Should().BeFalse();
        }
    }
}