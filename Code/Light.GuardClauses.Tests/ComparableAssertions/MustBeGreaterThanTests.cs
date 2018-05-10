using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustBeGreaterThanTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        public static void ParameterEqualOrLess(int first, int second)
        {
            Action act = () => first.MustBeGreaterThan(second, nameof(first));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be greater than {second}, but it actually is {first}.");
        }

        [Theory]
        [InlineData(133, 99)]
        [InlineData(int.MaxValue, int.MaxValue - 1)]
        [InlineData(byte.MaxValue, byte.MinValue)]
        public static void ParameterGreater(int first, int second) => first.MustBeGreaterThan(second).Should().Be(first);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(40, 50, (x, y, exceptionFactory) => x.MustBeGreaterThan(y, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() => 5.6m.MustBeGreaterThan(5.1m, (v, b) => null).Should().Be(5.6m);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 100.MustBeGreaterThan(100, message: message));

    }
}