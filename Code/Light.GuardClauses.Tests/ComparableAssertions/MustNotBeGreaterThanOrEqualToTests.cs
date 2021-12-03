using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustNotBeGreaterThanOrEqualToTests
    {
        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        [InlineData(-87, -88)]
        public static void ParameterAtOrAboveBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary, nameof(value));

            var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
            exceptionAssertion.Message.Should().Contain($"{nameof(value)} must not be greater than or equal to {boundary}, but it actually is {value}.");
            exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
        }

        [Theory]
        [InlineData(0L, 1L)]
        [InlineData(-80L, -70L)]
        public static void ParameterBelowBoundary(long value, long boundary) => value.MustNotBeGreaterThanOrEqualTo(boundary).Should().Be(value);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(20, 10, (x, y, exceptionFactory) => x.MustNotBeGreaterThanOrEqualTo(y, exceptionFactory));

        [Fact]
        public static void CustomExceptionParameterNull() => 
            Test.CustomException((string) null,
                                 "Foo",
                                 (x, y, exceptionFactory) => x.MustNotBeGreaterThanOrEqualTo(y, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() => 5m.MustNotBeGreaterThanOrEqualTo(5.1m, (_, _) => null).Should().Be(5m);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 300.MustNotBeGreaterThanOrEqualTo(300, message: message));

        [Fact]
        public static void CustomMessageParameterNull() => 
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeGreaterThanOrEqualTo("Bar", message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var four = 4;

            Action act = () => four.MustNotBeGreaterThanOrEqualTo(4);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be(nameof(four));
        }
    }
}