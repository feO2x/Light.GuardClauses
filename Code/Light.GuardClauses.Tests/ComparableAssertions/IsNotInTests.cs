using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class IsNotInTests
    {
        [Fact]
        public static void NotInRange() => 'A'.IsNotIn(Range<char>.FromInclusive('a').ToInclusive('z')).Should().BeTrue();

        [Fact]
        public static void InRange() => 42.IsNotIn(Range<int>.FromInclusive(30).ToExclusive(43)).Should().BeFalse();
    }
}