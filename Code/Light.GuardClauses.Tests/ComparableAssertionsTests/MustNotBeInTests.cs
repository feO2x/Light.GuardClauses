using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeInTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeIn must throw an exception when the specified value is inside of range (with inclusive lower boundary and exclusive upper boundary).")]
        [InlineData(1, 1, 5)]
        [InlineData(2, 1, 5)]
        [InlineData(4, 1, 5)]
        [InlineData('b', 'b', 'f')]
        [InlineData('c', 'b', 'f')]
        [InlineData('e', 'b', 'f')]
        public void ParameterWithinInclusiveLowerAndExclusiveUpperBoundary<T>(T value, T lowerBoundary, T upperBoundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeIn(Range<T>.FromInclusive(lowerBoundary).ToExclusive(upperBoundary), nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be between {lowerBoundary} (inclusive) and {upperBoundary} (exclusive), but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeIn must throw an exception when the specified value is inside of range (with exclusive lower boundary and inclusive upper boundary).")]
        [InlineData(2, 1, 5)]
        [InlineData(4, 1, 5)]
        [InlineData(5, 1, 5)]
        [InlineData('c', 'b', 'f')]
        [InlineData('d', 'b', 'f')]
        [InlineData('f', 'b', 'f')]
        public void ParameterWithinExclusiveLowerAndInclusiveUpperBoundary<T>(T value, T lowerBoundary, T upperBoundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeIn(Range<T>.FromExclusive(lowerBoundary).ToInclusive(upperBoundary), nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be between {lowerBoundary} (exclusive) and {upperBoundary} (inclusive), but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeIn must not throw an exception when the specified value is outside of the range.")]
        [InlineData(9, 10, 20, true, true)]
        [InlineData(21, 10, 20, true, true)]
        [InlineData(20, 10, 20, true, false)]
        [InlineData(10, 10, 20, false, false)]
        [InlineData(181, 10, 20, false, false)]
        public void ParameterOutOfRange<T>(T value, T lowerBoundary, T upperBoundary, bool isLowerBoundaryInclusive, bool isUpperBoundaryInclusive) where T : IComparable<T>
        {
            var range = new Range<T>(lowerBoundary, upperBoundary, isLowerBoundaryInclusive, isUpperBoundaryInclusive);
            Action act = () => value.MustNotBeIn(range, nameof(value));

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => 30.MustNotBeIn(Range<int>.FromInclusive(30).ToExclusive(35), exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentOutOfRangeException>(message => 42.MustNotBeIn(Range<int>.FromInclusive(40).ToExclusive(50), message: message)));
        }
    }
}
