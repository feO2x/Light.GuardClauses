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
               .And.Message.Should().Contain($"{nameof(first)} must be greater than or equal to {second}, but you specified {first}.");
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

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 2.MustBeGreaterThanOrEqualTo(42, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 30.MustBeGreaterThanOrEqualTo(31, message: message)));
        }
    }
}