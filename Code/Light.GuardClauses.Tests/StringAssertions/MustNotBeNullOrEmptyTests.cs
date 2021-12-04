using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustNotBeNullOrEmptyTests
{
    [Fact]
    public static void StringNull()
    {
        Action act = () => ((string) null).MustNotBeNullOrEmpty("Foo");

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("Foo");
    }

    [Fact]
    public static void EmptyString()
    {
        var @string = string.Empty;

        Action act = () => @string.MustNotBeNullOrEmpty(nameof(@string));

        act.Should().Throw<EmptyStringException>()
           .And.Message.Should().Contain($"{nameof(@string)} must not be an empty string, but it actually is.");
    }

    [Theory]
    [DefaultVariablesData]
    public static void StringNotEmpty(string @string) => @string.MustNotBeNullOrEmpty(nameof(@string)).Should().BeSameAs(@string);

    [Fact]
    public static void CustomExceptionStringNull() =>
        Test.CustomException((string) null,
                             (@null, exceptionFactory) => @null.MustNotBeNullOrEmpty(exceptionFactory));

    [Fact]
    public static void CustomExceptionStringEmpty() =>
        Test.CustomException(string.Empty,
                             (emptyString, exceptionFactory) => emptyString.MustNotBeNullOrEmpty(exceptionFactory));

    [Fact]
    public static void CustomMessageStringNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeNullOrEmpty(message: message));

    [Fact]
    public static void CustomMessageStringEmpty() =>
        Test.CustomMessage<EmptyStringException>(message => string.Empty.MustNotBeNullOrEmpty(message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var nullString = default(string);

        // ReSharper disable once ExpressionIsAlwaysNull
        var act = () => nullString.MustNotBeNullOrEmpty();

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(nullString));
    }
}