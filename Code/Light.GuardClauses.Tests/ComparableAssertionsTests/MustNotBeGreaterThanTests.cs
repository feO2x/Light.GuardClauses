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

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than {boundary}, but you specified {value}.");
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

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 12.MustNotBeGreaterThan(0, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeGreaterThan(41, message: message)));
        }
    }
}