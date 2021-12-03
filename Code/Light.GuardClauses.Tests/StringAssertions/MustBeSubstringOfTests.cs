using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeSubstringOfTests
    {
        [Theory]
        [InlineData("Foo", "Bar")]
        [InlineData("FO", "Foo")]
        [InlineData("Hey, you over there!", "Where is the pigeon pie?")]
        public static void NotSubstring(string @string, string other)
        {
            Action act = () => @string.MustBeSubstringOf(other, nameof(@string));

            var assertion = act.Should().Throw<SubstringException>().Which;
            assertion.Message.Should().Contain($"{nameof(@string)} must be a substring of \"{other}\", but it actually is {@string.ToStringOrNull()}.");
            assertion.ParamName.Should().BeSameAs(nameof(@string));
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
        [InlineData("Foo", "Bar", StringComparison.OrdinalIgnoreCase)]
        [InlineData("Baz", "Qux", StringComparison.CurrentCulture)]
        public static void NotSubstringCustomComparisonType(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustBeSubstringOf(second, comparisonType, nameof(first));

            var assertion = act.Should().Throw<SubstringException>().Which;
            assertion.Message.Should().Contain($"{nameof(first)} must be a substring of \"{second}\" ({comparisonType}), but it actually is \"{first}\".");
            assertion.ParamName.Should().BeSameAs(nameof(first));
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
            Action act = () => ((string) null).MustBeSubstringOf("Foo");

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void OtherNull()
        {
            Action act = () => "Foo".MustBeSubstringOf(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void ParameterNullCustomComparisonType()
        {
            Action act = () => ((string) null).MustBeSubstringOf("Bar", StringComparison.Ordinal);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void OtherNullCustomComparisonType()
        {
            Action act = () => "Baz".MustBeSubstringOf(null!, StringComparison.CurrentCultureIgnoreCase);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Foo", "Bar")]
        [InlineData(null, "Baz")]
        [InlineData("Qux", null)]
        public static void CustomException(string first, string second) =>
            Test.CustomException(first,
                                 second,
                                 (s1, s2, exceptionFactory) => s1.MustBeSubstringOf(s2, exceptionFactory));

        [Theory]
        [InlineData("Foo", "FOO", StringComparison.Ordinal)]
        [InlineData("Bar", null, StringComparison.OrdinalIgnoreCase)]
        [InlineData(null, "Baz", StringComparison.CurrentCulture)]
        [InlineData("Qux", "Quux", (StringComparison) 509)]
        public static void CustomExceptionCustomComparisonType(string first, string second, StringComparison comparisonType) =>
            Test.CustomException(first,
                                 second,
                                 comparisonType,
                                 (s1, s2, ct, exceptionFactory) => s1.MustBeSubstringOf(s2, ct, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() =>
            "Foo".MustBeSubstringOf("Food", (_, _) => new Exception()).Should().BeSameAs("Foo");

        [Fact]
        public static void NoCustomExceptionThrownWithCustomComparisonType() =>
            "Bar".MustBeSubstringOf("Bar", (_, _) => new Exception()).Should().BeSameAs("Bar");

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<SubstringException>(message => "Baz".MustBeSubstringOf("Qux", message: message));

        [Fact]
        public static void CustomMessageParameterNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeSubstringOf("Foo", message: message));

        [Fact]
        public static void CustomMessageValueNull() =>
            Test.CustomMessage<ArgumentNullException>(message => "Foo".MustBeSubstringOf(null!, message: message));

        [Fact]
        public static void CustomMessageCustomComparisonType() =>
            Test.CustomMessage<SubstringException>(message => "1".MustBeSubstringOf("OMG", StringComparison.OrdinalIgnoreCase, message: message));

        [Fact]
        public static void CustomMessageCustomComparisonTypeParameterNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeSubstringOf("Foo", StringComparison.CurrentCulture, message: message));

        [Fact]
        public static void CustomMessageCustomComparisonTypeValueNull() =>
            Test.CustomMessage<ArgumentNullException>(message => "Foo".MustBeSubstringOf(null!, StringComparison.Ordinal, message: message));
    }
}