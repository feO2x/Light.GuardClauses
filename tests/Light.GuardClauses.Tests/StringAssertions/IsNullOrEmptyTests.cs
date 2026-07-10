using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class IsNullOrEmptyTests
{
    [Fact]
    public static void StringNull() => ((string) null).IsNullOrEmpty().Should().BeTrue();

    [Fact]
    public static void StringEmpty() => string.Empty.IsNullOrEmpty().Should().BeTrue();

    [Theory]
    [InlineData("abcdef")]
    [InlineData("\t")]
    [InlineData("  ")]
    [InlineData("\u2028")]
    public static void StringNotEmpty(string @string) => @string.IsNullOrEmpty().Should().BeFalse();
}