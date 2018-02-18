using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustNotBeGreaterThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeGreaterThan must throw an exception when the specified value is above the given boundary.")]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        [InlineData(-88, -100)]
        public void ParamterAboveBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeGreaterThan(boundary, nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than {boundary}, but it actually is {value}.");
        }

        [Theory(DisplayName = "MustNotBeGreaterThan must not throw an exception when the specified value is equal or lower than the given boundary.")]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(-88, 0)]
        public void ParameterAtOrBelowBoundary(short value, short boundary)
        {
            var result = value.MustNotBeGreaterThan(boundary, nameof(value));

            result.Should().Be(value);
        }

        [Fact(DisplayName = "MustNotBeGreaterThan must throw the custom exception with single parameter when parameter is greater than the boundary value.")]
        public void ThrowCustomExceptionWithOneParameter()
        {
            var value = 42;
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => value.MustNotBeGreaterThan(3, v =>
            {
                recordedValue = v;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(value);
        }

        [Fact(DisplayName = "MustNotBeGreaterThan must not throw the custom exception with single parameter when parameter is less than or equal to the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 8.MustNotBeGreaterThan(8, v => null);

            result.Should().Be(8);
        }

        [Fact(DisplayName = "MustNotBeGreaterThan must throw the custom exception with two parameters when parameter is greater than the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 1656.MustNotBeGreaterThan(421, (v, b) =>
            {
                recordedParameter = v;
                recordedBoundary = b;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(1656);
            recordedBoundary.Should().Be(421);
        }

        [Fact(DisplayName = "MustNotBeGreaterThan must not throw the custom exception with two parameters when parameter is less than or equal to the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5m.MustNotBeGreaterThan(5.1m, (v, b) => null);

            result.Should().Be(5m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 12.MustNotBeGreaterThan(0, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeGreaterThan(41, message: message)));
        }
    }
}