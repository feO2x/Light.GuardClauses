using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeInTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeIn must throw an exception when the specified value is outside of range (with inclusive lower boundary and exclusive upper boundary).")]
        [InlineData(-1, 1, 5)]
        [InlineData(5, 1, 5)]
        [InlineData(6, 1, 5)]
        public void ParameterOutOfInclusiveLowerAndExclusiveUpperBoundary(int value, int lowerBoundary, int upperBoundary)
        {
            Action act = () => value.MustBeIn(Range<int>.FromInclusive(lowerBoundary).ToExclusive(upperBoundary), nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be between {lowerBoundary} (inclusive) and {upperBoundary} (exclusive), but you specified {value}.");
        }

        [Theory(DisplayName = "MustBeIn must throw an exception when the specified value is outside of range (with exclusive lower boundary and inclusive upper boundary).")]
        [InlineData(0, 1, 5)]
        [InlineData(1, 1, 5)]
        [InlineData(6, 1, 5)]
        [InlineData('a', 'b', 'f')]
        [InlineData('b', 'b', 'f')]
        [InlineData('g', 'b', 'f')]
        public void ParameterOutOfExclusiveLowerAndInclusiveUpperBoundary(char value, char lowerBoundary, char upperBoundary)
        {
            Action act = () => value.MustBeIn(Range<char>.FromExclusive(lowerBoundary).ToInclusive(upperBoundary), nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must be between {lowerBoundary} (exclusive) and {upperBoundary} (inclusive), but you specified {value}.");
        }

        [Theory(DisplayName = "MustBeIn must not throw an exception when the specified value is inside the range.")]
        [InlineData(10, 10, 20, true, true)]
        [InlineData(20, 10, 20, true, true)]
        [InlineData(11, 10, 20, true, false)]
        [InlineData(11, 10, 20, false, false)]
        [InlineData(19, 10, 20, false, false)]
        public void ParameterWithinRange(int value, int lowerBoundary, int upperBoundary, bool isLowerBoundaryInclusive, bool isUpperBoundaryInclusive)
        {
            var range = new Range<int>(lowerBoundary, upperBoundary, isLowerBoundaryInclusive, isUpperBoundaryInclusive);
            var result = value.MustBeIn(range, nameof(value));

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 42.MustBeIn(Range<int>.FromInclusive(10).ToExclusive(20), exception: exception)))
                    .Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustBeIn(Range<int>.FromInclusive(0).ToInclusive(10), message: message)));
        }
    }
}