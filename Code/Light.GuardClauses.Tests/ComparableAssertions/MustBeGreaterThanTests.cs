using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBeGreaterThanTests
{
    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(-1, -1)]
    [InlineData(0, 0)]
    public static void ParameterEqualOrLess(int first, int second)
    {
        Action act = () => first.MustBeGreaterThan(second, nameof(first));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain($"{nameof(first)} must be greater than {second}, but it actually is {first}.");
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(first));
    }

    [Theory]
    [InlineData(133, 99)]
    [InlineData(int.MaxValue, int.MaxValue - 1)]
    [InlineData(byte.MaxValue, byte.MinValue)]
    public static void ParameterGreater(int first, int second) => first.MustBeGreaterThan(second).Should().Be(first);

    [Fact]
    public static void CustomException() =>
        Test.CustomException(40, 50, (x, y, exceptionFactory) => x.MustBeGreaterThan(y, exceptionFactory));

    [Fact]
    public static void CustomExceptionParameterNull() =>
        Test.CustomException((string) null,
                             "Foo",
                             (x, y, exceptionFactory) => x.MustBeGreaterThan(y, exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown() => 5.6m.MustBeGreaterThan(5.1m, (_, _) => null).Should().Be(5.6m);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 100.MustBeGreaterThan(100, message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeGreaterThan("Bar", message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var fifteen = 15;

        Action act = () => fifteen.MustBeGreaterThan(20);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be(nameof(fifteen));
    }
}