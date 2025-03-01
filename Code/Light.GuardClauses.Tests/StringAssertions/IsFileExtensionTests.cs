using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class IsFileExtensionTests
{
    [Fact]
    public static void Null() =>
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse -- we need to test this
        ((string) null).IsFileExtension().Should().BeFalse();

    [Fact]
    public static void Empty() =>
        string.Empty.IsFileExtension().Should().BeFalse();

    [Theory]
    [InlineData(".txt")]
    [InlineData(".tar.gz")]
    [InlineData(".docx")]
    [InlineData(".config")]
    public static void ValidFileExtensions(string extension) =>
        extension.IsFileExtension().Should().BeTrue();

    [Theory]
    [InlineData("txt")] // No leading period
    [InlineData(".")] // Just a period
    [InlineData(".txt!")] // Invalid character
    [InlineData(".txt ")] // Contains space
    [InlineData(".doc/")] // Invalid character
    [InlineData("..")] // Just periods
    [InlineData("...")]
    [InlineData("....")]
    [InlineData(".txt.")] // Invalid - ends with period
    [InlineData(".docx.")]
    public static void InvalidFileExtensions(string extension) =>
        extension.IsFileExtension().Should().BeFalse();
}
