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

            act.Should().Throw<SubstringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must contain {other.ToStringOrNull()}, but it actually is {@string.ToStringOrNull()}.");
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

            act.Should().Throw<SubstringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must contain {other.ToStringOrNull()} ({comparisonType}), but it actually is {@string.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData("Foo", "foo", StringComparison.OrdinalIgnoreCase)]
        [InlineData("I suppose I'll have to kill the Mountain myself. Won't that make for a great song.", "KILL", StringComparison.OrdinalIgnoreCase)]
        public static void StringContainsCustomSearch(string @string, string substring, StringComparison comparisonType) => 
            @string.MustContain(substring, comparisonType).Should().BeSameAs(@string);

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustContain(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void StringNullCustomSearch()
        {
            Action act = () => ((string)null).MustContain(Metasyntactic.Foo, StringComparison.Ordinal);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() => 
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (@string, other, exceptionFactory) => @string.MustContain(other, exceptionFactory));

        [Fact]
        public static void CustomExceptionCustomSearch() => 
            Test.CustomException("Foo",
                                 "foo",
                                 StringComparison.Ordinal,
                                 (@string, other, comparisonType, exceptionFactory) => @string.MustContain(other, comparisonType, exceptionFactory));

        [Fact]
        public static void CustomMessage() => 
            Test.CustomMessage<SubstringException>(message => Metasyntactic.Foo.MustContain(Metasyntactic.Bar, message: message));

        [Fact]
        public static void CustomMessageCustomSearch() => 
            Test.CustomMessage<SubstringException>(message => Metasyntactic.Baz.MustContain(Metasyntactic.Qux, StringComparison.OrdinalIgnoreCase, message: message));
    }
}