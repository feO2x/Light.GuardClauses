using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeShorterThanTests
    {
        [Theory]
        [InlineData("Foo", 4)]
        [InlineData("Once you've accepted your flaws, no one can use them against you.", 70)]
        [InlineData("", 1)]
        public static void IsShorterThan(string @string, int length) =>
            @string.MustBeShorterThan(length).Should().BeSameAs(@string);

        [Theory]
        [InlineData("Bar", 3)]
        [InlineData("Baz", 2)]
        [InlineData("The great thing about reading is that it broadens your life", 20)]
        public static void NotShorterThan(string @string, int length)
        {
            Action act = () => @string.MustBeShorterThan(length, nameof(@string));

            act.Should().Throw<StringLengthException>()
               .And.Message.Should().Contain($"string must be shorter than {length}, but it actually has length {@string.Length}.");
        }

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustBeShorterThan(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException("Foo",
                                 2,
                                 (s, l, exceptionFactory) => s.MustBeShorterThan(l, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringLengthException>(message => "Foo".MustBeShorterThan(1, message: message));
    }
}