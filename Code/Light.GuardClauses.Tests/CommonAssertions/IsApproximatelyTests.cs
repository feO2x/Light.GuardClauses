using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class IsApproximatelyTests
    {
        [Theory]
        [InlineData(3.5, 3.6, false)]
        [InlineData(5.1, 5.1, true)]
        [InlineData(-5, 5, false)]
        [InlineData(1.1, 1.099999, true)]
        public static void DoubleWithDefaultTolerance(double first, double second, bool expected) =>
            first.IsApproximately(second).Should().Be(expected);

        [Theory]
        [InlineData(1.1, 1.3, 0.5, true)]
        [InlineData(100.55, 100.555, 0.00001, false)]
        [InlineData(5.0, 14.999999, 10.0, true)]
        [InlineData(5.0, 15.0, 10.0, false)]
        [InlineData(5.0, 15.0001, 10.0, false)]
        public static void DoubleWithCustomTolerance(double first, double second, double tolerance, bool expected) =>
            first.IsApproximately(second, tolerance).Should().Be(expected);
    }
}