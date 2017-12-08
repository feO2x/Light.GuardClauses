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
               .And.Message.Should().Contain($"{nameof(first)} must be less than or equal to {second}, but you specified {first}.");
        }

        [Theory(DisplayName = "MustBeLessThanOrEqualTo must not throw an exception when the specified parameter is less than or equal to the boundary value.")]
        [InlineData(13, 13)]
        [InlineData(12, 13)]
        public void ParameterLessOrEqual(int first, int second)
        {
            var result = first.MustBeLessThanOrEqualTo(second);

            result.Should().Be(first);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 3.MustBeLessThanOrEqualTo(2, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 14.MustBeLessThanOrEqualTo(1, message: message)));
        }
    }
}