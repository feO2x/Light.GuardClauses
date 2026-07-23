using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBePositiveTests
{
    [Fact]
    public static void PositiveSBytesAreAccepted()
    {
        ((sbyte) 1).MustBePositive().Should().Be((sbyte) 1);
        sbyte.MaxValue.MustBePositive().Should().Be(sbyte.MaxValue);
    }

    [Fact]
    public static void NonPositiveSBytesAreRejected()
    {
        CheckIntegralIsRejected((sbyte) 0, value => value.MustBePositive());
        CheckIntegralIsRejected(sbyte.MinValue, value => value.MustBePositive());
    }

    [Fact]
    public static void PositiveBytesAreAccepted()
    {
        ((byte) 1).MustBePositive().Should().Be((byte) 1);
        byte.MaxValue.MustBePositive().Should().Be(byte.MaxValue);
    }

    [Fact]
    public static void ZeroByteIsRejected() =>
        CheckIntegralIsRejected((byte) 0, value => value.MustBePositive());

    [Fact]
    public static void PositiveInt16sAreAccepted()
    {
        ((short) 1).MustBePositive().Should().Be((short) 1);
        short.MaxValue.MustBePositive().Should().Be(short.MaxValue);
    }

    [Fact]
    public static void NonPositiveInt16sAreRejected()
    {
        CheckIntegralIsRejected((short) 0, value => value.MustBePositive());
        CheckIntegralIsRejected(short.MinValue, value => value.MustBePositive());
    }

    [Fact]
    public static void PositiveUInt16sAreAccepted()
    {
        ((ushort) 1).MustBePositive().Should().Be((ushort) 1);
        ushort.MaxValue.MustBePositive().Should().Be(ushort.MaxValue);
    }

    [Fact]
    public static void ZeroUInt16IsRejected() =>
        CheckIntegralIsRejected((ushort) 0, value => value.MustBePositive());

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

    [Fact]
    public static void PositiveUInt32sAreAccepted()
    {
        1U.MustBePositive().Should().Be(1U);
        uint.MaxValue.MustBePositive().Should().Be(uint.MaxValue);
    }

    [Fact]
    public static void ZeroUInt32IsRejected() =>
        CheckIntegralIsRejected(0U, value => value.MustBePositive());

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
    public static void PositiveUInt64sAreAccepted()
    {
        1UL.MustBePositive().Should().Be(1UL);
        ulong.MaxValue.MustBePositive().Should().Be(ulong.MaxValue);
    }

    [Fact]
    public static void ZeroUInt64IsRejected() =>
        CheckIntegralIsRejected(0UL, value => value.MustBePositive());

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
    public static void NewIntegralOverloadPropagatesParameterNameAndCustomMessage()
    {
        const uint invalidValue = 0U;

        var act = () => invalidValue.MustBePositive("quantity", "A positive quantity is required.");

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("quantity")
           .WithMessage("A positive quantity is required.*");
    }

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException((sbyte) -1, (value, factory) => value.MustBePositive(factory));
        Test.CustomException((byte) 0, (value, factory) => value.MustBePositive(factory));
        Test.CustomException((short) -1, (value, factory) => value.MustBePositive(factory));
        Test.CustomException((ushort) 0, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(0, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(0U, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(-1L, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(0UL, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(0m, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(float.NaN, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(double.NegativeInfinity, (value, factory) => value.MustBePositive(factory));
        Test.CustomException(TimeSpan.Zero, (value, factory) => value.MustBePositive(factory));
    }

    [Fact]
    public static void NullFactoriesThrowArgumentNullExceptionForNewIntegralOverloads()
    {
        CheckNullFactory(() => ((sbyte) 0).MustBePositive((Func<sbyte, Exception>) null!));
        CheckNullFactory(() => ((byte) 0).MustBePositive((Func<byte, Exception>) null!));
        CheckNullFactory(() => ((short) 0).MustBePositive((Func<short, Exception>) null!));
        CheckNullFactory(() => ((ushort) 0).MustBePositive((Func<ushort, Exception>) null!));
        CheckNullFactory(() => 0U.MustBePositive((Func<uint, Exception>) null!));
        CheckNullFactory(() => 0UL.MustBePositive((Func<ulong, Exception>) null!));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void ExplicitGenericOverloadsRemainAvailable()
    {
        Check.MustBePositive<short>((short) 5).Should().Be((short) 5);
        Check.MustBePositive<byte>((byte) 3).Should().Be((byte) 3);
        ((Half) 1.5f).MustBePositive().Should().Be((Half) 1.5f);

        var zeroShort = () => Check.MustBePositive<short>((short) 0);
        var nanHalf = () => Half.NaN.MustBePositive();
        zeroShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");
        nanHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must be positive*");

        Test.CustomException(
            (short) -3,
            (value, factory) => Check.MustBePositive<short>(value, factory)
        );
    }
#endif

    private static void CheckIntegralIsRejected<T>(T value, Func<T, T> guard)
    {
        var act = () => guard(value);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(value))
           .WithMessage($"*value must be positive, but it actually is {value}*");
    }

    private static void CheckNullFactory(Action act) =>
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");

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
