using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
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

    public static readonly TheoryData<string> ValidFileExtensionsData =
    [
        ".txt",
        ".jpg",
        ".tar.gz",
    ];

    [Theory]
    [MemberData(nameof(ValidFileExtensionsData))]
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

    [Theory]
    [MemberData(nameof(ValidFileExtensionsData))]
    public static void ValidFileExtensions_ReadOnlySpan(string input)
    {
        var span = input.AsSpan();

        var result = span.MustBeFileExtension();

        result.Equals(span, StringComparison.Ordinal).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void InvalidFileExtensions_ReadOnlySpan(string invalidString)
    {
        var act = () =>
        {
            var span = new ReadOnlySpan<char>(invalidString?.ToCharArray() ?? []);
            span.MustBeFileExtension("parameterName");
        };

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain(
                $"parameterName must be a valid file extension, but it actually is {invalidString}"
            );
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void CustomExceptionInvalidFileExtensions_ReadOnlySpan(string invalidString)
    {
        var exception = new Exception();

        var act = () =>
        {
            var span = new ReadOnlySpan<char>(invalidString?.ToCharArray() ?? []);
            span.MustBeFileExtension(_ => exception);
        };

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void CallerArgumentExpression_ReadOnlySpan()
    {
        var act = () =>
        {
            var invalidSpan = "txt".AsSpan();
            invalidSpan.MustBeFileExtension();
        };

        act.Should().Throw<StringException>()
           .WithParameterName("invalidSpan");
    }

    [Theory]
    [MemberData(nameof(ValidFileExtensionsData))]
    public static void ValidFileExtensions_Span(string input)
    {
        var span = input.AsSpan().ToArray().AsSpan();

        var result = span.MustBeFileExtension();

        result.ToString().Should().Be(input);
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void InvalidFileExtensions_Span(string invalidString)
    {
        var act = () =>
        {
            var span = new Span<char>(invalidString?.ToCharArray() ?? []);
            span.MustBeFileExtension("parameterName");
        };

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain(
                $"parameterName must be a valid file extension, but it actually is {invalidString}"
            );
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void CustomExceptionInvalidFileExtensions_Span(string invalidString)
    {
        var exception = new Exception();

        var act = () =>
        {
            var span = new Span<char>(invalidString?.ToCharArray() ?? []);
            span.MustBeFileExtension(_ => exception);
        };

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void CallerArgumentExpression_Span()
    {
        var act = () =>
        {
            var charArray = "txt".ToCharArray();
            var invalidSpan = new Span<char>(charArray);
            invalidSpan.MustBeFileExtension();
        };

        act.Should().Throw<StringException>()
           .WithParameterName("invalidSpan");
    }

    [Theory]
    [MemberData(nameof(ValidFileExtensionsData))]
    public static void ValidFileExtensions_Memory(string input)
    {
        var memory = input.ToCharArray().AsMemory();

        var result = memory.MustBeFileExtension();

        result.ToString().Should().Be(input);
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void InvalidFileExtensions_Memory(string invalidString)
    {
        var act = () =>
        {
            var memory = new Memory<char>(invalidString?.ToCharArray() ?? []);
            memory.MustBeFileExtension("parameterName");
        };

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain(
                $"parameterName must be a valid file extension, but it actually is {invalidString}"
            );
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void CustomExceptionInvalidFileExtensions_Memory(string invalidString)
    {
        var exception = new Exception();

        var act = () =>
        {
            var memory = new Memory<char>(invalidString?.ToCharArray() ?? []);
            memory.MustBeFileExtension(_ => exception);
        };

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void CallerArgumentExpression_Memory()
    {
        var act = () =>
        {
            var charArray = "txt".ToCharArray();
            var invalidMemory = new Memory<char>(charArray);
            invalidMemory.MustBeFileExtension();
        };

        act.Should().Throw<StringException>()
           .WithParameterName("invalidMemory");
    }

    [Theory]
    [MemberData(nameof(ValidFileExtensionsData))]
    public static void ValidFileExtensions_ReadOnlyMemory(string input)
    {
        var memory = input.AsMemory();

        var result = memory.MustBeFileExtension();

        result.Equals(memory).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void InvalidFileExtensions_ReadOnlyMemory(string invalidString)
    {
        var act = () =>
        {
            var memory = new ReadOnlyMemory<char>(invalidString?.ToCharArray() ?? []);
            memory.MustBeFileExtension("parameterName");
        };

        act.Should().Throw<StringException>()
           .And.Message.Should().Contain(
                $"parameterName must be a valid file extension, but it actually is {invalidString}"
            );
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionsData))]
    public static void CustomExceptionInvalidFileExtensions_ReadOnlyMemory(string invalidString)
    {
        var exception = new Exception();

        var act = () =>
        {
            var memory = new ReadOnlyMemory<char>(invalidString?.ToCharArray() ?? []);
            memory.MustBeFileExtension(_ => exception);
        };

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void CallerArgumentExpression_ReadOnlyMemory()
    {
        var act = void () =>
        {
            var invalidMemory = "txt".AsMemory();
            invalidMemory.MustBeFileExtension();
        };

        act.Should().Throw<StringException>()
           .WithParameterName("invalidMemory");
    }
}
