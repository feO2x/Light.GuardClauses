using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class ContainsOnlyLettersTests
    {
        [Theory(DisplayName = "ContainsOnlyLetters must return true when the specified string contains only uppercase or lowercase characters.")]
        [InlineData("abcd")]
        [InlineData("AjOOp")]
        [InlineData("z")]
        [InlineData("ONLYUPPERCASE")]
        public void OnlyLetters(string @string)
        {
            var result = @string.ContainsOnlyLetters();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "ContainsOnlyLetters must return false when the specified string contains other characters than letters (like white space, special characters or numbers).")]
        [InlineData("This sentence contains white space")]
        [InlineData("P7ssw0rd!")]
        [InlineData("\tHere is other white space\u2028")]
        [InlineData("")]
        public void OtherCharacters(string @string)
        {
            var result = @string.ContainsOnlyLetters();

            result.Should().BeFalse();
        }
    }
}