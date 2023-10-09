using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustBeTrimmedAtEndTests
{
    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData(" bar")]
    [InlineData("Sto p")]
    public static void StringIsTrimmedAtEnd(string input) =>
        input.MustBeTrimmedAtEnd().Should().BeSameAs(input);

    [Fact]
    public static void StringIsNull()
    {
        var nullString = default(string);

        // ReSharper disable once ExpressionIsAlwaysNull
        var act = () => nullString.MustBeTrimmedAtEnd(nameof(nullString));

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(nullString));
    }

    [Theory]
    [InlineData("abc ")]
    [InlineData(" Let's go\n")]
    [InlineData("not trimmed at the end\t")]
    public static void StringIsNotTrimmedAtTheEnd(string invalidString)
    {
        var act = () => invalidString.MustBeTrimmedAtEnd(nameof(invalidString));

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain($"invalidString must be trimmed at the end, but it actually is {invalidString.ToStringOrNull()}");
    }

    [Fact]
    public static void CustomExceptionStringNull() =>
        Test.CustomException(
            default(string),
            (@null, exceptionFactory) => @null.MustBeTrimmedAtEnd(exceptionFactory)
        );

    [Theory]
    [InlineData("Not trimmed at end ")]
    [InlineData("Meh\t")]
    public static void CustomExceptionInvalidString(string invalidString) =>
        Test.CustomException(
            invalidString,
            (@string, exceptionFactory) => @string.MustBeTrimmedAtEnd(exceptionFactory)
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string invalidString = "not trimmed ";

        var act = () => invalidString.MustBeTrimmedAtEnd();

        act.Should().Throw<StringException>()
           .WithParameterName(nameof(invalidString));
    }
}