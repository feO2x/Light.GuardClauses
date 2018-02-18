using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustNotBeGreaterThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must throw an exception when the specified value is greater than or equal to the given boundary.")]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        [InlineData(-87, -88)]
        public void ParameterAtOrAboveBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary, nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than or equal to {boundary}, but it actually is {value}.");
        }

        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must not throw an exception when the specified value is less than the given boundary.")]
        [InlineData(0L, 1L)]
        [InlineData(-80L, -70L)]
        public void ParameterBelowBoundary(long value, long boundary)
        {
            var result = value.MustNotBeGreaterThanOrEqualTo(boundary);

            result.Should().Be(value);
        }

        [Fact(DisplayName = "MustNotBeGreaterThanOrEqualTo must throw the custom exception with single parameter when parameter is not less than the boundary value.")]
        public static void ThrowCustomExceptionWithOneParameter()
        {
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => 4.MustNotBeGreaterThanOrEqualTo(2, v =>
            {
                recordedValue = v;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(4);
        }

        [Fact(DisplayName = "MustNotBeGreaterThanOrEqualTo must not throw the custom exception with single parameter when parameter is less than the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 8.MustNotBeGreaterThanOrEqualTo(15, v => null);

            result.Should().Be(8);
        }

        [Fact(DisplayName = "MustNotBeGreaterThanOrEqualTo must throw the custom exception with two parameters when parameter is not less than the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 1656.MustNotBeGreaterThanOrEqualTo(421, (v, b) =>
            {
                recordedParameter = v;
                recordedBoundary = b;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(1656);
            recordedBoundary.Should().Be(421);
        }

        [Fact(DisplayName = "MustNotBeGreaterThanOrEqualTo must not throw the custom exception with two parameters when parameter is less than the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5m.MustNotBeGreaterThanOrEqualTo(5.1m, (v, b) => null);

            result.Should().Be(5m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeGreaterThanOrEqualTo(41, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeGreaterThanOrEqualTo(42, message: message)));
        }
    }
}