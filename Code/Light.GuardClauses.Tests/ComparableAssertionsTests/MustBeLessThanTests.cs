using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class MustBeLessThanTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeLessThan must throw an ArgumentOutOfRangeException when the specified parameter is not less than the boundary value.")]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(short.MaxValue, short.MinValue)]
        [InlineData('Z', 'Z')]
        public static void ParameterNotLess(int first, int second)
        {
            Action act = () => first.MustBeLessThan(second, nameof(first));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(first)} must be less than {second}, but it actually is {first}.");
        }

        [Theory(DisplayName = "MustBeLessThan must not throw any exception when the specified parameter is less than the boundary value.")]
        [InlineData(1L, 2L)]
        [InlineData(-444L, -410L)]
        [InlineData(long.MinValue, long.MinValue + 14)]
        [InlineData('A', 'a')]
        public static void ParameterLess(long first, long second)
        {
            var result = first.MustBeLessThan(second);

            result.Should().Be(first);
        }

        [Fact(DisplayName = "MustBeLessThan must throw the custom exception with single parameter when parameter is not less than the boundary value.")]
        public static void ThrowCustomExceptionWithOneParameter()
        {
            var recordedValue = default(int);
            var exception = new Exception();

            Action act = () => 4.MustBeLessThan(2, v =>
                                                   {
                                                       recordedValue = v;
                                                       return exception;
                                                   });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(4);
        }

        [Fact(DisplayName = "MustBeLessThan must not throw the custom exception with single parameter when parameter is less than the boundary value.")]
        public static void NoCustomExceptionWithOneParameter()
        {
            var result = 8.MustBeLessThan(15, v => null);

            result.Should().Be(8);
        }

        [Fact(DisplayName = "MustBeLessThan must throw the custom exception with two parameters when parameter is not less than the boundary value.")]
        public static void ThrowCustomExceptionWithTwoParameters()
        {
            var recordedParameter = default(int);
            var recordedBoundary = default(int);
            var exception = new Exception();

            Action act = () => 1656.MustBeLessThan(421, (v, b) =>
                                                        {
                                                            recordedParameter = v;
                                                            recordedBoundary = b;
                                                            return exception;
                                                        });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedParameter.Should().Be(1656);
            recordedBoundary.Should().Be(421);
        }

        [Fact(DisplayName = "MustBeLessThan must not throw the custom exception with two parameters when parameter is less than the boundary value.")]
        public static void NoCustomExceptionWithTwoParameters()
        {
            var result = 5m.MustBeLessThan(5.1m, (v, b) => null);

            result.Should().Be(5m);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 14.MustBeLessThan(2, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 2.MustBeLessThan(2, message: message)));
        }
    }
}