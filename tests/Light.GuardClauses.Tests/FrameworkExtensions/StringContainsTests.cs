using System;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

// The extension method is called explicitly via StringExtensions because modern TFMs
// provide string.Contains(string, StringComparison) as an instance method that would
// otherwise always win overload resolution.
public static class StringContainsTests
{
    [Theory]
    [InlineData("Hello, World", "World", StringComparison.Ordinal, true)]
    [InlineData("Hello, World", "world", StringComparison.Ordinal, false)]
    [InlineData("Hello, World", "world", StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("Hello, World", "Foo", StringComparison.OrdinalIgnoreCase, false)]
    public static void ContainsWithComparisonType(
        string @string,
        string value,
        StringComparison comparisonType,
        bool expected
    ) =>
        StringExtensions.Contains(@string, value, comparisonType).Should().Be(expected);

    [Fact]
    public static void StringNull()
    {
        var act = () => StringExtensions.Contains(null!, "foo", StringComparison.Ordinal);

        act.Should().Throw<ArgumentNullException>().WithParameterName("string");
    }

    [Fact]
    public static void ValueNull()
    {
        var act = () => StringExtensions.Contains("foo", null!, StringComparison.Ordinal);

        act.Should().Throw<ArgumentNullException>().WithParameterName("value");
    }
}
