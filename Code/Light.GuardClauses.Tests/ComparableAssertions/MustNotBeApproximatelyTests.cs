using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustNotBeApproximatelyTests
{
    [Theory]
    [InlineData(5.3, 5.0, 0.2)]
    [InlineData(10.4, 10.3, 0.01)]
    [InlineData(3.15, 3.14, 0.001)]
    [InlineData(-42.002, -42.0001, 0.001)]
    public static void ValuesNotApproximatelyEqual_Double(double value, double other, double tolerance) =>
        value.MustNotBeApproximately(other, tolerance).Should().Be(value);

    [Theory]
    [InlineData(5.3f, 5.0f, 0.2f)]
    [InlineData(10.4f, 10.3f, 0.01f)]
    [InlineData(3.15f, 3.14f, 0.001f)]
    [InlineData(-42.002f, -42.0001f, 0.001f)]
    public static void ValuesNotApproximatelyEqual_Float(float value, float other, float tolerance) =>
        value.MustNotBeApproximately(other, tolerance).Should().Be(value);

    [Theory]
    [InlineData(5.0, 5.05, 0.1)]
    [InlineData(100.0, 99.95, 0.1)]
    [InlineData(-20.0, -20.05, 0.1)]
    [InlineData(0.0001, 0.00015, 0.0001)]
    public static void ValuesApproximatelyEqual_Double(double value, double other, double tolerance)
    {
        var act = () => value.MustNotBeApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must not be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Theory]
    [InlineData(5.0f, 5.05f, 0.1f)]
    [InlineData(100.0f, 99.95f, 0.1f)]
    [InlineData(-20.0f, -20.05f, 0.1f)]
    [InlineData(0.0001f, 0.00015f, 0.0001f)]
    public static void ValuesApproximatelyEqual_Float(float value, float other, float tolerance)
    {
        var act = () => value.MustNotBeApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must not be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void DefaultTolerance_Double()
    {
        // Should throw - difference is 0.00005 which is less than default tolerance 0.0001
        const double value = 1.00005;
        Action act = () => value.MustNotBeApproximately(1.0, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");

        // Should pass - difference is 0.0002 which is greater than default tolerance 0.0001
        const double value2 = 1.0002;
        value2.MustNotBeApproximately(1.0).Should().Be(value2);
    }

    [Fact]
    public static void DefaultTolerance_Float()
    {
        // Should throw - difference is 0.00005f which is less than default tolerance 0.0001f
        const float value = 1.00005f;
        Action act = () => value.MustNotBeApproximately(1.0f, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");

        // Should pass - difference is 0.0002f which is greater than default tolerance 0.0001f
        const float value2 = 1.0002f;
        value2.MustNotBeApproximately(1.0f).Should().Be(value2);
    }

    [Fact]
    public static void CustomException_Double() =>
        Test.CustomException(
            5.0,
            5.0000000001,
            (x, y, exceptionFactory) => x.MustNotBeApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Double() =>
        Test.CustomException(
            5.0,
            5.05,
            0.1,
            (x, y, z, exceptionFactory) => x.MustNotBeApproximately(y, z, exceptionFactory)
        );

    [Fact]
    public static void CustomException_Float() =>
        Test.CustomException(
            5.0f,
            5.000001f,
            (x, y, exceptionFactory) => x.MustNotBeApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Float() =>
        Test.CustomException(
            5.0f,
            5.05f,
            0.1f,
            (x, y, z, exceptionFactory) => x.MustNotBeApproximately(y, z, exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown_Double() =>
        5.2.MustNotBeApproximately(5.0, 0.1, (_, _, _) => null).Should().Be(5.2);

    [Fact]
    public static void NoCustomExceptionThrown_Float() =>
        5.2f.MustNotBeApproximately(5.0f, 0.1f, (_, _, _) => null).Should().Be(5.2f);

    [Fact]
    public static void CustomMessage_Double() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0.MustNotBeApproximately(100.05, 0.1, message: message)
        );

    [Fact]
    public static void CustomMessage_Float() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0f.MustNotBeApproximately(100.05f, 0.1f, message: message)
        );

    [Fact]
    public static void CallerArgumentExpression_Double()
    {
        const double seventyEightO1 = 78.1;

        var act = () => seventyEightO1.MustNotBeApproximately(78.099999);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Double()
    {
        const double pi = 3.14159;

        var act = () => pi.MustNotBeApproximately(3.14, 0.01);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(pi));
    }

    [Fact]
    public static void CallerArgumentExpression_Float()
    {
        const float seventyEightO1 = 78.1f;

        var act = () => seventyEightO1.MustNotBeApproximately(78.100005f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Float()
    {
        const float pi = 3.14159f;

        var act = () => pi.MustNotBeApproximately(3.14f, 0.01f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(pi));
    }

#if NET8_0
    [Theory]
    [InlineData(5.3, 5.0, 0.2)]
    [InlineData(10.4, 10.3, 0.01)]
    [InlineData(3.15, 3.14, 0.001)]
    [InlineData(-42.002, -42.0001, 0.001)]
    public static void ValuesNotApproximatelyEqual_Generic(double value, double other, double tolerance) =>
        value.MustNotBeApproximately<double>(other, tolerance).Should().Be(value);

    [Theory]
    [InlineData(5.0, 5.05, 0.1)]
    [InlineData(100.0, 99.95, 0.1)]
    [InlineData(-20.0, -20.05, 0.1)]
    [InlineData(0.0001, 0.00015, 0.0001)]
    public static void ValuesApproximatelyEqual_Generic(double value, double other, double tolerance)
    {
        var act = () => value.MustNotBeApproximately<double>(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must not be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void CustomExceptionWithTolerance_Generic() =>
        Test.CustomException(
            5.0,
            5.05,
            0.1,
            (x, y, t, exceptionFactory) => x.MustNotBeApproximately<double>(y, t, exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown_Generic() =>
        5.2.MustNotBeApproximately<double>(5.0, 0.1, (_, _, _) => null).Should().Be(5.2);
    
    [Fact]
    public static void CustomMessage_Generic() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0.MustNotBeApproximately<double>(100.05, 0.1, message: message)
        );

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Generic()
    {
        const double seventyEightO1 = 78.1;

        var act = () => seventyEightO1.MustNotBeApproximately<double>(78.0, 0.2);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }
#endif
}