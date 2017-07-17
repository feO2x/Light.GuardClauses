using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsNotInTests
    {
        [Theory(DisplayName = "IsNotIn must return true when the specified value is not within the specified range.")]
        [MemberData(nameof(NotInRangeData))]
        public void NotInRange<T>(T value, Range<T> range) where T : IComparable<T>
        {
            var result = value.IsNotIn(range);

            result.Should().BeTrue();
        }

        public static readonly TestData NotInRangeData =
            new[]
            {
                new object[] { 42, Range<int>.FromInclusive(0).ToInclusive(10) },
                new object[] { 'A', Range<char>.FromInclusive('a').ToInclusive('z') }
            };

        [Theory (DisplayName = "IsNotIn must return false when the specified value is within the specified range.")]
        [MemberData(nameof(InRangeData))]
        public void InRange<T>(T value, Range<T> range) where T : IComparable<T>
        {
            var result = value.IsNotIn(range);

            result.Should().BeFalse();
        }

        public static readonly TestData InRangeData =
            new[]
            {
                new object[] { 42, Range<int>.FromInclusive(30).ToExclusive(43) },
                new object[] { 65.0, Range<double>.FromInclusive(50.0).ToExclusive(100.0) }
            };
    }
}