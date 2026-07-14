using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustNotBeNegativeTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public static void NonNegativeInt32sAreAccepted(int value) => value.MustNotBeNegative().Should().Be(value);

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public static void NegativeInt32sAreRejected(int value)
    {
        var act = () => value.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    public static void NonNegativeInt64sAreAccepted(long value) => value.MustNotBeNegative().Should().Be(value);

    [Theory]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    public static void NegativeInt64sAreRejected(long value)
    {
        var act = () => value.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
    }

    [Fact]
    public static void NonNegativeDecimalsAreAccepted()
    {
        0m.MustNotBeNegative().Should().Be(0m);
        0.5m.MustNotBeNegative().Should().Be(0.5m);
        decimal.MaxValue.MustNotBeNegative().Should().Be(decimal.MaxValue);
    }

    [Fact]
    public static void NegativeDecimalsAreRejected()
    {
        CheckDecimalIsRejected(-0.00001m);
        CheckDecimalIsRejected(decimal.MinValue);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(float.Epsilon)]
    [InlineData(float.MaxValue)]
    [InlineData(float.PositiveInfinity)]
    public static void NonNegativeFloatsAreAccepted(float value) => value.MustNotBeNegative().Should().Be(value);

    [Theory]
    [InlineData(-float.Epsilon)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(float.NaN)]
    public static void NegativeFloatsAreRejected(float value)
    {
        var act = () => value.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(double.Epsilon)]
    [InlineData(double.MaxValue)]
    [InlineData(double.PositiveInfinity)]
    public static void NonNegativeDoublesAreAccepted(double value) => value.MustNotBeNegative().Should().Be(value);

    [Theory]
    [InlineData(-double.Epsilon)]
    [InlineData(double.NegativeInfinity)]
    [InlineData(double.NaN)]
    public static void NegativeDoublesAreRejected(double value)
    {
        var act = () => value.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
    }

    [Fact]
    public static void NegativeZerosAreAcceptedLikeZero()
    {
        (-0f).MustNotBeNegative().Should().Be(-0f);
        (-0d).MustNotBeNegative().Should().Be(-0d);
        new decimal(0, 0, 0, true, 0).MustNotBeNegative().Should().Be(0m);
        0.000m.MustNotBeNegative().Should().Be(0m);
    }

    [Fact]
    public static void NonNegativeTimeSpansAreAccepted()
    {
        TimeSpan.Zero.MustNotBeNegative().Should().Be(TimeSpan.Zero);
        TimeSpan.FromTicks(1).MustNotBeNegative().Should().Be(TimeSpan.FromTicks(1));
        TimeSpan.MaxValue.MustNotBeNegative().Should().Be(TimeSpan.MaxValue);
    }

    [Fact]
    public static void NegativeTimeSpansAreRejected()
    {
        CheckTimeSpanIsRejected(TimeSpan.FromTicks(-1));
        CheckTimeSpanIsRejected(TimeSpan.MinValue);
    }

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        const int invalidValue = -3;

        var act = () => invalidValue.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage("*must not be negative, but it actually is -3*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => (-1).MustNotBeNegative(message: message));

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException(-1, (value, factory) => value.MustNotBeNegative(factory));
        Test.CustomException(-1L, (value, factory) => value.MustNotBeNegative(factory));
        Test.CustomException(-0.5m, (value, factory) => value.MustNotBeNegative(factory));
        Test.CustomException(float.NaN, (value, factory) => value.MustNotBeNegative(factory));
        Test.CustomException(double.NegativeInfinity, (value, factory) => value.MustNotBeNegative(factory));
        Test.CustomException(TimeSpan.FromTicks(-1), (value, factory) => value.MustNotBeNegative(factory));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void GenericOverloadsCoverTypesWithoutConcreteOverloads()
    {
        ((short) 0).MustNotBeNegative().Should().Be((short) 0);
        ((short) 5).MustNotBeNegative().Should().Be((short) 5);
        ((byte) 3).MustNotBeNegative().Should().Be((byte) 3);
        Half.Zero.MustNotBeNegative().Should().Be(Half.Zero);

        var negativeShort = () => ((short) -3).MustNotBeNegative();
        var nanHalf = () => Half.NaN.MustNotBeNegative();
        negativeShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
        nanHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");

        Test.CustomException((short) -3, (value, factory) => value.MustNotBeNegative(factory));
    }
#endif

    private static void CheckDecimalIsRejected(decimal value)
    {
        var act = () => value.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
    }

    private static void CheckTimeSpanIsRejected(TimeSpan value)
    {
        var act = () => value.MustNotBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be negative*");
    }
}
