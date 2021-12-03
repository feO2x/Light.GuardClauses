using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustNotBeLessThanOrEqualToTests
    {
        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(-80, -40)]
        public static void ParameterAtOrBelowBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeLessThanOrEqualTo(boundary, nameof(value));

            var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
            exceptionAssertion.Message.Should().Contain($"{nameof(value)} must not be less than or equal to {boundary}, but it actually is {value}.");
            exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
        }

        [Theory]
        [InlineData(5L, 4L)]
        [InlineData(100L, 1L)]
        [InlineData(-87L, -90L)]
        public static void ParameterAboveBoundary(long value, long boundary)
        {
            var result = value.MustNotBeLessThanOrEqualTo(boundary, nameof(value));

            result.Should().Be(value);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(15, 16, (x, y, exceptionFactory) => x.MustNotBeLessThanOrEqualTo(y, exceptionFactory));

        [Fact]
        public static void CustomExceptionParameterNull() =>
            Test.CustomException((string) null,
                                 "Foo",
                                 (x, y, exceptionFactory) => x.MustNotBeLessThanOrEqualTo(y, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() => 5.6m.MustNotBeLessThanOrEqualTo(5.1m, (_, _) => null).Should().Be(5.6m);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 15.MustNotBeLessThanOrEqualTo(15, message: message));

        [Fact]
        public static void CustomMessageParameterNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeLessThanOrEqualTo("Bar", message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var nine = 9;

            Action act = () => nine.MustNotBeLessThanOrEqualTo(12);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be(nameof(nine));
        }
    }
}