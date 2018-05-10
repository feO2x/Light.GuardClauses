using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustBeLessThanOrEqualToTests
    {
        [Theory]
        [InlineData(10, 9)]
        [InlineData(-42, -1888)]
        public static void ParameterGreater(int first, int second)
        {
            Action act = () => first.MustBeLessThanOrEqualTo(second, nameof(first));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be less than or equal to {second}, but it actually is {first}.");
        }

        [Theory]
        [InlineData(13, 13)]
        [InlineData(12, 13)]
        public static void ParameterLessOrEqual(int first, int second) => first.MustBeLessThanOrEqualTo(second).Should().Be(first);

        [Fact]
        public static void ThrowCustomExceptionWithTwoParameters() =>
            Test.CustomException(20, 19, (x, y, exceptionFactory) => x.MustBeLessThanOrEqualTo(y, exceptionFactory));


        [Fact]
        public static void NoCustomExceptionThrown() => 5m.MustBeLessThanOrEqualTo(5.1m, (v, b) => null).Should().Be(5m);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 'c'.MustBeLessThanOrEqualTo('a', message: message));
    }
}