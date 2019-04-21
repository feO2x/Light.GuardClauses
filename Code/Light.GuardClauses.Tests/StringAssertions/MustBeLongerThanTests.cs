using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeLongerThanTests
    {
        [Theory]
        [InlineData("Foo", 2)]
        [InlineData("Gorge", 3)]
        [InlineData("", -1)]
        [InlineData("If you think this has a happy ending, you haven’t been paying attention.", 69)]
        public static void LongerThan(string @string, int length) =>
            @string.MustBeLongerThan(length).Should().BeSameAs(@string);

        [Theory]
        [InlineData("Bar", 3)]
        [InlineData("Baz", 4)]
        [InlineData("", 0)]
        [InlineData("Hush, Hodor. No more Hodor-ing!", 100)]
        public static void NotLongerThan(string @string, int length)
        {
            Action act = () => @string.MustBeLongerThan(length, nameof(@string));

            act.Should().Throw<StringLengthException>()
               .And.Message.Should().Contain($"string must be longer than {length}, but it actually has length {@string.Length}.");
        }

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustBeLongerThan(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException("Foo",
                                 5,
                                 (s, l, exceptionFactory) => s.MustBeLongerThan(l, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringLengthException>(message => "Foo".MustBeLongerThan(10, message: message));
    }
}