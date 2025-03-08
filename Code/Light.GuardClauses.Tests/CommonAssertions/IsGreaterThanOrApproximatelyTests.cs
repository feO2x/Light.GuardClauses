using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class IsGreaterThanOrApproximatelyTests
{
    [Theory]
    [InlineData(17.4, 17.3, true)]
    [InlineData(19.9999999, 20.0, true)]
    [InlineData(-5.49998, -5.5, true)]
    [InlineData(-5.4998, -5.4996, false)]
    public static void DoubleWithDefaultTolerance(double first, double second, bool expected) =>
        first.IsGreaterThanOrApproximately(second).Should().Be(expected);

    [Theory]
    [InlineData(15.91, 15.9, 0.1, true)]
    [InlineData(24.449, 24.45, 0.0001, false)]
    [InlineData(-3.12, -3.2, 0.001, true)]
    [InlineData(2.369, 2.37, 0.0005, false)]
    public static void DoubleWithCustomTolerance(double first, double second, double tolerance, bool expected) =>
        first.IsGreaterThanOrApproximately(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(100.225f, 100.2f, true)]
    [InlineData(-5.9f, -5.5f, false)]
    [InlineData(0f, -0.02f, true)]
    [InlineData(-0.001f, 0f, false)]
    public static void FloatWithDefaultTolerance(float first, float second, bool expected) =>
        first.IsGreaterThanOrApproximately(second).Should().Be(expected);

    [Theory]
    [InlineData(2.0f, 1.0f, 0.1f, true)]
    [InlineData(1.0f, 1.0f, 0.1f, true)]
    [InlineData(1.0f, 1.1f, 0.01f, false)]
    [InlineData(1.0f, 2.0f, 0.1f, false)]
    public static void FloatWIthCustomTolerance(float first, float second, float tolerance, bool expected) =>
        first.IsGreaterThanOrApproximately(second, tolerance).Should().Be(expected);

#if NET8_0
    [Theory]
    [InlineData(15.91, 15.9, 0.1, true)]
    [InlineData(24.449, 24.45, 0.0001, false)]
    [InlineData(-3.12, -3.2, 0.001, true)]
    [InlineData(2.369, 2.37, 0.0005, false)]
    [InlineData(15.0, 14.0, 0.1, true)] // Greater than case
    [InlineData(14.95, 15.0, 0.1, true)] // Approximately equal case
    public static void GenericDoubleWithCustomTolerance(double first, double second, double tolerance, bool expected) =>
        first.IsGreaterThanOrApproximately<double>(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(2.0f, 1.0f, 0.1f, true)]
    [InlineData(1.0f, 1.0f, 0.1f, true)]
    [InlineData(1.0f, 1.1f, 0.01f, false)]
    [InlineData(1.0f, 2.0f, 0.1f, false)]
    [InlineData(2.1f, 2.0f, 0.01f, true)] // Greater than case
    public static void GenericFloatWithCustomTolerance(float first, float second, float tolerance, bool expected) =>
        first.IsGreaterThanOrApproximately<float>(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(10, 5, 1, true)] // Greater than case
    [InlineData(5, 5, 1, true)] // Equal case
    [InlineData(5, 6, 1, true)] // Approximately equal case
    [InlineData(5, 7, 1, false)] // Not greater than or approximately equal case
    public static void GenericIntWithCustomTolerance(int first, int second, int tolerance, bool expected) =>
        first.IsGreaterThanOrApproximately(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(10L, 5L, 1L, true)] // Greater than case
    [InlineData(5L, 5L, 1L, true)] // Equal case
    [InlineData(5L, 6L, 1L, true)] // Approximately equal case
    [InlineData(5L, 7L, 1L, false)] // Not greater than or approximately equal case
    public static void GenericLongWithCustomTolerance(long first, long second, long tolerance, bool expected) =>
        first.IsGreaterThanOrApproximately(second, tolerance).Should().Be(expected);

    [Theory]
    [MemberData(nameof(DecimalTestData))]
    public static void GenericDecimalWithCustomTolerance(
        decimal first,
        decimal second,
        decimal tolerance,
        bool expected
    ) =>
        first.IsGreaterThanOrApproximately(second, tolerance).Should().Be(expected);

    public static TheoryData<decimal, decimal, decimal, bool> DecimalTestData() => new ()
    {
        { 1.3m, 1.1m, 0.1m, true }, // Greater than case
        { 1.1m, 1.1m, 0.1m, true }, // Equal case
        { 1.0m, 1.1m, 0.2m, true }, // Approximately equal case
        { 1.0m, 1.3m, 0.1m, false }, // Not greater than or approximately equal case
    };
#endif
}
