using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class FiniteTests
{
    [Theory]
    [InlineData(0d)]
    [InlineData(double.Epsilon)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    public static void FiniteDoublesAreAccepted(double value)
    {
        value.IsFinite().Should().BeTrue();
        value.MustBeFinite().Should().Be(value);
    }

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public static void NonFiniteDoublesAreRejected(double value) => value.IsFinite().Should().BeFalse();

    [Theory]
    [InlineData(0f)]
    [InlineData(float.Epsilon)]
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    public static void FiniteFloatsAreAccepted(float value)
    {
        value.IsFinite().Should().BeTrue();
        value.MustBeFinite().Should().Be(value);
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public static void NonFiniteFloatsAreRejected(float value) => value.IsFinite().Should().BeFalse();

    [Fact]
    public static void NegativeZerosAreFinite()
    {
        (-0d).IsFinite().Should().BeTrue();
        (-0f).IsFinite().Should().BeTrue();
    }

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        const double invalidValue = double.NaN;

        var act = () => invalidValue.MustBeFinite();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage("*must be finite*");
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => float.PositiveInfinity.MustBeFinite(message: message));

    [Fact]
    public static void CustomFactoriesReceiveValues()
    {
        Test.CustomException(float.NaN, (value, factory) => value.MustBeFinite(factory));
        Test.CustomException(double.NegativeInfinity, (value, factory) => value.MustBeFinite(factory));
    }

#if NET8_0_OR_GREATER
    [Fact]
    public static void GenericHalfOverloadsAreAvailable()
    {
        var finite = (Half) 1.5f;
        var nonFinite = Half.PositiveInfinity;

        Check.IsFinite<Half>(finite).Should().BeTrue();
        Check.IsFinite<Half>(nonFinite).Should().BeFalse();
        Check.MustBeFinite<Half>(finite).Should().Be(finite);
        Test.CustomException(nonFinite, (value, factory) => Check.MustBeFinite(value, factory));
    }
#endif
}
