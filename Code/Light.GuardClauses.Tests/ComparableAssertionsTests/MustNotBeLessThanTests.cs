using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustNotBeLessThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeLessThan must throw an exception when the specified value is below the given boundary.")]
        [InlineData(1, 2)]
        [InlineData(-87, 2)]
        [InlineData(15U, 16U)]
        public void ParameterBelowBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeLessThan(boundary, nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but it actually is {value}.");
        }

        [Theory(DisplayName = "MustNotBeLessThan must not throw an exception when the specified value is equal to or greater than the given boundary.")]
        [InlineData(1, 0)]
        [InlineData(42, 42)]
        [InlineData(-87, -88)]
        public void ParameterAtOrAboveBoundary(short value, short boundary)
        {
            var result = value.MustNotBeLessThan(boundary, nameof(value));

            result.Should().Be(value);
        }

        [Fact(DisplayName = "MustNotBeLessThan must throw the custom exception with single parameter when parameter is less than the boundary value.")]
        public static void ThrowCustomExceptionWithOneParameter()
        {
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => 2.MustNotBeLessThan(3, v =>
                                                      {
                                                          recordedValue = v;
                                                          return exception;
                                                      });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(2);
        }

        [Fact(DisplayName = "MustNotBeLessThan must not throw the custom exception with single parameter when parameter is not less than the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 15.MustNotBeLessThan(15, v => null);

            result.Should().Be(15);
        }

        [Fact(DisplayName = "MustNotBeLessThan must throw the custom exception with two parameters when parameter is less than the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 15152.MustNotBeLessThan(42124, (v, b) =>
                                                              {
                                                                  recordedParameter = v;
                                                                  recordedBoundary = b;
                                                                  return exception;
                                                              });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(15152);
            recordedBoundary.Should().Be(42124);
        }

        [Fact(DisplayName = "MustNotBeLessThan must not throw the custom exception with two parameters when parameter is not less than the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5.6m.MustNotBeLessThan(5.1m, (v, b) => null);

            result.Should().Be(5.6m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeLessThan(43, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeLessThan(43, message: message)));
        }
    }
}