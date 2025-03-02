using System;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustBeFileExtensionTests
{
    public static readonly TheoryData<string> InvalidFileExtensionsData =
    [
        "txt",
        ".jpg/",
        ".",
        "..",
        "...",
        "....",
        ".docx.",
    ];

    [Theory]
    [InlineData(".txt")]
    [InlineData(".jpg")]
    [InlineData(".tar.gz")]
    public static void ValidFileExtensions(string input) =>
        input.MustBeFileExtension().Should().BeSameAs(input);

    [Fact]
    public static void StringIsNull()
    {
        var nullString = default(string);

        // ReSharper disable once ExpressionIsAlwaysNull
        var act = () => nullString.MustBeFileExtension(nameof(nullString));

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(nullString));
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void InvalidFileExtensions(string invalidString)
    {
        var act = () => invalidString.MustBeFileExtension(nameof(invalidString));

        act.Should().Throw<ArgumentException>()
           .And.Message.Should().Contain(
                $"invalidString must be a valid file extension, but it actually is {invalidString.ToStringOrNull()}"
            );
    }

    [Fact]
    public static void CustomExceptionStringNull() =>
        Test.CustomException(
            default(string),
            (@null, exceptionFactory) => @null.MustBeFileExtension(exceptionFactory)
        );

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void CustomExceptionInvalidFileExtensions(string invalidString) =>
        Test.CustomException(
            invalidString,
            (@string, exceptionFactory) => @string.MustBeFileExtension(exceptionFactory)
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string invalidString = "txt";

        var act = () => invalidString.MustBeFileExtension();

        act.Should().Throw<ArgumentException>()
           .WithParameterName(nameof(invalidString));
    }
}
