using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class IsLessThanOrApproximatelyTests
{
    [Theory]
    [InlineData(13.25, 13.5, true)]
    [InlineData(48.99, 48.98999, true)]
    [InlineData(19.775, 19.7, false)]
    [InlineData(-14.32, -14.33, false)]
    public static void DoubleWithDefaultTolerance(double first, double second, bool expected) =>
        first.IsLessThanOrApproximately(second).Should().Be(expected);

    [Theory]
    [InlineData(50.3, 50.5, 0.5, true)]
    [InlineData(2015.5, 2015.3, 0.3, true)]
    [InlineData(-519, -519.5, 0.25, false)]
    [InlineData(0, -1, 0.9, false)]
    public static void DoubleWithCustomTolerance(double first, double second, double tolerance, bool expected) =>
        first.IsLessThanOrApproximately(second, tolerance).Should().Be(expected);
    
    [Theory]
    [InlineData(13.25f, 13.5f, true)]
    [InlineData(48.99f, 48.98999f, true)]
    [InlineData(19.775f, 19.7f, false)]
    [InlineData(-14.32f, -14.33f, false)]
    public static void FloatWithDefaultTolerance(float first, float second, bool expected) =>
        first.IsLessThanOrApproximately(second).Should().Be(expected);

    [Theory]
    [InlineData(50.3f, 50.5f, 0.5f, true)]
    [InlineData(2015.5f, 2015.3f, 0.3f, true)]
    [InlineData(-519f, -519.5f, 0.25f, false)]
    [InlineData(0f, -1f, 0.9f, false)]
    public static void FloatWithCustomTolerance(float first, float second, float tolerance, bool expected) =>
        first.IsLessThanOrApproximately(second, tolerance).Should().Be(expected);
}