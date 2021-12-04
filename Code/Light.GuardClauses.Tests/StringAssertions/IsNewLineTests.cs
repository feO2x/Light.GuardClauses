using FluentAssertions;
using Xunit;

#nullable enable

namespace Light.GuardClauses.Tests.StringAssertions;

public static class IsNewLineTests
{
    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public static void IsNewLine(string @string) => @string.IsNewLine().Should().BeTrue();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Some other string")]
    [InlineData("You can't play God without being acquainted with the devil.")]
    public static void IsNotNewLine(string? @string) => @string.IsNewLine().Should().BeFalse();
}