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
               .And.Message.Should().Contain($"{nameof(value)} must not be less than or equal to {boundary}, but you specified {value}.");
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

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeLessThanOrEqualTo(42, exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeLessThanOrEqualTo(42, message: message)));
        }
    }
}