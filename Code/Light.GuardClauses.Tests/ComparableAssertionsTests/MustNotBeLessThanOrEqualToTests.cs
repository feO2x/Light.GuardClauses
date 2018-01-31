using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustNotBeLessThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeLessThanOrEqualTo must throw an exception when the specified value is below or equal to the given boundary.")]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(-80, -40)]
        public void ParameterAtOrBelowBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeLessThanOrEqualTo(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than or equal to {boundary}, but it actually is {value}.");
        }

        [Theory(DisplayName = "MustNotBeLessThanOrEqualTo must not throw an exception when the specified value is above the given boundary.")]
        [InlineData(5L, 4L)]
        [InlineData(100L, 1L)]
        [InlineData(-87L, -90L)]
        public void ParamterAboveBoundary(long value, long boundary)
        {
            var result = value.MustNotBeLessThanOrEqualTo(boundary, nameof(value));

            result.Should().Be(value);
        }

        [Fact(DisplayName = "MustNotBeLessThanOrEqualTo must throw the custom exception with single parameter when parameter is less than or equal to the boundary value.")]
        public static void ThrowCustomExceptionWithOneParameter()
        {
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => 3.MustNotBeLessThanOrEqualTo(3, v =>
            {
                recordedValue = v;
                return exception;
            });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(3);
        }

        [Fact(DisplayName = "MustNotBeLessThanOrEqualTo must not throw the custom exception with single parameter when parameter is greater than the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 19.MustNotBeLessThanOrEqualTo(15, v => null);

            result.Should().Be(19);
        }

        [Fact(DisplayName = "MustNotBeLessThanOrEqualTo must throw the custom exception with two parameters when parameter is less than or equal to the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 131.MustNotBeLessThanOrEqualTo(4200, (v, b) =>
            {
                recordedParameter = v;
                recordedBoundary = b;
                return exception;
            });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(131);
            recordedBoundary.Should().Be(4200);
        }

        [Fact(DisplayName = "MustNotBeLessThanOrEqualTo must not throw the custom exception with two parameters when parameter is greater than the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5.6m.MustNotBeLessThanOrEqualTo(5.1m, (v, b) => null);

            result.Should().Be(5.6m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeLessThanOrEqualTo(42, exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeLessThanOrEqualTo(42, message: message)));
        }
    }
}