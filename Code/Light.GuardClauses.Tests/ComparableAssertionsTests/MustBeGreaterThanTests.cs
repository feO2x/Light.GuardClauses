using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustBeGreaterThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeGreaterThan must throw an ArgumentOutOfRangeException when the boundary value is greater or equal to the parameter value to be checked.")]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        public void ParameterEqualOrLess(int first, int second)
        {
            Action act = () => first.MustBeGreaterThan(second, nameof(first));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be greater than {second}, but it actually is {first}.");
        }

        [Theory(DisplayName = "MustBeGreaterThan must not throw an exception when the specified parameter is greater than the boundary value.")]
        [InlineData(133, 99)]
        [InlineData(int.MaxValue, int.MaxValue - 1)]
        [InlineData(byte.MaxValue, byte.MinValue)]
        public void ParameterGreater(int first, int second)
        {
            var result = first.MustBeGreaterThan(second);

            result.Should().Be(first);
        }

        [Fact(DisplayName = "MustBeGreaterThan must throw the custom exception with single parameter when parameter is less than or equal to the boundary value.")]
        public static void ThrowCustomExceptionWithOneParameter()
        {
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => 3.MustBeGreaterThan(3, v =>
            {
                recordedValue = v;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(3);
        }

        [Fact(DisplayName = "MustBeGreaterThan must not throw the custom exception with single parameter when parameter is greater than the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 19.MustBeGreaterThan(15, v => null);

            result.Should().Be(19);
        }

        [Fact(DisplayName = "MustBeGreaterThan must throw the custom exception with two parameters when parameter is less than or equal to the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 131.MustBeGreaterThan(4200, (v, b) =>
            {
                recordedParameter = v;
                recordedBoundary = b;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(131);
            recordedBoundary.Should().Be(4200);
        }

        [Fact(DisplayName = "MustBeGreaterThan must not throw the custom exception with two parameters when parameter is greater than the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5.6m.MustBeGreaterThan(5.1m, (v, b) => null);

            result.Should().Be(5.6m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 1.MustBeGreaterThan(2, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustBeGreaterThan(87, message: message)));
        }
    }
}