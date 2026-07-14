using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class NumericCustomFactorySuccessTests
{
    [Fact]
    public static void MustBePositiveReturnsValidValuesWithoutInvokingFactories()
    {
        1.MustBePositive(_ => throw new InvalidOperationException()).Should().Be(1);
        2L.MustBePositive(_ => throw new InvalidOperationException()).Should().Be(2L);
        3m.MustBePositive(_ => throw new InvalidOperationException()).Should().Be(3m);
        4f.MustBePositive(_ => throw new InvalidOperationException()).Should().Be(4f);
        5d.MustBePositive(_ => throw new InvalidOperationException()).Should().Be(5d);
        TimeSpan.FromTicks(6).MustBePositive(_ => throw new InvalidOperationException()).Should()
                .Be(TimeSpan.FromTicks(6));
        ((short) 7).MustBePositive(_ => throw new InvalidOperationException()).Should().Be(7);
    }

    [Fact]
    public static void MustBeNegativeReturnsValidValuesWithoutInvokingFactories()
    {
        (-1).MustBeNegative(_ => throw new InvalidOperationException()).Should().Be(-1);
        (-2L).MustBeNegative(_ => throw new InvalidOperationException()).Should().Be(-2L);
        (-3m).MustBeNegative(_ => throw new InvalidOperationException()).Should().Be(-3m);
        (-4f).MustBeNegative(_ => throw new InvalidOperationException()).Should().Be(-4f);
        (-5d).MustBeNegative(_ => throw new InvalidOperationException()).Should().Be(-5d);
        TimeSpan.FromTicks(-6).MustBeNegative(_ => throw new InvalidOperationException()).Should()
                .Be(TimeSpan.FromTicks(-6));
        ((short) -7).MustBeNegative(_ => throw new InvalidOperationException()).Should().Be(-7);
    }

    [Fact]
    public static void MustNotBePositiveReturnsValidValuesWithoutInvokingFactories()
    {
        0.MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(0);
        0L.MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(0L);
        0m.MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(0m);
        0f.MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(0f);
        0d.MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(0d);
        TimeSpan.Zero.MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(TimeSpan.Zero);
        ((short) 0).MustNotBePositive(_ => throw new InvalidOperationException()).Should().Be(0);
    }

    [Fact]
    public static void MustNotBeNegativeReturnsValidValuesWithoutInvokingFactories()
    {
        0.MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(0);
        0L.MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(0L);
        0m.MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(0m);
        0f.MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(0f);
        0d.MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(0d);
        TimeSpan.Zero.MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(TimeSpan.Zero);
        ((short) 0).MustNotBeNegative(_ => throw new InvalidOperationException()).Should().Be(0);
    }

    [Fact]
    public static void MustNotBeZeroReturnsValidValuesWithoutInvokingFactories()
    {
        1.MustNotBeZero(_ => throw new InvalidOperationException()).Should().Be(1);
        2L.MustNotBeZero(_ => throw new InvalidOperationException()).Should().Be(2L);
        3m.MustNotBeZero(_ => throw new InvalidOperationException()).Should().Be(3m);
        4f.MustNotBeZero(_ => throw new InvalidOperationException()).Should().Be(4f);
        5d.MustNotBeZero(_ => throw new InvalidOperationException()).Should().Be(5d);
        TimeSpan.FromTicks(6).MustNotBeZero(_ => throw new InvalidOperationException()).Should()
                .Be(TimeSpan.FromTicks(6));
        ((short) 7).MustNotBeZero(_ => throw new InvalidOperationException()).Should().Be(7);
    }
}
