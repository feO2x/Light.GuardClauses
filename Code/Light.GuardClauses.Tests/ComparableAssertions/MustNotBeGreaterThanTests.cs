using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustNotBeGreaterThanTests
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(1, -1)]
    [InlineData(-88, -100)]
    public static void ParameterAboveBoundary(int value, int boundary)
    {
        Action act = () => value.MustNotBeGreaterThan(boundary, nameof(value));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain($"{nameof(value)} must not be greater than {boundary}, but it actually is {value}.");
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 0)]
    [InlineData(-88, 0)]
    public static void ParameterAtOrBelowBoundary(short value, short boundary) => value.MustNotBeGreaterThan(boundary, nameof(value)).Should().Be(value);

    [Fact]
    public static void CustomException() =>
        Test.CustomException(15, 10, (x, y, exceptionFactory) => x.MustNotBeGreaterThan(y, exceptionFactory));

    [Fact]
    public static void CustomExceptionParameterNull() =>
        Test.CustomException((string) null,
                             "Foo",
                             (x, y, exceptionFactory) => x.MustNotBeGreaterThan(y, exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown() => 5m.MustNotBeGreaterThan(5.1m, (_, _) => null).Should().Be(5m);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 21.MustNotBeGreaterThan(20, message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeGreaterThan("Bar", message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var five = 5;

        Action act = () => five.MustNotBeGreaterThan(2);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be(nameof(five));
    }
}