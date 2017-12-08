using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeGreaterThanOrEqualToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must throw an exception when the specified value is greater than or equal to the given boundary.")]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        [InlineData(-87, -88)]
        public void ParameterAtOrAboveBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than or equal to {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must not throw an exception when the specified value is less than the given boundary.")]
        [InlineData(0L, 1L)]
        [InlineData(-80L, -70L)]
        public void ParameterBelowBoundary(long value, long boundary)
        {
            var result = value.MustNotBeGreaterThanOrEqualTo(boundary);

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeGreaterThanOrEqualTo(41, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeGreaterThanOrEqualTo(42, message: message)));
        }
    }
}