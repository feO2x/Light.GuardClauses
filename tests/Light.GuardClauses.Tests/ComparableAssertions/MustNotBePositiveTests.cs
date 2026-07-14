using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustNotBePositiveTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public static void NonPositiveInt32sAreAccepted(int value) => value.MustNotBePositive().Should().Be(value);

    [Theory]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public static void PositiveInt32sAreRejected(int value)
    {
        var act = () => value.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    public static void NonPositiveInt64sAreAccepted(long value) => value.MustNotBePositive().Should().Be(value);

    [Theory]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    public static void PositiveInt64sAreRejected(long value)
    {
        var act = () => value.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
    }

    [Fact]
    public static void NonPositiveDecimalsAreAccepted()
    {
        0m.MustNotBePositive().Should().Be(0m);
        (-0.5m).MustNotBePositive().Should().Be(-0.5m);
        decimal.MinValue.MustNotBePositive().Should().Be(decimal.MinValue);
    }

    [Fact]
    public static void PositiveDecimalsAreRejected()
    {
        CheckDecimalIsRejected(0.00001m);
        CheckDecimalIsRejected(decimal.MaxValue);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-float.Epsilon)]
    [InlineData(float.MinValue)]
    [InlineData(float.NegativeInfinity)]
    public static void NonPositiveFloatsAreAccepted(float value) => value.MustNotBePositive().Should().Be(value);

    [Theory]
    [InlineData(float.Epsilon)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NaN)]
    public static void PositiveFloatsAreRejected(float value)
    {
        var act = () => value.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(-double.Epsilon)]
    [InlineData(double.MinValue)]
    [InlineData(double.NegativeInfinity)]
    public static void NonPositiveDoublesAreAccepted(double value) => value.MustNotBePositive().Should().Be(value);

    [Theory]
    [InlineData(double.Epsilon)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NaN)]
    public static void PositiveDoublesAreRejected(double value)
    {
        var act = () => value.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
    }

    [Fact]
    public static void NegativeZerosAreAcceptedLikeZero()
    {
        (-0f).MustNotBePositive().Should().Be(-0f);
        (-0d).MustNotBePositive().Should().Be(-0d);
        new decimal(0, 0, 0, true, 0).MustNotBePositive().Should().Be(0m);
        0.000m.MustNotBePositive().Should().Be(0m);
    }

    [Fact]
    public static void NonPositiveTimeSpansAreAccepted()
    {
        TimeSpan.Zero.MustNotBePositive().Should().Be(TimeSpan.Zero);
        TimeSpan.FromTicks(-1).MustNotBePositive().Should().Be(TimeSpan.FromTicks(-1));
        TimeSpan.MinValue.MustNotBePositive().Should().Be(TimeSpan.MinValue);
    }

    [Fact]
    public static void PositiveTimeSpansAreRejected()
    {
        CheckTimeSpanIsRejected(TimeSpan.FromTicks(1));
        CheckTimeSpanIsRejected(TimeSpan.MaxValue);
    }

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        const int invalidValue = 5;

        var act = () => invalidValue.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage("*must not be positive, but it actually is 5*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 1.MustNotBePositive(message: message));

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException(1, (value, factory) => value.MustNotBePositive(factory));
        Test.CustomException(1L, (value, factory) => value.MustNotBePositive(factory));
        Test.CustomException(0.5m, (value, factory) => value.MustNotBePositive(factory));
        Test.CustomException(float.NaN, (value, factory) => value.MustNotBePositive(factory));
        Test.CustomException(double.PositiveInfinity, (value, factory) => value.MustNotBePositive(factory));
        Test.CustomException(TimeSpan.FromTicks(1), (value, factory) => value.MustNotBePositive(factory));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void GenericOverloadsCoverTypesWithoutConcreteOverloads()
    {
        ((short) 0).MustNotBePositive().Should().Be((short) 0);
        ((short) -5).MustNotBePositive().Should().Be((short) -5);
        ((byte) 0).MustNotBePositive().Should().Be((byte) 0);
        Half.Zero.MustNotBePositive().Should().Be(Half.Zero);

        var positiveShort = () => ((short) 3).MustNotBePositive();
        var nanHalf = () => Half.NaN.MustNotBePositive();
        positiveShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
        nanHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");

        Test.CustomException((short) 3, (value, factory) => value.MustNotBePositive(factory));
    }
#endif

    private static void CheckDecimalIsRejected(decimal value)
    {
        var act = () => value.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
    }

    private static void CheckTimeSpanIsRejected(TimeSpan value)
    {
        var act = () => value.MustNotBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be positive*");
    }
}
