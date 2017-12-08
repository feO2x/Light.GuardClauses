using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsNotInTests
    {
        [Fact(DisplayName = "IsNotIn must return true when the specified value is not within the specified range.")]
        public void NotInRange()
        {
            var result = 'A'.IsNotIn(Range<char>.FromInclusive('a').ToInclusive('z'));

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsNotIn must return false when the specified value is within the specified range.")]
        public void InRange()
        {
            var result = 42.IsNotIn(Range<int>.FromInclusive(30).ToExclusive(43));

            result.Should().BeFalse();
        }
    }
}