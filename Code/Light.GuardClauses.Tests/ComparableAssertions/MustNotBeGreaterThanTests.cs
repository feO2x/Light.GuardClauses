using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustNotBeGreaterThanTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        [InlineData(-88, -100)]
        public static void ParamterAboveBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeGreaterThan(boundary, nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than {boundary}, but it actually is {value}.");
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(-88, 0)]
        public static void ParameterAtOrBelowBoundary(short value, short boundary) => value.MustNotBeGreaterThan(boundary, nameof(value)).Should().Be(value);

        [Fact]
        public static void ThrowCustomExceptionWithTwoParameters() =>
            Test.CustomException(15, 10, (x, y, exceptionFactory) => x.MustNotBeGreaterThan(y, exceptionFactory));


        [Fact]
        public static void NoCustomExceptionThrown() => 5m.MustNotBeGreaterThan(5.1m, (v, b) => null).Should().Be(5m);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 21.MustNotBeGreaterThan(20, message: message));
    }
}