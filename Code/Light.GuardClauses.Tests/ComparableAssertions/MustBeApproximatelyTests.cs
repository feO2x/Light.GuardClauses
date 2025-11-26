using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBeApproximatelyTests
{
    [Theory]
    [InlineData(5.1, 5.0, 0.2)]
    [InlineData(10.3, 10.3, 0.01)]
    [InlineData(3.14159, 3.14, 0.002)]
    [InlineData(-42.0, -42.0001, 0.001)]
    public static void ValuesApproximatelyEqual_Double(double value, double other, double tolerance) =>
        value.MustBeApproximately(other, tolerance).Should().Be(value);

    [Theory]
    [InlineData(5.1f, 5.0f, 0.2f)]
    [InlineData(10.3f, 10.3f, 0.01f)]
    [InlineData(3.14159f, 3.14f, 0.002f)]
    [InlineData(-42.0f, -42.0001f, 0.001f)]
    public static void ValuesApproximatelyEqual_Float(float value, float other, float tolerance) =>
        value.MustBeApproximately(other, tolerance).Should().Be(value);

    [Theory]
    [InlineData(5.0, 5.3, 0.1)]
    [InlineData(100.0, 99.8, 0.1)]
    [InlineData(-20.0, -20.2, 0.1)]
    [InlineData(0.0001, 0.0002, 0.00005)]
    public static void ValuesNotApproximatelyEqual_Double(double value, double other, double tolerance)
    {
        var act = () => value.MustBeApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Theory]
    [InlineData(5.0f, 5.3f, 0.1f)]
    [InlineData(100.0f, 99.8f, 0.1f)]
    [InlineData(-20.0f, -20.2f, 0.1f)]
    [InlineData(0.0001f, 0.0002f, 0.00005f)]
    public static void ValuesNotApproximatelyEqual_Float(float value, float other, float tolerance)
    {
        var act = () => value.MustBeApproximately(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void DefaultTolerance_Double()
    {
        // Should pass - difference is 0.00005 which is less than default tolerance 0.0001
        const double value = 1.00005;
        value.MustBeApproximately(1.0).Should().Be(value);

        // Should throw - difference is 0.0002 which is greater than default tolerance 0.0001
        Action act = () => 1.0002.MustBeApproximately(1.0, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");
    }

    [Fact]
    public static void DefaultTolerance_Float()
    {
        // Should pass - difference is 0.00005f which is less than default tolerance 0.0001f
        const float value = 1.00005f;
        value.MustBeApproximately(1.0f).Should().Be(value);

        // Should throw - difference is 0.0002f which is greater than default tolerance 0.0001f
        Action act = () => 1.0002f.MustBeApproximately(1.0f, "parameter");
        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName("parameter");
    }

    [Fact]
    public static void CustomException_Double() =>
        Test.CustomException(
            5.0,
            5.3,
            (x, y, exceptionFactory) => x.MustBeApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Double() =>
        Test.CustomException(
            5.0,
            5.5,
            0.1,
            (x, y, z, exceptionFactory) => x.MustBeApproximately(y, z, exceptionFactory)
        );

    [Fact]
    public static void CustomException_Float() =>
        Test.CustomException(
            5.0f,
            5.3f,
            (x, y, exceptionFactory) => x.MustBeApproximately(y, exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionWithTolerance_Float() =>
        Test.CustomException(
            5.0f,
            5.5f,
            0.1f,
            (x, y, z, exceptionFactory) => x.MustBeApproximately(y, z, exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown_Double() =>
        5.0.MustBeApproximately(5.05, 0.1, (_, _, _) => null).Should().Be(5.0);

    [Fact]
    public static void NoCustomExceptionThrown_Float() =>
        5.0f.MustBeApproximately(5.05f, 0.1f, (_, _, _) => null).Should().Be(5.0f);

    [Fact]
    public static void CustomMessage_Double() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0.MustBeApproximately(101.0, 0.5, message: message)
        );

    [Fact]
    public static void CustomMessage_Float() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0f.MustBeApproximately(101.0f, 0.5f, message: message)
        );

    [Fact]
    public static void CallerArgumentExpression_Double()
    {
        const double seventyEightO1 = 78.1;

        var act = () => seventyEightO1.MustBeApproximately(3.0);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Double()
    {
        const double pi = 3.14159;

        var act = () => pi.MustBeApproximately(3.0, 0.1);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(pi));
    }

    [Fact]
    public static void CallerArgumentExpression_Float()
    {
        const float seventyEightO1 = 78.1f;

        var act = () => seventyEightO1.MustBeApproximately(3.0f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Float()
    {
        const float pi = 3.14159f;

        var act = () => pi.MustBeApproximately(3.0f, 0.1f);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(pi));
    }

#if NET8_0_OR_GREATER
    [Theory]
    [InlineData(5.1, 5.0, 0.2)]
    [InlineData(10.3, 10.3, 0.01)]
    [InlineData(3.14159, 3.14, 0.002)]
    [InlineData(-42.0, -42.0001, 0.001)]
    public static void ValuesApproximatelyEqual_Generic(double value, double other, double tolerance) =>
        value.MustBeApproximately<double>(other, tolerance).Should().Be(value);

    [Theory]
    [InlineData(5.0, 5.3, 0.1)]
    [InlineData(100.0, 99.8, 0.1)]
    [InlineData(-20.0, -20.2, 0.1)]
    [InlineData(0.0001, 0.0002, 0.00005)]
    public static void ValuesNotApproximatelyEqual_Generic(double value, double other, double tolerance)
    {
        var act = () => value.MustBeApproximately<double>(other, tolerance, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain(
            $"{nameof(value)} must be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {value}."
        );
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Fact]
    public static void CustomExceptionWithTolerance_Generic() =>
        Test.CustomException(
            5.0,
            5.3,
            0.2,
            (x, y, t, exceptionFactory) => x.MustBeApproximately<double>(y, t, exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown_Generic() =>
        5.0.MustBeApproximately<double>(5.05, 0.1, (_, _, _) => null).Should().Be(5.0);
    
    [Fact]
    public static void CustomMessage_Generic() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => 100.0.MustBeApproximately<double>(101.0, 0.5, message: message)
        );

    [Fact]
    public static void CallerArgumentExpressionWithTolerance_Generic()
    {
        const double seventyEightO1 = 78.1;

        var act = () => seventyEightO1.MustBeApproximately<double>(3.0, 10.0);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(seventyEightO1));
    }
#endif
}
