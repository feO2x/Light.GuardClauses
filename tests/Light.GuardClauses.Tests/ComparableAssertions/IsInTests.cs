using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class IsInTests
{
    [Fact]
    public static void IsInRange() => 42.IsIn(Range<int>.FromInclusive(42).ToExclusive(50)).Should().BeTrue();

    [Fact]
    public static void NotInRange() => 50.775.IsIn(Range<double>.FromExclusive(25.0).ToExclusive(50.00)).Should().BeFalse();

    [Fact]
    public static void NonGenericIsInRange() =>
        187.5f.IsIn(Range.FromInclusive(20f).ToExclusive(250f)).Should().BeTrue();

    [Fact]
    public static void NonGenericNotInRange() => 
        75.IsIn(Range.FromInclusive(10).ToExclusive(30)).Should().BeFalse();
}