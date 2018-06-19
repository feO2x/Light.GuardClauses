using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustNotBeSubstringTests
    {
        [Theory]
        [InlineData("123", " 12345 ")]
        [InlineData("friend", "Say herro to my littre friend")]
        public static void Substring(string x, string y)
        {
            Action act = () => x.MustNotBeSubstringOf(y, nameof(x));

            act.Should().Throw<SubstringException>()
               .And.Message.Should().Contain($"{nameof(x)} must not be a substring of \"{y}\", but it actually is \"{x}\".");
        }

        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar)]
        [InlineData("Love and peace", "When you play the game of thrones you win, or you die.There is no middle ground.")]
        public static void NotSubstring(string first, string second) =>
            first.MustNotBeSubstringOf(second).Should().BeSameAs(first);

        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Foo, StringComparison.Ordinal)]
        [InlineData("bar", "Bar", StringComparison.OrdinalIgnoreCase)]
        [InlineData("I am", "I AM YOUR MASTER", StringComparison.CurrentCultureIgnoreCase)]
        public static void SubstringCustomComparison(string s1, string s2, StringComparison comparisonType)
        {
            Action act = () => s1.MustNotBeSubstringOf(s2, comparisonType, nameof(s1));

            act.Should().Throw<SubstringException>()
               .And.Message.Should().Contain($"{nameof(s1)} must not be a substring of \"{s2}\" ({comparisonType}), but it actually is \"{s1}\".");
        }

        [Theory]
        [InlineData("light", "Where is the LIGHT?", StringComparison.Ordinal)]
        [InlineData("Pawned", "Pwnd", StringComparison.CurrentCulture)]
        [InlineData("drink", "Let's go to the mall", StringComparison.CurrentCultureIgnoreCase)]
        public static void NotSubstringCustomComparison(string a, string b, StringComparison comparisonType) => 
            a.MustNotBeSubstringOf(b, comparisonType).Should().BeSameAs(a);

        [Fact]
        public static void ParameterNull()
        {
            Action act = () => ((string) null).MustNotBeSubstringOf(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void ValueNull()
        {
            Action act = () => Metasyntactic.Bar.MustNotBeSubstringOf(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void ParameterNullCustomComparison()
        {
            Action act = () => ((string) null).MustNotBeSubstringOf(Metasyntactic.Foo, StringComparison.OrdinalIgnoreCase);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void ValueNullCustomComparison()
        {
            Action act = () => Metasyntactic.Baz.MustNotBeSubstringOf(null, StringComparison.CurrentCulture);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() => 
            Test.CustomException("drink",
                                 "I'll have a drink right now",
                                 (s1, s2, exceptionFactory) => s1.MustNotBeSubstringOf(s2, exceptionFactory));

        [Fact]
        public static void CustomExceptionCustomComparison() => 
            Test.CustomException("Full of Possibilities",
                                 "Death is so terribly final, while life is full of possibilities",
                                 StringComparison.OrdinalIgnoreCase,
                                 (s1, s2, ct, exceptionFactory) => s1.MustNotBeSubstringOf(s2, ct, exceptionFactory));

        [Fact]
        public static void CustomMessage() => 
            Test.CustomMessage<SubstringException>(message => "Foo".MustNotBeSubstringOf("Food", message: message));

        [Fact]
        public static void CustomMessageCustomComparison() => 
            Test.CustomMessage<SubstringException>(message => "Bar".MustNotBeSubstringOf("Walk into a bar", StringComparison.OrdinalIgnoreCase, message: message));
    }
}