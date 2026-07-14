using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustNotBeZeroTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public static void NonZeroInt32sAreAccepted(int value) => value.MustNotBeZero().Should().Be(value);

    [Theory]
    [InlineData(1L)]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    public static void NonZeroInt64sAreAccepted(long value) => value.MustNotBeZero().Should().Be(value);

    [Fact]
    public static void NonZeroDecimalsAreAccepted()
    {
        0.5m.MustNotBeZero().Should().Be(0.5m);
        (-0.5m).MustNotBeZero().Should().Be(-0.5m);
        decimal.MinValue.MustNotBeZero().Should().Be(decimal.MinValue);
        decimal.MaxValue.MustNotBeZero().Should().Be(decimal.MaxValue);
    }

    [Theory]
    [InlineData(float.Epsilon)]
    [InlineData(-float.Epsilon)]
    [InlineData(1f)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public static void NonZeroFloatsAreAccepted(float value) => value.MustNotBeZero().Should().Be(value);

    [Theory]
    [InlineData(double.Epsilon)]
    [InlineData(-double.Epsilon)]
    [InlineData(-1d)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public static void NonZeroDoublesAreAccepted(double value) => value.MustNotBeZero().Should().Be(value);

    [Fact]
    public static void NaNIsAccepted()
    {
        float.NaN.MustNotBeZero().Should().Be(float.NaN);
        double.NaN.MustNotBeZero().Should().Be(double.NaN);
    }

    [Fact]
    public static void ZerosAreRejected()
    {
        CheckInt32IsRejected(0);
        CheckInt64IsRejected(0L);
        CheckDecimalIsRejected(0m);
        CheckFloatIsRejected(0f);
        CheckDoubleIsRejected(0d);
        CheckTimeSpanIsRejected(TimeSpan.Zero);
    }

    [Fact]
    public static void NegativeZerosAreRejectedLikeZero()
    {
        CheckFloatIsRejected(-0f);
        CheckDoubleIsRejected(-0d);
        CheckDecimalIsRejected(new decimal(0, 0, 0, true, 0));
        CheckDecimalIsRejected(0.000m);
    }

    [Fact]
    public static void NonZeroTimeSpansAreAccepted()
    {
        TimeSpan.FromTicks(1).MustNotBeZero().Should().Be(TimeSpan.FromTicks(1));
        TimeSpan.FromTicks(-1).MustNotBeZero().Should().Be(TimeSpan.FromTicks(-1));
    }

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        const int invalidValue = 0;

        var act = () => invalidValue.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage("*must not be zero, but it actually is 0*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 0.MustNotBeZero(message: message));

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException(0, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0L, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0m, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0f, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0d, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(TimeSpan.Zero, (value, factory) => value.MustNotBeZero(factory));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void GenericOverloadsCoverTypesWithoutConcreteOverloads()
    {
        ((short) 5).MustNotBeZero().Should().Be((short) 5);
        ((byte) 3).MustNotBeZero().Should().Be((byte) 3);
        Half.NaN.MustNotBeZero().Should().Be(Half.NaN);

        var zeroShort = () => ((short) 0).MustNotBeZero();
        var zeroHalf = () => Half.Zero.MustNotBeZero();
        var negativeZeroHalf = () => Half.NegativeZero.MustNotBeZero();
        zeroShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
        zeroHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
        negativeZeroHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");

        Test.CustomException((short) 0, (value, factory) => value.MustNotBeZero(factory));
    }
#endif

    private static void CheckInt32IsRejected(int value)
    {
        var act = () => value.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
    }

    private static void CheckInt64IsRejected(long value)
    {
        var act = () => value.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
    }

    private static void CheckDecimalIsRejected(decimal value)
    {
        var act = () => value.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
    }

    private static void CheckFloatIsRejected(float value)
    {
        var act = () => value.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
    }

    private static void CheckDoubleIsRejected(double value)
    {
        var act = () => value.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
    }

    private static void CheckTimeSpanIsRejected(TimeSpan value)
    {
        var act = () => value.MustNotBeZero();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
    }
}
