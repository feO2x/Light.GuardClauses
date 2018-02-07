using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustBeLessThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeLessThanOrEqualTo must throw an ArgumentOutOfRangeException when the specified paramter is greater than the boundary value.")]
        [InlineData(10, 9)]
        [InlineData(-42, -1888)]
        public void ParameterGreater(int first, int second)
        {
            Action act = () => first.MustBeLessThanOrEqualTo(second, nameof(first));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be less than or equal to {second}, but it actually is {first}.");
        }

        [Theory(DisplayName = "MustBeLessThanOrEqualTo must not throw an exception when the specified parameter is less than or equal to the boundary value.")]
        [InlineData(13, 13)]
        [InlineData(12, 13)]
        public void ParameterLessOrEqual(int first, int second)
        {
            var result = first.MustBeLessThanOrEqualTo(second);

            result.Should().Be(first);
        }

        [Fact(DisplayName = "MustBeLessThanOrEqualTo must throw the custom exception with single parameter when parameter is greater than the boundary value.")]
        public void ThrowCustomExceptionWithOneParameter()
        {
            var value = 42;
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => value.MustBeLessThanOrEqualTo(3, v =>
                                                                {
                                                                    recordedValue = v;
                                                                    return exception;
                                                                });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(value);
        }

        [Fact(DisplayName = "MustBeLessThanOrEqualTo must not throw the custom exception with single parameter when parameter is less than or equal to the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 8.MustBeLessThanOrEqualTo(8, v => null);

            result.Should().Be(8);
        }

        [Fact(DisplayName = "MustBeLessThanOrEqualTo must throw the custom exception with two parameters when parameter is greater than the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 1656.MustBeLessThanOrEqualTo(421, (v, b) =>
                                                                 {
                                                                     recordedParameter = v;
                                                                     recordedBoundary = b;
                                                                     return exception;
                                                                 });

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(1656);
            recordedBoundary.Should().Be(421);
        }

        [Fact(DisplayName = "MustBeLessThanOrEqualTo must not throw the custom exception with two parameters when parameter is less than or equal to the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5m.MustBeLessThan(5.1m, (v, b) => null);

            result.Should().Be(5m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 3.MustBeLessThanOrEqualTo(2, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 14.MustBeLessThanOrEqualTo(1, message: message)));
        }
    }
}