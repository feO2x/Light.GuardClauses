using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DateTimeAssertions;

public static class MustBeUnspecifiedTests
{
    [Fact]
    public static void NotUnspecified()
    {
        var invalidDateTime = DateTime.UtcNow;

        Action act = () => invalidDateTime.MustBeUnspecified(nameof(invalidDateTime));

        act.Should().Throw<InvalidDateTimeException>()
           .And.Message.Should().Contain($"{nameof(invalidDateTime)} must use kind \"{DateTimeKind.Unspecified}\", but it actually uses \"{invalidDateTime.Kind}\" and is \"{invalidDateTime:O}\".");
    }

    [Fact]
    public static void Unspecified()
    {
        var dateTime = DateTime.MinValue;

        var result = dateTime.MustBeUnspecified();

        result.Should().Be(dateTime);
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException(new DateTime(2018, 2, 18, 16, 57, 00, DateTimeKind.Utc),
                             (value, exceptionFactory) => value.MustBeUnspecified(exceptionFactory));

    [Fact]
    public static void NoCustomException()
    {
        var value = new DateTime(2000, 9, 10, 15, 32, 47, DateTimeKind.Unspecified);

        var result = value.MustBeUnspecified(_ => null);

        result.Should().Be(value);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeUnspecified(message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var invalidDateTime = new DateTime(2021, 11, 17, 20, 30, 0, DateTimeKind.Local);

        Action act = () => invalidDateTime.MustBeUnspecified();

        act.Should().Throw<InvalidDateTimeException>()
           .And.ParamName.Should().Be(nameof(invalidDateTime));
    }
}