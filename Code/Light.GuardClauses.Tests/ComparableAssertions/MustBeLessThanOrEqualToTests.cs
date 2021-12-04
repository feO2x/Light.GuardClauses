using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBeLessThanOrEqualToTests
{
    [Theory]
    [InlineData(10, 9)]
    [InlineData(-42, -1888)]
    public static void ParameterGreater(int first, int second)
    {
        Action act = () => first.MustBeLessThanOrEqualTo(second, nameof(first));

        var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        exceptionAssertion.Message.Should().Contain($"{nameof(first)} must be less than or equal to {second}, but it actually is {first}.");
        exceptionAssertion.ParamName.Should().BeSameAs(nameof(first));
    }

    [Theory]
    [InlineData(13, 13)]
    [InlineData(12, 13)]
    public static void ParameterLessOrEqual(int first, int second) => first.MustBeLessThanOrEqualTo(second).Should().Be(first);

    [Fact]
    public static void CustomException() =>
        Test.CustomException(20, 19, (x, y, exceptionFactory) => x.MustBeLessThanOrEqualTo(y, exceptionFactory));

    [Fact]
    public static void CustomExceptionParameterNull() =>
        Test.CustomException((string) null,
                             "Foo",
                             (x, y, exceptionFactory) => x.MustBeLessThanOrEqualTo(y, exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown() => 5m.MustBeLessThanOrEqualTo(5.1m, (_, _) => null).Should().Be(5m);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 'c'.MustBeLessThanOrEqualTo('a', message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeLessThanOrEqualTo("Bar", message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var six = 6;

        Action act = () => six.MustBeLessThanOrEqualTo(5);

        act.Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be(nameof(six));
    }
}