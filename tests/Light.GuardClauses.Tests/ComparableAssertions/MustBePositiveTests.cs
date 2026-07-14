using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBePositiveTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    [InlineData(int.MaxValue)]
    public static void PositiveInt32sAreAccepted(int value) => value.MustBePositive().Should().Be(value);

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public static void NonPositiveInt32sAreRejected(int value)
    {
        var act = () => value.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    public static void PositiveInt64sAreAccepted(long value) => value.MustBePositive().Should().Be(value);

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    public static void NonPositiveInt64sAreRejected(long value)
    {
        var act = () => value.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
    }

    [Fact]
    public static void PositiveDecimalsAreAccepted()
    {
        0.00001m.MustBePositive().Should().Be(0.00001m);
        decimal.MaxValue.MustBePositive().Should().Be(decimal.MaxValue);
    }

    [Fact]
    public static void NonPositiveDecimalsAreRejected()
    {
        CheckDecimalIsRejected(0m);
        CheckDecimalIsRejected(-0.5m);
        CheckDecimalIsRejected(decimal.MinValue);
    }

    [Theory]
    [InlineData(float.Epsilon)]
    [InlineData(1f)]
    [InlineData(float.MaxValue)]
    [InlineData(float.PositiveInfinity)]
    public static void PositiveFloatsAreAccepted(float value) => value.MustBePositive().Should().Be(value);

    [Theory]
    [InlineData(0f)]
    [InlineData(-float.Epsilon)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public static void NonPositiveFloatsAreRejected(float value)
    {
        var act = () => value.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
    }

    [Theory]
    [InlineData(double.Epsilon)]
    [InlineData(1d)]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public static void PositiveDoublesAreAccepted(double value) => value.MustBePositive().Should().Be(value);

    [Theory]
    [InlineData(0d)]
    [InlineData(-double.Epsilon)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.NaN)]
    public static void NonPositiveDoublesAreRejected(double value)
    {
        var act = () => value.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
    }

    [Fact]
    public static void NegativeZerosAreRejectedLikeZero()
    {
        var negativeZeroFloat = () => (-0f).MustBePositive();
        var negativeZeroDouble = () => (-0d).MustBePositive();
        negativeZeroFloat.Should().Throw<ArgumentOutOfRangeException>();
        negativeZeroDouble.Should().Throw<ArgumentOutOfRangeException>();

        CheckDecimalIsRejected(new decimal(0, 0, 0, true, 0));
        CheckDecimalIsRejected(0.000m);
    }

    [Fact]
    public static void PositiveTimeSpansAreAccepted()
    {
        TimeSpan.FromTicks(1).MustBePositive().Should().Be(TimeSpan.FromTicks(1));
        TimeSpan.MaxValue.MustBePositive().Should().Be(TimeSpan.MaxValue);
    }

    [Fact]
    public static void NonPositiveTimeSpansAreRejected()
    {
        CheckTimeSpanIsRejected(TimeSpan.Zero);
        CheckTimeSpanIsRejected(TimeSpan.FromTicks(-1));
        CheckTimeSpanIsRejected(TimeSpan.MinValue);
    }

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        const int invalidValue = -5;

        var act = () => invalidValue.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage("*must be positive, but it actually is -5*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => (-1).MustBePositive(message: message));

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException(0, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(-1L, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(0m, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(float.NaN, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(double.NegativeInfinity, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(TimeSpan.Zero, (value, factory) => value.MustBePositive(factory));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void GenericOverloadsCoverTypesWithoutConcreteOverloads()
    {
        ((short) 5).MustBePositive().Should().Be((short) 5);
        ((byte) 3).MustBePositive().Should().Be((byte) 3);
        ((Half) 1.5f).MustBePositive().Should().Be((Half) 1.5f);

        var zeroShort = () => ((short) 0).MustBePositive();
        var nanHalf = () => Half.NaN.MustBePositive();
        zeroShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
        nanHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");

        Test.CustomException((short) -3, (value, factory) => value.MustBePositive(factory));
    }
#endif

    private static void CheckDecimalIsRejected(decimal value)
    {
        var act = () => value.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
    }

    private static void CheckTimeSpanIsRejected(TimeSpan value)
    {
        var act = () => value.MustBePositive();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
    }
}
