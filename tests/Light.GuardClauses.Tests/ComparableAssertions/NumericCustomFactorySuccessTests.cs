using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

// Each test targets exactly one numeric overload so that a failing test
// immediately identifies the defective production method.
public static class NumericCustomFactorySuccessTests
{
    private static InvalidOperationException FactoryMustNotBeInvoked() =>
        throw new InvalidOperationException("The factory must not be invoked.");

    [Fact]
    public static void MustBePositive_SByte() =>
        ((sbyte) 1).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(1);

    [Fact]
    public static void MustBePositive_Byte() =>
        ((byte) 2).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(2);

    [Fact]
    public static void MustBePositive_Short() =>
        ((short) 3).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(3);

    [Fact]
    public static void MustBePositive_UShort() =>
        ((ushort) 4).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(4);

    [Fact]
    public static void MustBePositive_Int() =>
        5.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(5);

    [Fact]
    public static void MustBePositive_UInt() =>
        6U.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(6U);

    [Fact]
    public static void MustBePositive_Long() =>
        7L.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(7L);

    [Fact]
    public static void MustBePositive_ULong() =>
        8UL.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(8UL);

    [Fact]
    public static void MustBePositive_Decimal() =>
        9m.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(9m);

    [Fact]
    public static void MustBePositive_Float() =>
        10f.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(10f);

    [Fact]
    public static void MustBePositive_Double() =>
        11d.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(11d);

    [Fact]
    public static void MustBePositive_TimeSpan() =>
        TimeSpan.FromTicks(12).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.FromTicks(12));

    [Fact]
    public static void MustBePositive_Generic() =>
        ((short) 13).MustBePositive<short>(_ => FactoryMustNotBeInvoked()).Should().Be(13);

    [Fact]
    public static void MustBeNegative_SByte() =>
        ((sbyte) -1).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-1);

    [Fact]
    public static void MustBeNegative_Short() =>
        ((short) -2).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-2);

    [Fact]
    public static void MustBeNegative_Int() =>
        (-3).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-3);

    [Fact]
    public static void MustBeNegative_Long() =>
        (-4L).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-4L);

    [Fact]
    public static void MustBeNegative_Decimal() =>
        (-5m).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-5m);

    [Fact]
    public static void MustBeNegative_Float() =>
        (-6f).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-6f);

    [Fact]
    public static void MustBeNegative_Double() =>
        (-7d).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-7d);

    [Fact]
    public static void MustBeNegative_TimeSpan() =>
        TimeSpan.FromTicks(-8).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.FromTicks(-8));

    [Fact]
    public static void MustBeNegative_Generic() =>
        ((short) -9).MustBeNegative<short>(_ => FactoryMustNotBeInvoked()).Should().Be(-9);

    [Fact]
    public static void MustNotBePositive_SByte() =>
        ((sbyte) 0).MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBePositive_Byte() =>
        ((byte) 0).MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBePositive_Short() =>
        ((short) 0).MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBePositive_UShort() =>
        ((ushort) 0).MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBePositive_Int() =>
        0.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBePositive_UInt() =>
        0U.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0U);

    [Fact]
    public static void MustNotBePositive_Long() =>
        0L.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0L);

    [Fact]
    public static void MustNotBePositive_ULong() =>
        0UL.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0UL);

    [Fact]
    public static void MustNotBePositive_Decimal() =>
        0m.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0m);

    [Fact]
    public static void MustNotBePositive_Float() =>
        0f.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0f);

    [Fact]
    public static void MustNotBePositive_Double() =>
        0d.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0d);

    [Fact]
    public static void MustNotBePositive_TimeSpan() =>
        TimeSpan.Zero.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.Zero);

    [Fact]
    public static void MustNotBePositive_Generic() =>
        ((short) 0).MustNotBePositive<short>(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBeNegative_SByte() =>
        ((sbyte) 0).MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBeNegative_Short() =>
        ((short) 0).MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBeNegative_Int() =>
        0.MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBeNegative_Long() =>
        0L.MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0L);

    [Fact]
    public static void MustNotBeNegative_Decimal() =>
        0m.MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0m);

    [Fact]
    public static void MustNotBeNegative_Float() =>
        0f.MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0f);

    [Fact]
    public static void MustNotBeNegative_Double() =>
        0d.MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0d);

    [Fact]
    public static void MustNotBeNegative_TimeSpan() =>
        TimeSpan.Zero.MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.Zero);

    [Fact]
    public static void MustNotBeNegative_Generic() =>
        ((short) 0).MustNotBeNegative<short>(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBeZero_SByte() =>
        ((sbyte) 1).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(1);

    [Fact]
    public static void MustNotBeZero_Byte() =>
        ((byte) 2).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(2);

    [Fact]
    public static void MustNotBeZero_Short() =>
        ((short) 3).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(3);

    [Fact]
    public static void MustNotBeZero_UShort() =>
        ((ushort) 4).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(4);

    [Fact]
    public static void MustNotBeZero_Int() =>
        5.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(5);

    [Fact]
    public static void MustNotBeZero_UInt() =>
        6U.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(6U);

    [Fact]
    public static void MustNotBeZero_Long() =>
        7L.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(7L);

    [Fact]
    public static void MustNotBeZero_ULong() =>
        8UL.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(8UL);

    [Fact]
    public static void MustNotBeZero_Decimal() =>
        9m.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(9m);

    [Fact]
    public static void MustNotBeZero_Float() =>
        10f.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(10f);

    [Fact]
    public static void MustNotBeZero_Double() =>
        11d.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(11d);

    [Fact]
    public static void MustNotBeZero_TimeSpan() =>
        TimeSpan.FromTicks(12).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.FromTicks(12));

    [Fact]
    public static void MustNotBeZero_Generic() =>
        ((short) 13).MustNotBeZero<short>(_ => FactoryMustNotBeInvoked()).Should().Be(13);
}
