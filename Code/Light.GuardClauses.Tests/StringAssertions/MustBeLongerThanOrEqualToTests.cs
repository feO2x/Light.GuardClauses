using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeLongerThanOrEqualToTests
    {
        [Theory]
        [InlineData("Foo", 3)]
        [InlineData("Bar", 2)]
        [InlineData("", 0)]
        [InlineData("All rulers are either butchers or meat.", 10)]
        public static void LongerThanOrEqualTo(string @string, int length) =>
            @string.MustBeLongerThanOrEqualTo(length).Should().BeSameAs(@string);

        [Theory]
        [InlineData("Baz", 4)]
        [InlineData("Gorge", 10)]
        [InlineData("", 1)]
        [InlineData("He was no dragon. Fire cannot kill a dragon.", 75)]
        public static void Shorter(string @string, int length)
        {
            Action act = () => @string.MustBeLongerThanOrEqualTo(length, nameof(@string));

            act.Should().Throw<StringLengthException>()
               .And.Message.Should().Contain($"string must be longer than or equal to {length}, but it actually has length {@string.Length}.");
        }

        [Fact]
        public static void StringNull()
        {
            Action act = () => ((string) null).MustBeLongerThanOrEqualTo(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException("Foo",
                                 10,
                                 (s, l, exceptionFactory) => s.MustBeLongerThanOrEqualTo(l, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringLengthException>(message => "Bar".MustBeLongerThanOrEqualTo(10, message: message));
    }
}