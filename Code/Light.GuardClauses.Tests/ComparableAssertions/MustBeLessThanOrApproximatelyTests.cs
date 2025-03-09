using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBeLessThanOrApproximatelyTests
{
    [Theory]
    [InlineData(17.3, 17.4)]
    [InlineData(19.9999999, 20.0)]
    [InlineData(-5.5, -5.49998)]
    [InlineData(0.0001, 0.0001)]
    public static void EqualOrLess_Double(double first, double second) =>
        first.MustBeLessThanOrApproximately(second).Should().Be(first);

    [Theory]
    [InlineData(15.9, 15.91, 0.1)]
    [InlineData(24.45, 24.49999, 0.0001)]
    [InlineData(-3.2, -3.12, 0.001)]
    [InlineData(2.37, 2.369, 0.05)]
    public static void EqualOrLessWithTolerance_Double(double first, double second, double tolerance) =>
        first.MustBeLessThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(100.2f, 100.225f)]
    [InlineData(-5.900005f, -5.9f)]
    [InlineData(-0.02f, 0f)]
    [InlineData(0f, 0.00001f)]
    public static void EqualOrLess_Float(float first, float second) =>
        first.MustBeLessThanOrApproximately(second).Should().Be(first);

    [Theory]
    [InlineData(1.0f, 2.0f, 0.1f)]
    [InlineData(1.0f, 1.0f, 0.1f)]
    [InlineData(1.1f, 1.01f, 0.1f)]
    [InlineData(1.0f, 2.0f, 1.0f)]
    public static void EqualOrLessWithTolerance_Float(float first, float second, float tolerance) =>
        first.MustBeLessThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(5.3, 5.0, 0.1)]
    [InlineData(100.5, 100.0, 0.1)]
    [InlineData(-19.8, -20.0, 0.1)]
    [InlineData(0.0003, 0.0001, 0.00005)]
    public static void NotLessThanOrApproximately_Double(double value, double other, double tolerance)
    {
        var act = () => value.MustBeLessThanOrApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be less than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Theory]
    [InlineData(5.3f, 5.0f, 0.1f)]
    [InlineData(100.5f, 100.0f, 0.1f)]
    [InlineData(-19.8f, -20.0f, 0.1f)]
    [InlineData(0.0003f, 0.0001f, 0.00005f)]
    public static void NotLessThanOrApproximately_Float(float value, float other, float tolerance)
    {
        var act = () => value.MustBeLessThanOrApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be less than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void DefaultTolerance_Double()
    {
        // Should pass - difference is 0.00005 which is less than default tolerance 0.0001
        const double value = 1.0;
        value.MustBeLessThanOrApproximately(1.00005).Should().Be(value);

        // Should throw - difference is 0.0002 which is greater than default tolerance 0.0001
        Action act = () => 1.0002.MustBeLessThanOrApproximately(1.0, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");
    }

    [Fact]
    public static void DefaultTolerance_Float()
    {
        // Should pass - difference is 0.00005f which is less than default tolerance 0.0001f
        const float value = 1.0f;
        value.MustBeLessThanOrApproximately(1.00005f).Should().Be(value);

        // Should throw - difference is 0.0002f which is greater than default tolerance 0.0001f
        Action act = () => 1.0002f.MustBeLessThanOrApproximately(1.0f, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");
    }

    [Fact]
    public static void CustomException_Double() =>
        Test.CustomException(
            5.3,
            5.0,
            (x, y, exceptionFactory) => x.MustBeLessThanOrApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Double() =>
        Test.CustomException(
            5.5,
            5.0,
            0.1,
            (x, y, z, exceptionFactory) => x.MustBeLessThanOrApproximately(y, z, exceptionFactory)
        );

    [Fact]
    public static void CustomException_Float() =>
        Test.CustomException(
            5.3f,
            5.0f,
            (x, y, exceptionFactory) => x.MustBeLessThanOrApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Float() =>
        Test.CustomException(
            5.5f,
            5.0f,
            0.1f,
            (x, y, z, exceptionFactory) => x.MustBeLessThanOrApproximately(y, z, exceptionFactory)
        );
    
    [Fact]
    public static void NoCustomExceptionThrown_Double() =>
        5.1.MustBeLessThanOrApproximately(5.2, (_, _) => null).Should().Be(5.1);

    [Fact]
    public static void NoCustomExceptionThrownWithTolerance_Double() =>
        5.0.MustBeLessThanOrApproximately(5.2, 0.1, (_, _, _) => null).Should().Be(5.0);
    
    [Fact]
    public static void NoCustomExceptionThrown_Float() =>
        5.0f.MustBeLessThanOrApproximately(5.2f, (_, _) => null).Should().Be(5.0f);

    [Fact]
    public static void NoCustomExceptionThrownWithTolerance_Float() =>
        5.0f.MustBeLessThanOrApproximately(5.2f, 0.1f, (_, _, _) => null).Should().Be(5.0f);

    [Fact]
    public static void CustomMessage_Double() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 101.0.MustBeLessThanOrApproximately(100.0, 0.5, message: message)
        );

    [Fact]
    public static void CustomMessage_Float() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 101.0f.MustBeLessThanOrApproximately(100.0f, 0.5f, message: message)
        );

    [Fact]
    public static void CallerArgumentExpression_Double()
    {
        const double seventyNinePoint0 = 79.0;

        var act = () => seventyNinePoint0.MustBeLessThanOrApproximately(78.1);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyNinePoint0));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Double()
    {
        const double threePointFive = 3.5;

        var act = () => threePointFive.MustBeLessThanOrApproximately(3.14159, 0.1);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(threePointFive));
    }

    [Fact]
    public static void CallerArgumentExpression_Float()
    {
        const float seventyNinePoint0 = 79.0f;

        var act = () => seventyNinePoint0.MustBeLessThanOrApproximately(78.1f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyNinePoint0));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Float()
    {
        const float threePointFive = 3.5f;

        var act = () => threePointFive.MustBeLessThanOrApproximately(3.14159f, 0.1f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(threePointFive));
    }

#if NET8_0
    [Theory]
    [InlineData(15.9, 15.91, 0.1)]
    [InlineData(24.45, 24.4999, 0.0001)]
    [InlineData(-3.2, -3.12, 0.001)]
    [InlineData(2.37, 2.369, 0.05)]
    [InlineData(14.0, 15.0, 0.1)] // Less than case
    [InlineData(15.0, 14.95, 0.1)] // Approximately equal case
    public static void EqualOrLessWithCustomTolerance_GenericDouble(double first, double second, double tolerance) =>
        first.MustBeLessThanOrApproximately<double>(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(5, 10, 1)] // Less than case
    [InlineData(5, 5, 1)] // Equal case
    [InlineData(6, 5, 1)] // Approximately equal case
    [InlineData(7, 5, 2)] // Not less than or approximately equal case
    public static void EqualOrLessWithCustomTolerance_GenericInt32(int first, int second, int tolerance) =>
        first.MustBeLessThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(5L, 10L, 1L)] // Less than case
    [InlineData(5L, 5L, 1L)] // Equal case
    [InlineData(6L, 5L, 1L)] // Approximately equal case
    [InlineData(7L, 4L, 3L)] // Not less than or approximately equal case
    public static void EqualOrLessWithCustomTolerance_GenericInt64(long first, long second, long tolerance) =>
        first.MustBeLessThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [MemberData(nameof(DecimalTestData))]
    public static void GenericDecimalWithCustomTolerance(
        decimal first,
        decimal second,
        decimal tolerance
    ) =>
        first.MustBeLessThanOrApproximately(second, tolerance).Should().Be(first);

    public static TheoryData<decimal, decimal, decimal> DecimalTestData() => new ()
    {
        { 1.1m, 1.3m, 0.1m }, // Less than case
        { 1.1m, 1.1m, 0.1m }, // Equal case
        { 1.1m, 1.0m, 0.2m }, // Approximately equal case
        { 1.3m, 1.292m, 0.1m }, // Not less than or approximately equal case
    };
    
    [Theory]
    [InlineData(5.3, 5.0, 0.1)]
    [InlineData(100.5, 100.0, 0.1)]
    [InlineData(-19.8, -20.0, 0.1)]
    [InlineData(0.0003, 0.0001, 0.00005)]
    public static void NotLessThanOrApproximately_Generic(double value, double other, double tolerance)
    {
        var act = () => value.MustBeLessThanOrApproximately<double>(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be less than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void CustomExceptionWithTolerance_Generic() =>
        Test.CustomException(
            5.5,
            5.0,
            0.1,
            (x, y, t, exceptionFactory) => x.MustBeLessThanOrApproximately<double>(y, t, exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown_Generic() =>
        5.0.MustBeLessThanOrApproximately<double>(5.2, 0.1, (_, _, _) => null).Should().Be(5.0);
    
    [Fact]
    public static void CustomMessage_Generic() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 101.0.MustBeLessThanOrApproximately<double>(100.0, 0.5, message: message)
        );

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Generic()
    {
        const double seventyNinePoint0 = 79.0;

        var act = () => seventyNinePoint0.MustBeLessThanOrApproximately<double>(78.1, 0.5);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyNinePoint0));
    }
#endif
}