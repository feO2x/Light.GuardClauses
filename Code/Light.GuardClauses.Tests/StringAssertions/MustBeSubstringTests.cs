using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeSubstringTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar)]
        [InlineData("FO", Metasyntactic.Foo)]
        [InlineData("Hey, you over there!", "Where is the pigeon pie?")]
        public static void NotSubstring(string @string, string other)
        {
            Action act = () => @string.MustBeSubstringOf(other, nameof(@string));

            act.Should().Throw<SubstringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must be a substring of \"{other}\", but it actually is {@string.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData("Foo", "Food")]
        [InlineData("123", "123")]
        [InlineData("", "Any string can contain the empty string")]
        [InlineData("bar", "Let's go to the bar and have a drink.")]
        [InlineData("I'll have you", "If you ever call me sister again, I'll have you strangled in your sleep.")]
        public static void Substring(string substring, string other) =>
            substring.MustBeSubstringOf(other).Should().BeSameAs(substring);

        [Fact]
        public static void ParameterNull()
        {
            Action act = () => Metasyntactic.Foo.MustBeSubstringOf(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void OtherNull()
        {
            Action act = () => Metasyntactic.Foo.MustBeSubstringOf(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (s1, s2, exceptionFactory) => s1.MustBeSubstringOf(s2, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<SubstringException>(message => Metasyntactic.Baz.MustBeSubstringOf(Metasyntactic.Qux, message: message));
    }
}