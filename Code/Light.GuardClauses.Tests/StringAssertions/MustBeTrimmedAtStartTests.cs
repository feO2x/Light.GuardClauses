using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustBeTrimmedAtStartTests
{
    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData("bar\n")]
    [InlineData("I have white space ")]
    public static void StringIsTrimmedAtStart(string input) =>
        input.MustBeTrimmedAtStart().Should().BeSameAs(input);

    [Fact]
    public static void StringIsNulL()
    {
        var nullString = default(string);

        // ReSharper disable once ExpressionIsAlwaysNull
        var act = () => nullString.MustBeTrimmedAtStart(nameof(nullString));

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(nullString));
    }

    [Theory]
    [InlineData(" Hey")]
    [InlineData("\tTab")]
    [InlineData(" White space on both ends ")]
    public static void StringIsNotTrimmedAtStart(string invalidString)
    {
        var act = () => invalidString.MustBeTrimmedAtStart(nameof(invalidString));

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain($"invalidString must be trimmed at the start, but it actually is {invalidString.ToStringOrNull()}");
    }

    [Fact]
    public static void CustomExceptionStringNull() =>
        Test.CustomException(
            default(string),
            (@null, exceptionFactory) => @null.MustBeTrimmedAtStart(exceptionFactory)
        );

    [Theory]
    [InlineData(" Not trimmed at the start")]
    [InlineData("\nFoo")]
    public static void CustomExceptionInvalidString(string invalidString) =>
        Test.CustomException(
            invalidString,
            (@string, exceptionFactory) => @string.MustBeTrimmedAtStart(exceptionFactory)
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string invalidString = " Not trimmed at start";

        var act = () => invalidString.MustBeTrimmedAtStart();

        act.Should().Throw<StringException>()
           .And.ParamName.Should().Be(nameof(invalidString));
    }
}