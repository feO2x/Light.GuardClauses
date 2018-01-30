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

            act.ShouldThrow<ArgumentOutOfRangeException>()
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

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustNotBeLessThan(43, exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeLessThan(43, message: message)));
        }
    }
}