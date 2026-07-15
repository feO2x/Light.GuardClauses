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
    public static void IsFinite_FiniteDoublesAreAccepted(double value) => value.IsFinite().Should().BeTrue();

    [Fact]
    public static void IsFinite_NegativeZeroDoubleIsAccepted() => (-0d).IsFinite().Should().BeTrue();

    [Theory]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public static void IsFinite_NonFiniteDoublesAreRejected(double value) => value.IsFinite().Should().BeFalse();

    [Theory]
    [InlineData(0f)]
    [InlineData(float.Epsilon)]
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    public static void IsFinite_FiniteFloatsAreAccepted(float value) => value.IsFinite().Should().BeTrue();

    [Fact]
    public static void IsFinite_NegativeZeroFloatIsAccepted() => (-0f).IsFinite().Should().BeTrue();

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public static void IsFinite_NonFiniteFloatsAreRejected(float value) => value.IsFinite().Should().BeFalse();

    [Theory]
    [InlineData(0d)]
    [InlineData(double.Epsilon)]
    [InlineData(double.MinValue)]
    [InlineData(double.MaxValue)]
    public static void MustBeFinite_FiniteDoublesAreReturned(double value) =>
        value.MustBeFinite().Should().Be(value);

    [Theory]
    [InlineData(0f)]
    [InlineData(float.Epsilon)]
    [InlineData(float.MinValue)]
    [InlineData(float.MaxValue)]
    public static void MustBeFinite_FiniteFloatsAreReturned(float value) =>
        value.MustBeFinite().Should().Be(value);

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
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => float.PositiveInfinity.MustBeFinite(message: message)
        );

    [Fact]
    public static void CustomFactoryReceivesValue_Float() =>
        Test.CustomException(float.NaN, (value, factory) => value.MustBeFinite(factory));

    [Fact]
    public static void CustomFactoryReceivesValue_Double() =>
        Test.CustomException(double.NegativeInfinity, (value, factory) => value.MustBeFinite(factory));

    [Fact]
    public static void NoCustomExceptionThrown_Double() =>
        42.5.MustBeFinite(_ => null).Should().Be(42.5);

    [Fact]
    public static void NoCustomExceptionThrown_Float() =>
        42.5f.MustBeFinite(_ => null).Should().Be(42.5f);

#if NET8_0_OR_GREATER
    [Fact]
    public static void IsFinite_FiniteHalfIsAccepted() =>
        ((Half) 1.5f).IsFinite().Should().BeTrue();

    [Fact]
    public static void IsFinite_NonFiniteHalfIsRejected() =>
        Half.PositiveInfinity.IsFinite().Should().BeFalse();

    [Fact]
    public static void MustBeFinite_FiniteHalfIsReturned()
    {
        var finiteHalf = (Half) 1.5f;

        finiteHalf.MustBeFinite().Should().Be(finiteHalf);
    }

    [Fact]
    public static void CustomFactoryReceivesValue_Generic() =>
        Test.CustomException(Half.PositiveInfinity, (value, factory) => value.MustBeFinite(factory));

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue_Generic()
    {
        var nonFiniteHalf = Half.NaN;

        var act = () => nonFiniteHalf.MustBeFinite();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(nonFiniteHalf))
           .WithMessage("*must be finite*");
    }

    [Fact]
    public static void NoCustomExceptionThrown_Generic()
    {
        var finiteHalf = (Half) 1.5f;

        finiteHalf.MustBeFinite(_ => null).Should().Be(finiteHalf);
    }
#endif
}
