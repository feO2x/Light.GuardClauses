using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class IsApproximatelyTests
    {
        [Theory]
        [InlineData(3.5, 3.6, false)]
        [InlineData(5.1, 5.1, true)]
        [InlineData(-5.0, 5.0, false)]
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

        [Theory]
        [InlineData(17.5f, 17.4f, false)]
        [InlineData(-3.75f, -3.75f, true)]
        [InlineData(-10.0f, 10.0f, false)]
        [InlineData(1.1f, 1.099999f, true)]
        public static void FloatWithDefaultTolerance(float first, float second, bool expected) =>
            first.IsApproximately(second).Should().Be(expected);

        [Theory]
        [InlineData(1.1f, 1.3f, 0.5f, true)]
        [InlineData(100.55f, 100.555f, 0.00001f, false)]
        [InlineData(5.0f, 14.999999f, 10.0f, true)]
        [InlineData(5.0f, 15.0f, 10.0f, false)]
        [InlineData(5.0f, 15.0001f, 10.0f, false)]
        public static void FloatWithCustomTolerance(float first, float second, float tolerance, bool expected) =>
            first.IsApproximately(second, tolerance).Should().Be(expected);
    }
}