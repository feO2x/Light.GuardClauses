using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustBeTrimmedTests
{
    [Theory]
    [InlineData("")]
    [InlineData("foo")]
    [InlineData("bar")]
    [InlineData("Another string\twith whitespace")]
    public static void StringIsTrimmed(string input) =>
        input.MustBeTrimmed().Should().BeSameAs(input);

    [Fact]
    public static void StringIsNull()
    {
        var nullString = default(string);

        // ReSharper disable once ExpressionIsAlwaysNull
        var act = () => nullString.MustBeTrimmed(nameof(nullString));

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(nullString));
    }

    [Theory]
    [InlineData("123 ")]
    [InlineData("\nFoo")]
    [InlineData(" not trimmed ")]
    public static void StringIsNotTrimmed(string invalidString)
    {
        var act = () => invalidString.MustBeTrimmed(nameof(invalidString));

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain($"invalidString must be trimmed, but it actually is {invalidString.ToStringOrNull()}");
    }

    [Fact]
    public static void CustomExceptionStringNull()
    {
        Test.CustomException(
            default(string),
            (@null, exceptionFactory) => @null.MustBeTrimmed(exceptionFactory)
        );
    }

    [Theory]
    [InlineData(" This is not trimmed at the start")]
    [InlineData("And not at the end\t ")]
    public static void CustomExceptionInvalidString(string invalidString)
    {
        Test.CustomException(
            invalidString,
            (@string, exceptionFactory) => @string.MustBeTrimmed(exceptionFactory)
        );
    }

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string invalidString = " Not trimmed ";

        var act = () => invalidString.MustBeTrimmed();

        act.Should().Throw<StringException>()
           .And.ParamName.Should().Be(nameof(invalidString));
    }
}