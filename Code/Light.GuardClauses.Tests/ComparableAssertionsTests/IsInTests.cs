using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsInTests
    {
        [Theory(DisplayName = "IsIn must return true if the specified value is within the specified range.")]
        [MemberData(nameof(IsInRangeData))]
        public void IsInRange<T>(T value, Range<T> range) where T : IComparable<T>
        {
            var result = value.IsIn(range);

            result.Should().BeTrue();
        }

        public static readonly TestData IsInRangeData =
            new[]
            {
                new object[] { 42, Range<int>.FromInclusive(42).ToExclusive(50) },
                new object[] { new DateTime(2007, 12, 24), Range<DateTime>.FromInclusive(new DateTime(2006, 12, 24)).ToExclusive(new DateTime(2008, 12, 24)) }
            };

        [Theory(DisplayName = "IsIn must return false if the specified value is not within the specified range.")]
        [MemberData(nameof(NotInRangeData))]
        public void NotInRange<T>(T value, Range<T> range) where T : IComparable<T>
        {
            var result = value.IsIn(range);

            result.Should().BeFalse();
        }

        public static readonly TestData NotInRangeData =
            new[]
            {
                new object[] { 100, Range<int>.FromInclusive(0).ToExclusive(100) },
                new object[] { 50.775, Range<double>.FromExclusive(25.0).ToExclusive(50.00) }
            };
    }
}