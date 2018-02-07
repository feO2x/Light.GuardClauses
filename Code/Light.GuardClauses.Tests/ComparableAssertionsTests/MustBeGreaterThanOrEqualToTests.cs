using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustBeGreaterThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeGreaterThanOrEqualTo must throw an ArgumentOutOfRangeException when the specified parameter is less than the boundary value.")]
        [InlineData(1, 2)]
        [InlineData(-1, 1)]
        public void ParameterLess(int first, int second)
        {
            Action act = () => first.MustBeGreaterThanOrEqualTo(second, nameof(first));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be greater than or equal to {second}, but it actually is {first}.");
        }

        [Theory(DisplayName = "MustBeGreaterThanOrEqualTo must not throw an exception when the specified parameter is greater than or equal to the boundary value.")]
        [InlineData(14L, 2L)]
        [InlineData(15L, 15L)]
        [InlineData(long.MaxValue, long.MaxValue - 2)]
        public void ParameterGreaterOrEqual(long first, long second)
        {
            var result = first.MustBeGreaterThanOrEqualTo(second);

            result.Should().Be(first);
        }

        [Fact(DisplayName = "MustBeGreaterThanOrEqualTo must throw the custom exception with single parameter when parameter is less than the boundary value.")]
        public static void ThrowCustomExceptionWithOneParameter()
        {
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => 2.MustBeGreaterThanOrEqualTo(3, v =>
            {
                recordedValue = v;
                return exception;
            });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(2);
        }

        [Fact(DisplayName = "MustBeGreaterThanOrEqualTo must not throw the custom exception with single parameter when parameter is not less than the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 15.MustBeGreaterThanOrEqualTo(15, v => null);

            result.Should().Be(15);
        }

        [Fact(DisplayName = "MustBeGreaterThanOrEqualTo must throw the custom exception with two parameters when parameter is less than the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 15152.MustBeGreaterThanOrEqualTo(42124, (v, b) =>
            {
                recordedParameter = v;
                recordedBoundary = b;
                return exception;
            });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(15152);
            recordedBoundary.Should().Be(42124);
        }

        [Fact(DisplayName = "MustBeGreaterThanOrEqualTo must not throw the custom exception with two parameters when parameter is not less than the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5.6m.MustBeGreaterThanOrEqualTo(5.1m, (v, b) => null);

            result.Should().Be(5.6m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 2.MustBeGreaterThanOrEqualTo(42, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 30.MustBeGreaterThanOrEqualTo(31, message: message)));
        }
    }
}