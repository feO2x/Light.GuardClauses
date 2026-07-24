using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustNotBeZeroTests
{
    [Fact]
    public static void NonZeroSBytesAreAccepted()
    {
        ((sbyte) 1).MustNotBeZero().Should().Be(1);
        ((sbyte) -1).MustNotBeZero().Should().Be(-1);
        sbyte.MinValue.MustNotBeZero().Should().Be(sbyte.MinValue);
        sbyte.MaxValue.MustNotBeZero().Should().Be(sbyte.MaxValue);
    }

    [Fact]
    public static void ZeroSByteIsRejected() =>
        CheckIntegralIsRejected((sbyte) 0, value => value.MustNotBeZero());

    [Fact]
    public static void NonZeroBytesAreAccepted()
    {
        ((byte) 1).MustNotBeZero().Should().Be(1);
        byte.MaxValue.MustNotBeZero().Should().Be(byte.MaxValue);
    }

    [Fact]
    public static void ZeroByteIsRejected() =>
        CheckIntegralIsRejected((byte) 0, value => value.MustNotBeZero());

    [Fact]
    public static void NonZeroInt16sAreAccepted()
    {
        ((short) 1).MustNotBeZero().Should().Be(1);
        ((short) -1).MustNotBeZero().Should().Be(-1);
        short.MinValue.MustNotBeZero().Should().Be(short.MinValue);
        short.MaxValue.MustNotBeZero().Should().Be(short.MaxValue);
    }

    [Fact]
    public static void ZeroInt16IsRejected() =>
        CheckIntegralIsRejected((short) 0, value => value.MustNotBeZero());

    [Fact]
    public static void NonZeroUInt16sAreAccepted()
    {
        ((ushort) 1).MustNotBeZero().Should().Be(1);
        ushort.MaxValue.MustNotBeZero().Should().Be(ushort.MaxValue);
    }

    [Fact]
    public static void ZeroUInt16IsRejected() =>
        CheckIntegralIsRejected((ushort) 0, value => value.MustNotBeZero());

    [Theory]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    [InlineData(int.MaxValue)]
    public static void NonZeroInt32sAreAccepted(int value) => value.MustNotBeZero().Should().Be(value);

    [Fact]
    public static void NonZeroUInt32sAreAccepted()
    {
        1U.MustNotBeZero().Should().Be(1U);
        uint.MaxValue.MustNotBeZero().Should().Be(uint.MaxValue);
    }

    [Fact]
    public static void ZeroUInt32IsRejected() =>
        CheckIntegralIsRejected(0U, value => value.MustNotBeZero());

    [Theory]
    [InlineData(1L)]
    [InlineData(-1L)]
    [InlineData(long.MinValue)]
    [InlineData(long.MaxValue)]
    public static void NonZeroInt64sAreAccepted(long value) => value.MustNotBeZero().Should().Be(value);

    [Fact]
    public static void NonZeroUInt64sAreAccepted()
    {
        1UL.MustNotBeZero().Should().Be(1UL);
        ulong.MaxValue.MustNotBeZero().Should().Be(ulong.MaxValue);
    }

    [Fact]
    public static void ZeroUInt64IsRejected() =>
        CheckIntegralIsRejected(0UL, value => value.MustNotBeZero());

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
        CheckDecimalIsRejected(new (0, 0, 0, true, 0));
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
    public static void NewIntegralOverloadPropagatesParameterNameAndCustomMessage()
    {
        const ulong invalidValue = 0UL;

        var act = () => invalidValue.MustNotBeZero("quantity", "A non-zero quantity is required.");

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("quantity")
           .WithMessage("A non-zero quantity is required.*");
    }

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException((sbyte) 0, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException((byte) 0, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException((short) 0, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException((ushort) 0, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0U, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0L, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0UL, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0m, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0f, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(0d, (value, factory) => value.MustNotBeZero(factory));
        Test.CustomException(TimeSpan.Zero, (value, factory) => value.MustNotBeZero(factory));
    }

    [Fact]
    public static void NullFactoriesThrowArgumentNullExceptionForNewIntegralOverloads()
    {
        CheckNullFactory(() => ((sbyte) 0).MustNotBeZero(null!));
        CheckNullFactory(() => ((byte) 0).MustNotBeZero(null!));
        CheckNullFactory(() => ((short) 0).MustNotBeZero(null!));
        CheckNullFactory(() => ((ushort) 0).MustNotBeZero(null!));
        CheckNullFactory(() => 0U.MustNotBeZero(null!));
        CheckNullFactory(() => 0UL.MustNotBeZero(null!));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void ExplicitGenericOverloadsRemainAvailable()
    {
        ((short) 5).MustNotBeZero<short>().Should().Be(5);
        Half.NaN.MustNotBeZero().Should().Be(Half.NaN);

        var zeroShort = () => ((short) 0).MustNotBeZero<short>();
        var zeroHalf = () => Half.Zero.MustNotBeZero();
        var negativeZeroHalf = () => Half.NegativeZero.MustNotBeZero();
        zeroShort.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
        zeroHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");
        negativeZeroHalf.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*must not be zero*");

        Test.CustomException(
            (short) 0,
            (value, factory) => value.MustNotBeZero<short>(factory)
        );
    }
#endif

    private static void CheckIntegralIsRejected<T>(T value, Func<T, T> guard)
    {
        var act = () => guard(value);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(value))
           .WithMessage($"*value must not be zero, but it actually is {value}*");
    }

    private static void CheckNullFactory(Action act) =>
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");

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
