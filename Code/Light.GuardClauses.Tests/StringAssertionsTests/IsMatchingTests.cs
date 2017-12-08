using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class IsMatchingTests
    {
        [Fact(DisplayName = "IsMatching must return false when the specified string does not match the regular expression.")]
        public void StringDoesNotMatch()
        {
            var pattern = new Regex(@"\d{5}");
            const string @string = "143L6";

            var result = @string.IsMatching(pattern);

            result.Should().BeFalse();
        }

        [Fact(DisplayName = "IsMatching must return true when the specified string matches the regular expression.")]
        public void StringMatches()
        {
            var pattern = new Regex(@"\d{5}");
            const string @string = "12345";

            var result = @string.IsMatching(pattern);

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsMatching must throw an ArgumentNullException when string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).IsMatching(new Regex(@"\d{2}"));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("string");
        }

        [Fact(DisplayName = "IsMatching must throw an ArgumentNullException when pattern is null.")]
        public void RegexNull()
        {
            Action act = () => "foo".IsMatching(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("pattern");
        }
    }
}