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

        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar, StringComparison.OrdinalIgnoreCase)]
        [InlineData(Metasyntactic.Baz, Metasyntactic.Qux, StringComparison.CurrentCulture)]
        public static void NotSubstringCustomComparisonType(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustBeSubstringOf(second, comparisonType, nameof(first));

            act.Should().Throw<SubstringException>()
               .And.Message.Should().Contain($"{nameof(first)} must be a substring of \"{second}\" ({comparisonType}), but it actually is \"{first}\".");
        }

        [Theory]
        [InlineData("foo", "Food", StringComparison.OrdinalIgnoreCase)]
        [InlineData("ab", "AB", StringComparison.CurrentCultureIgnoreCase)]
        [InlineData("be drunk", "Isn't it a man's duty to be drunk at his own wedding?", StringComparison.CurrentCulture)]
        public static void SubstringCustomComparisonType(string x, string y, StringComparison comparisonType) =>
            x.MustBeSubstringOf(y, comparisonType).Should().BeSameAs(x);

        [Fact]
        public static void ParameterNull()
        {
            Action act = () => ((string) null).MustBeSubstringOf(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void OtherNull()
        {
            Action act = () => Metasyntactic.Foo.MustBeSubstringOf(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void ParameterNullCustomComparisonType()
        {
            Action act = () => ((string) null).MustBeSubstringOf(Metasyntactic.Bar, StringComparison.Ordinal);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void OtherNullCustomComparisonType()
        {
            Action act = () => Metasyntactic.Baz.MustBeSubstringOf(null, StringComparison.CurrentCultureIgnoreCase);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (s1, s2, exceptionFactory) => s1.MustBeSubstringOf(s2, exceptionFactory));

        [Fact]
        public static void CustomExceptionCustomComparisonType() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 StringComparison.OrdinalIgnoreCase,
                                 (s1, s2, ct, exceptionFactory) => s1.MustBeSubstringOf(s2, ct, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<SubstringException>(message => Metasyntactic.Baz.MustBeSubstringOf(Metasyntactic.Qux, message: message));

        [Fact]
        public static void CustomMessageCustomComparisonType() =>
            Test.CustomMessage<SubstringException>(message => "1".MustBeSubstringOf("OMG", StringComparison.OrdinalIgnoreCase, message: message));
    }
}