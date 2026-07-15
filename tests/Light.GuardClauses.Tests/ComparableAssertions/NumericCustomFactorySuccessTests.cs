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
    public static void MustBePositive_Int() =>
        1.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(1);

    [Fact]
    public static void MustBePositive_Long() =>
        2L.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(2L);

    [Fact]
    public static void MustBePositive_Decimal() =>
        3m.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(3m);

    [Fact]
    public static void MustBePositive_Float() =>
        4f.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(4f);

    [Fact]
    public static void MustBePositive_Double() =>
        5d.MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(5d);

    [Fact]
    public static void MustBePositive_TimeSpan() =>
        TimeSpan.FromTicks(6).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.FromTicks(6));

    [Fact]
    public static void MustBePositive_Generic() =>
        ((short) 7).MustBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(7);

    [Fact]
    public static void MustBeNegative_Int() =>
        (-1).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-1);

    [Fact]
    public static void MustBeNegative_Long() =>
        (-2L).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-2L);

    [Fact]
    public static void MustBeNegative_Decimal() =>
        (-3m).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-3m);

    [Fact]
    public static void MustBeNegative_Float() =>
        (-4f).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-4f);

    [Fact]
    public static void MustBeNegative_Double() =>
        (-5d).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-5d);

    [Fact]
    public static void MustBeNegative_TimeSpan() =>
        TimeSpan.FromTicks(-6).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.FromTicks(-6));

    [Fact]
    public static void MustBeNegative_Generic() =>
        ((short) -7).MustBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(-7);

    [Fact]
    public static void MustNotBePositive_Int() =>
        0.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBePositive_Long() =>
        0L.MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0L);

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
        ((short) 0).MustNotBePositive(_ => FactoryMustNotBeInvoked()).Should().Be(0);

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
        ((short) 0).MustNotBeNegative(_ => FactoryMustNotBeInvoked()).Should().Be(0);

    [Fact]
    public static void MustNotBeZero_Int() =>
        1.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(1);

    [Fact]
    public static void MustNotBeZero_Long() =>
        2L.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(2L);

    [Fact]
    public static void MustNotBeZero_Decimal() =>
        3m.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(3m);

    [Fact]
    public static void MustNotBeZero_Float() =>
        4f.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(4f);

    [Fact]
    public static void MustNotBeZero_Double() =>
        5d.MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(5d);

    [Fact]
    public static void MustNotBeZero_TimeSpan() =>
        TimeSpan.FromTicks(6).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(TimeSpan.FromTicks(6));

    [Fact]
    public static void MustNotBeZero_Generic() =>
        ((short) 7).MustNotBeZero(_ => FactoryMustNotBeInvoked()).Should().Be(7);
}
