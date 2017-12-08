using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class IsInTests
    {
        [Fact(DisplayName = "IsIn must return true if the specified value is within the specified range.")]
        public void IsInRange()
        {
            var result = 42.IsIn(Range<int>.FromInclusive(42).ToExclusive(50));

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsIn must return false if the specified value is not within the specified range.")]
        public void NotInRange()
        {
            var result = 50.775.IsIn(Range<double>.FromExclusive(25.0).ToExclusive(50.00));

            result.Should().BeFalse();
        }
    }
}