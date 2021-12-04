using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

#nullable enable

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustBeNewLineTests
{
    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public static void StringIsNewLine(string @string) =>
        @string.MustBeNewLine().Should().BeSameAs(@string);

    [Fact]
    public static void StringIsNull()
    {
        var stringValue = default(string);

        var act = () => stringValue.MustBeNewLine(nameof(stringValue));

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(stringValue));
    }

    [Theory]
    [InlineData("Foo")]
    [InlineData("")]
    public static void StringIsNotNewLine(string invalidString)
    {
        var act = () => invalidString.MustBeNewLine(nameof(invalidString));

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain($"{nameof(invalidString)} must be either \"\\n\" or \"\\r\\n\", but it actually is {invalidString.ToStringOrNull()}.");
    }

    [Fact]
    public static void CustomExceptionStringNull()
    {
        Test.CustomException(
            default(string),
            (@null, exceptionFactory) => @null.MustBeNewLine(exceptionFactory)
        );
    }

    [Theory]
    [InlineData("What humans describe as sane is a narrow range of behaviors.")]
    [InlineData("Someday sounds a lot like the thing people say when they actually mean never.")]
    public static void CustomExceptionInvalidString(string invalidString)
    {
        Test.CustomException(
            invalidString,
            (@string, exceptionFactory) => @string.MustBeNewLine(exceptionFactory!)
        );
    }

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string expression = "This is not a new line";

        var act = () => expression.MustBeNewLine();

        act.Should().Throw<StringException>()
           .And.ParamName.Should().Be(nameof(expression));
    }
}