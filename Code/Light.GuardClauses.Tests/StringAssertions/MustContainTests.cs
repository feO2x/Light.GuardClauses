using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustContainTests
    {
        [Theory]
        [InlineData("Foo", "Bar")]
        [InlineData("abc", "d")]
        [InlineData("Hello, World!", "You")]
        [InlineData("1, 2, 3", ". ")]
        public static void StringDoesNotContain(string @string, string other)
        {
            Action act = () => @string.MustContain(other, nameof(@string));

            var assertion = act.Should().Throw<SubstringException>().Which;
            assertion.Message.Should().Contain($"{nameof(@string)} must contain {other.ToStringOrNull()}, but it actually is {@string.ToStringOrNull()}.");
            assertion.ParamName.Should().BeSameAs(nameof(@string));
        }

        [Theory]
        [InlineData("Foo", "oo")]
        [InlineData("Bar", "Bar")]
        [InlineData("Baz", "B")]
        [InlineData("abc", "a")]
        [InlineData("Hello, World!", "orl")]
        [InlineData("1, 2, 3", ", ")]
        public static void StringContains(string @string, string substring) =>
            @string.MustContain(substring).Should().BeSameAs(@string);

        [Theory]
        [InlineData("Why are all the gods such vicious cunts ? Where is the god of tits and wine?", "beer", StringComparison.OrdinalIgnoreCase)]
        [InlineData("I'm the captain. If this ship goes down, I go down with it.", "SHIP", StringComparison.Ordinal)]
        public static void StringDoesNotContainCustomSearch(string @string, string other, StringComparison comparisonType)
        {
            Action act = () => @string.MustContain(other, comparisonType, nameof(@string));

            var assertion = act.Should().Throw<SubstringException>().Which;
            assertion.Message.Should().Contain($"{nameof(@string)} must contain {other.ToStringOrNull()} ({comparisonType}), but it actually is {@string.ToStringOrNull()}.");
            assertion.ParamName.Should().BeSameAs(nameof(@string));
        }

        [Theory]
        [InlineData("Foo", "foo", StringComparison.OrdinalIgnoreCase)]
        [InlineData("I suppose I'll have to kill the Mountain myself. Won't that make for a great song.", "KILL", StringComparison.OrdinalIgnoreCase)]
        public static void StringContainsCustomSearch(string @string, string substring, StringComparison comparisonType) => 
            @string.MustContain(substring, comparisonType).Should().BeSameAs(@string);

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustContain("Foo");

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void StringNullCustomSearch()
        {
            Action act = () => ((string)null).MustContain("Foo", StringComparison.Ordinal);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("Foo", "Bar")]
        [InlineData("Baz", null)]
        [InlineData(null, "Qux")]
        public static void CustomException(string first, string second) => 
            Test.CustomException(first,
                                 second,
                                 (@string, other, exceptionFactory) => @string.MustContain(other, exceptionFactory));

        [Theory]
        [InlineData("Foo", "foo", StringComparison.Ordinal)]
        [InlineData(null, "Bar", StringComparison.OrdinalIgnoreCase)]
        [InlineData("Baz", null, StringComparison.CurrentCulture)]
        [InlineData("Qux", "Qux", (StringComparison) 42)]
        public static void CustomExceptionCustomSearch(string x, string y, StringComparison comparison) => 
            Test.CustomException(x,
                                 y,
                                 comparison,
                                 (@string, other, comparisonType, exceptionFactory) => @string.MustContain(other, comparisonType, exceptionFactory));

        [Fact]
        public static void CustomMessage() => 
            Test.CustomMessage<SubstringException>(message => "Foo".MustContain("Bar", message: message));

        [Fact]
        public static void CustomMessageParameterNull() => 
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustContain("Foo", message: message));

        [Fact]
        public static void CustomMessageValueNull() => 
            Test.CustomMessage<ArgumentNullException>(message => "Foo".MustContain(null, message: message));

        [Fact]
        public static void CustomMessageCustomSearch() => 
            Test.CustomMessage<SubstringException>(message => "Baz".MustContain("Qux", StringComparison.OrdinalIgnoreCase, message: message));

        [Fact]
        public static void CustomMessageCustomSearchParameterNull() => 
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustContain("Foo", message: message));

        [Fact]
        public static void CustomMessageCustomSearchValueNull() => 
            Test.CustomMessage<ArgumentNullException>(message => "Bar".MustContain(null, message: message));
    }
}