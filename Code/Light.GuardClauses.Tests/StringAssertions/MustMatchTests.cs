using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustMatchTests
    {
        [Fact]
        public static void StringDoesNotMatch()
        {
            var pattern = new Regex(@"\d{5}");
            const string @string = "12c45";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.Should().Throw<StringDoesNotMatchException>()
               .And.Message.Should().Contain($"{nameof(@string)} must match the regular expression \"{pattern}\", but it actually is \"{@string}\".");
        }

        [Fact]
        public static void StringMatches()
        {
            var pattern = new Regex(@"\w{5}");
            const string @string = "abcde";

            var result = @string.MustMatch(pattern, nameof(@string));

            result.Should().BeSameAs(@string);
        }

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustMatch(new Regex("Foo"));

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException("ab",
                                 new Regex(@"\w{3}"),
                                 (@string, regex, exceptionFactory) => @string.MustMatch(regex, exceptionFactory));
        

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringDoesNotMatchException>(message => "abcde".MustMatch(new Regex("Foo"), message:message));
        
    }
}