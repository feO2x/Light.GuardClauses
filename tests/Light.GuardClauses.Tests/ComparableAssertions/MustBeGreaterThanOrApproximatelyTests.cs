using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class CheckMustBeGreaterThanOrApproximatelyTests
{
    [Theory]
    [InlineData(17.4, 17.3)]
    [InlineData(19.9999999, 20.0)]
    [InlineData(-5.49998, -5.5)]
    [InlineData(0.0001, 0.0001)]
    public static void EqualOrGreater_Double(double first, double second) =>
        first.MustBeGreaterThanOrApproximately(second).Should().Be(first);

    [Theory]
    [InlineData(15.91, 15.9, 0.1)]
    [InlineData(24.49999, 24.45, 0.0001)]
    [InlineData(-3.12, -3.2, 0.001)]
    [InlineData(2.369, 2.37, 0.05)]
    public static void EqualOrGreaterWithTolerance_Double(double first, double second, double tolerance) =>
        first.MustBeGreaterThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(100.225f, 100.2f)]
    [InlineData(-5.9f, -5.900005f)]
    [InlineData(0f, -0.02f)]
    [InlineData(-0.00001f, 0f)]
    public static void EqualOrGreater_Float(float first, float second) =>
        first.MustBeGreaterThanOrApproximately(second).Should().Be(first);

    [Theory]
    [InlineData(2.0f, 1.0f, 0.1f)]
    [InlineData(1.0f, 1.0f, 0.1f)]
    [InlineData(1.01f, 1.1f, 0.1f)]
    [InlineData(1.0f, 2.0f, 1.0f)]
    public static void EqualOrGreaterWithTolerance_Float(float first, float second, float tolerance) =>
        first.MustBeGreaterThanOrApproximately(second, tolerance).Should().Be(first);
    
    [Theory]
    [InlineData(5.0, 5.3, 0.1)]
    [InlineData(100.0, 100.5, 0.1)]
    [InlineData(-20.0, -19.8, 0.1)]
    [InlineData(0.0001, 0.0003, 0.00005)]
    public static void NotGreaterThanOrApproximately_Double(double value, double other, double tolerance)
    {
        var act = () => value.MustBeGreaterThanOrApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be greater than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Theory]
    [InlineData(5.0f, 5.3f, 0.1f)]
    [InlineData(100.0f, 100.5f, 0.1f)]
    [InlineData(-20.0f, -19.8f, 0.1f)]
    [InlineData(0.0001f, 0.0003f, 0.00005f)]
    public static void NotGreaterThanOrApproximately_Float(float value, float other, float tolerance)
    {
        var act = () => value.MustBeGreaterThanOrApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be greater than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void DefaultTolerance_Double()
    {
        // Should pass - difference is 0.00005 which is less than default tolerance 0.0001
        const double value = 1.00005;
        value.MustBeGreaterThanOrApproximately(1.0).Should().Be(value);

        // Should throw - difference is 0.0002 which is greater than default tolerance 0.0001
        Action act = () => 0.9998.MustBeGreaterThanOrApproximately(1.0, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");
    }

    [Fact]
    public static void DefaultTolerance_Float()
    {
        // Should pass - difference is 0.00005f which is less than default tolerance 0.0001f
        const float value = 1.00005f;
        value.MustBeGreaterThanOrApproximately(1.0f).Should().Be(value);

        // Should throw - difference is 0.0002f which is greater than default tolerance 0.0001f
        Action act = () => 0.9998f.MustBeGreaterThanOrApproximately(1.0f, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");
    }

    [Fact]
    public static void CustomException_Double() =>
        Test.CustomException(
            5.0,
            5.3,
            (x, y, exceptionFactory) => x.MustBeGreaterThanOrApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Double() =>
        Test.CustomException(
            5.0,
            5.5,
            0.1,
            (x, y, z, exceptionFactory) => x.MustBeGreaterThanOrApproximately(y, z, exceptionFactory)
        );

    [Fact]
    public static void CustomException_Float() =>
        Test.CustomException(
            5.0f,
            5.3f,
            (x, y, exceptionFactory) => x.MustBeGreaterThanOrApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Float() =>
        Test.CustomException(
            5.0f,
            5.5f,
            0.1f,
            (x, y, z, exceptionFactory) => x.MustBeGreaterThanOrApproximately(y, z, exceptionFactory)
        );
    
    [Fact]
    public static void NoCustomExceptionThrown_Double() =>
        5.2.MustBeGreaterThanOrApproximately(5.1, (_, _) => null).Should().Be(5.2);

    [Fact]
    public static void NoCustomExceptionThrownWithTolerance_Double() =>
        5.2.MustBeGreaterThanOrApproximately(5.0, 0.1, (_, _, _) => null).Should().Be(5.2);
    
    [Fact]
    public static void NoCustomExceptionThrown_Float() =>
        5.2f.MustBeGreaterThanOrApproximately(5.0f, (_, _) => null).Should().Be(5.2f);

    [Fact]
    public static void NoCustomExceptionThrownWithTolerance_Float() =>
        5.2f.MustBeGreaterThanOrApproximately(5.0f, 0.1f, (_, _, _) => null).Should().Be(5.2f);

    [Fact]
    public static void CustomMessage_Double() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0.MustBeGreaterThanOrApproximately(101.0, 0.5, message: message)
        );

    [Fact]
    public static void CustomMessage_Float() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0f.MustBeGreaterThanOrApproximately(101.0f, 0.5f, message: message)
        );

    [Fact]
    public static void CallerArgumentExpression_Double()
    {
        const double seventyEightO1 = 78.1;

        var act = () => seventyEightO1.MustBeGreaterThanOrApproximately(79.0);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Double()
    {
        const double pi = 3.14159;

        var act = () => pi.MustBeGreaterThanOrApproximately(3.5, 0.1);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(pi));
    }

    [Fact]
    public static void CallerArgumentExpression_Float()
    {
        const float seventyEightO1 = 78.1f;

        var act = () => seventyEightO1.MustBeGreaterThanOrApproximately(79.0f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Float()
    {
        const float pi = 3.14159f;

        var act = () => pi.MustBeGreaterThanOrApproximately(3.5f, 0.1f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(pi));
    }

#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(15.91, 15.9, 0.1)]
    [InlineData(24.4999, 24.45, 0.0001)]
    [InlineData(-3.12, -3.2, 0.001)]
    [InlineData(2.369, 2.37, 0.05)]
    [InlineData(15.0, 14.0, 0.1)] // Greater than case
    [InlineData(14.95, 15.0, 0.1)] // Approximately equal case
    public static void EqualOrGreaterWithCustomTolerance_GenericDouble(double first, double second, double tolerance) =>
        first.MustBeGreaterThanOrApproximately<double>(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(10, 5, 1)] // Greater than case
    [InlineData(5, 5, 1)] // Equal case
    [InlineData(5, 6, 1)] // Approximately equal case
    [InlineData(5, 7, 2)] // Not greater than or approximately equal case
    public static void EqualOrGreaterWithCustomTolerance_GenericInt32(int first, int second, int tolerance) =>
        first.MustBeGreaterThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [InlineData(10L, 5L, 1L)] // Greater than case
    [InlineData(5L, 5L, 1L)] // Equal case
    [InlineData(5L, 6L, 1L)] // Approximately equal case
    [InlineData(4L, 7L, 3L)] // Not greater than or approximately equal case
    public static void EqualOrGreaterWithCustomTolerance_GenericInt64(long first, long second, long tolerance) =>
        first.MustBeGreaterThanOrApproximately(second, tolerance).Should().Be(first);

    [Theory]
    [MemberData(nameof(DecimalTestData))]
    public static void GenericDecimalWithCustomTolerance(
        decimal first,
        decimal second,
        decimal tolerance
    ) =>
        first.MustBeGreaterThanOrApproximately(second, tolerance).Should().Be(first);

    public static TheoryData<decimal, decimal, decimal> DecimalTestData() => new ()
    {
        { 1.3m, 1.1m, 0.1m }, // Greater than case
        { 1.1m, 1.1m, 0.1m }, // Equal case
        { 1.0m, 1.1m, 0.2m }, // Approximately equal case
        { 1.292m, 1.3m, 0.1m }, // Not greater than or approximately equal case
    };
    
    [Theory]
    [InlineData(5.0, 5.3, 0.1)]
    [InlineData(100.0, 100.5, 0.1)]
    [InlineData(-20.0, -19.8, 0.1)]
    [InlineData(0.0001, 0.0003, 0.00005)]
    public static void NotGreaterThanOrApproximately_Generic(double value, double other, double tolerance)
    {
        var act = () => value.MustBeGreaterThanOrApproximately<double>(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be greater than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void CustomExceptionWithTolerance_Generic() =>
        Test.CustomException(
            5.0,
            5.5,
            0.1,
            (x, y, t, exceptionFactory) => x.MustBeGreaterThanOrApproximately<double>(y, t, exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown_Generic() =>
        5.2.MustBeGreaterThanOrApproximately<double>(5.0, 0.1, (_, _, _) => null).Should().Be(5.2);
    
    [Fact]
    public static void CustomMessage_Generic() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0.MustBeGreaterThanOrApproximately<double>(101.0, 0.5, message: message)
        );

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Generic()
    {
        const double seventyEightO1 = 78.1;

        var act = () => seventyEightO1.MustBeGreaterThanOrApproximately<double>(79.0, 0.5);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }
#endif
}
