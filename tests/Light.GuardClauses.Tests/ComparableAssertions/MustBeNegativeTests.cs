using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBeNegativeTests
{
    [Fact]
    public static void NegativeSBytesAreAccepted()
    {
        ((sbyte) -1).MustBeNegative().Should().Be(-1);
        sbyte.MinValue.MustBeNegative().Should().Be(sbyte.MinValue);
    }

    [Fact]
    public static void NonNegativeSBytesAreRejected()
    {
        CheckIntegralIsRejected((sbyte) 0, value => value.MustBeNegative());
        CheckIntegralIsRejected(sbyte.MaxValue, value => value.MustBeNegative());
    }

    [Fact]
    public static void NegativeInt16sAreAccepted()
    {
        ((short) -1).MustBeNegative().Should().Be(-1);
        short.MinValue.MustBeNegative().Should().Be(short.MinValue);
    }

    [Fact]
    public static void NonNegativeInt16sAreRejected()
    {
        CheckIntegralIsRejected((short) 0, value => value.MustBeNegative());
        CheckIntegralIsRejected(short.MaxValue, value => value.MustBeNegative());
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-42)]
    [InlineData(int.MinValue)]
    public static void NegativeInt32sAreAccepted(int value) => value.MustBeNegative().Should().Be(value);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public static void NonNegativeInt32sAreRejected(int value)
    {
        var act = () => value.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
    }

    [Theory]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    public static void NegativeInt64sAreAccepted(long value) => value.MustBeNegative().Should().Be(value);

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    public static void NonNegativeInt64sAreRejected(long value)
    {
        var act = () => value.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
    }

    [Fact]
    public static void NegativeDecimalsAreAccepted()
    {
        (-0.00001m).MustBeNegative().Should().Be(-0.00001m);
        decimal.MinValue.MustBeNegative().Should().Be(decimal.MinValue);
    }

    [Fact]
    public static void NonNegativeDecimalsAreRejected()
    {
        CheckDecimalIsRejected(0m);
        CheckDecimalIsRejected(0.5m);
        CheckDecimalIsRejected(decimal.MaxValue);
    }

    [Theory]
    [InlineData(-float.Epsilon)]
    [InlineData(-1f)]
    [InlineData(float.MinValue)]
    [InlineData(float.NegativeInfinity)]
    public static void NegativeFloatsAreAccepted(float value) => value.MustBeNegative().Should().Be(value);

    [Theory]
    [InlineData(0f)]
    [InlineData(float.Epsilon)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NaN)]
    public static void NonNegativeFloatsAreRejected(float value)
    {
        var act = () => value.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
    }

    [Theory]
    [InlineData(-double.Epsilon)]
    [InlineData(-1d)]
    [InlineData(double.MinValue)]
    [InlineData(double.NegativeInfinity)]
    public static void NegativeDoublesAreAccepted(double value) => value.MustBeNegative().Should().Be(value);

    [Theory]
    [InlineData(0d)]
    [InlineData(double.Epsilon)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NaN)]
    public static void NonNegativeDoublesAreRejected(double value)
    {
        var act = () => value.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
    }

    [Fact]
    public static void NegativeZerosAreRejectedLikeZero()
    {
        var negativeZeroFloat = () => (-0f).MustBeNegative();
        var negativeZeroDouble = () => (-0d).MustBeNegative();
        negativeZeroFloat.Should().Throw<ArgumentOutOfRangeException>();
        negativeZeroDouble.Should().Throw<ArgumentOutOfRangeException>();

        CheckDecimalIsRejected(new (0, 0, 0, true, 0));
        CheckDecimalIsRejected(0.000m);
    }

    [Fact]
    public static void NegativeTimeSpansAreAccepted()
    {
        TimeSpan.FromTicks(-1).MustBeNegative().Should().Be(TimeSpan.FromTicks(-1));
        TimeSpan.MinValue.MustBeNegative().Should().Be(TimeSpan.MinValue);
    }

    [Fact]
    public static void NonNegativeTimeSpansAreRejected()
    {
        CheckTimeSpanIsRejected(TimeSpan.Zero);
        CheckTimeSpanIsRejected(TimeSpan.FromTicks(1));
        CheckTimeSpanIsRejected(TimeSpan.MaxValue);
    }

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        const int invalidValue = 7;

        var act = () => invalidValue.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage("*must be negative, but it actually is 7*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 1.MustBeNegative(message: message));

    [Fact]
    public static void NewIntegralOverloadPropagatesParameterNameAndCustomMessage()
    {
        const sbyte invalidValue = 0;

        var act = () => invalidValue.MustBeNegative("quantity", "A negative quantity is required.");

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("quantity")
           .WithMessage("A negative quantity is required.*");
    }

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException((sbyte) 0, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException((short) 0, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException(0, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException(1L, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException(0m, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException(float.NaN, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException(double.PositiveInfinity, (value, factory) => value.MustBeNegative(factory));
        Test.CustomException(TimeSpan.Zero, (value, factory) => value.MustBeNegative(factory));
    }

    [Fact]
    public static void NullFactoriesThrowArgumentNullExceptionForNewIntegralOverloads()
    {
        CheckNullFactory(() => ((sbyte) 0).MustBeNegative(null!));
        CheckNullFactory(() => ((short) 0).MustBeNegative(null!));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void ExplicitGenericOverloadsRemainAvailable()
    {
        ((short) -5).MustBeNegative<short>().Should().Be(-5);
        ((Half) (-1.5f)).MustBeNegative().Should().Be((Half) (-1.5f));

        var zeroShort = () => ((short) 0).MustBeNegative<short>();
        var unsignedByte = () => ((byte) 3).MustBeNegative();
        var nanHalf = () => Half.NaN.MustBeNegative();
        zeroShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
        unsignedByte.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
        nanHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");

        Test.CustomException(
            (short) 3,
            (value, factory) => value.MustBeNegative<short>(factory)
        );
    }
#endif

    private static void CheckIntegralIsRejected<T>(T value, Func<T, T> guard)
    {
        var act = () => guard(value);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(value))
           .WithMessage($"*value must be negative, but it actually is {value}*");
    }

    private static void CheckNullFactory(Action act) =>
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");

    private static void CheckDecimalIsRejected(decimal value)
    {
        var act = () => value.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
    }

    private static void CheckTimeSpanIsRejected(TimeSpan value)
    {
        var act = () => value.MustBeNegative();

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be negative*");
    }
}
