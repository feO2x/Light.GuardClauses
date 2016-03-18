using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public class MustMatchTests
    {
        [Fact(DisplayName = "MustMatch must throw an exception when the specified string does not match the regular expression.")]
        public void StringDoesNotMatch()
        {
            var pattern = new Regex(@"\d{5}");
            const string @string = "12c45";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.ShouldThrow<StringDoesNotMatchException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustMatch must not throw an exception when the specified string matches the regular expression.")]
        public void StringMatches()
        {
            var pattern = new Regex(@"\w{5}");
            const string @string = "abcde";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustMatch must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall match the pattern";

            Action act = () => "12345".MustMatch(new Regex(@"\W{5}"), message: message);

            act.ShouldThrow<StringDoesNotMatchException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustMatch must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => "12345".MustMatch(new Regex(@"\W{5}"), exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}
