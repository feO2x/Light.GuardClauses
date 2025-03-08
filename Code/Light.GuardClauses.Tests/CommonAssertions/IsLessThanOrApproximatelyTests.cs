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

#if NET8_0
    [Theory]
    [InlineData(13.25, 13.5, 0.1, true)]  // Less than case
    [InlineData(13.5, 13.5, 0.1, true)]   // Equal case
    [InlineData(13.55, 13.5, 0.1, true)]  // Approximately equal case
    [InlineData(13.7, 13.5, 0.1, false)]  // Not less than or approximately equal case
    public static void GenericDoubleWithCustomTolerance(double first, double second, double tolerance, bool expected) =>
        first.IsLessThanOrApproximately<double>(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(13.25f, 13.5f, 0.1f, true)]  // Less than case
    [InlineData(13.5f, 13.5f, 0.1f, true)]   // Equal case
    [InlineData(13.55f, 13.5f, 0.1f, true)]  // Approximately equal case
    [InlineData(13.7f, 13.5f, 0.1f, false)]  // Not less than or approximately equal case
    public static void GenericFloatWithCustomTolerance(float first, float second, float tolerance, bool expected) =>
        first.IsLessThanOrApproximately<float>(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(5, 10, 1, true)]  // Less than case
    [InlineData(5, 5, 1, true)]   // Equal case
    [InlineData(6, 5, 1, true)]   // Approximately equal case
    [InlineData(7, 5, 1, false)]  // Not less than or approximately equal case
    public static void GenericIntWithCustomTolerance(int first, int second, int tolerance, bool expected) =>
        first.IsLessThanOrApproximately(second, tolerance).Should().Be(expected);

    [Theory]
    [InlineData(5L, 10L, 1L, true)]  // Less than case
    [InlineData(5L, 5L, 1L, true)]   // Equal case 
    [InlineData(6L, 5L, 1L, true)]   // Approximately equal case
    [InlineData(7L, 5L, 1L, false)]  // Not less than or approximately equal case
    public static void GenericLongWithCustomTolerance(long first, long second, long tolerance, bool expected) =>
        first.IsLessThanOrApproximately(second, tolerance).Should().Be(expected);

    [Theory]
    [MemberData(nameof(DecimalTestData))]
    public static void GenericDecimalWithCustomTolerance(
        decimal first,
        decimal second,
        decimal tolerance,
        bool expected
    ) =>
        first.IsLessThanOrApproximately(second, tolerance).Should().Be(expected);

    public static TheoryData<decimal, decimal, decimal, bool> DecimalTestData() => new ()
    {
        { 1.0m, 1.2m, 0.1m, true },  // Less than case
        { 1.1m, 1.1m, 0.1m, true },  // Equal case
        { 1.2m, 1.1m, 0.1m, true },  // Approximately equal case
        { 1.3m, 1.1m, 0.1m, false }, // Not less than or approximately equal case
    };
#endif
}