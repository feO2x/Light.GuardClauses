using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class CheckIsEmptyOrWhiteSpaceTests
{
    [Fact]
    public static void EmptySpan()
    {
        var span = new Span<char>([]);
        span.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void WhiteSpaceSpan()
    {
        var span = new Span<char>([' ', '\t', '\n']);
        span.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void NonWhiteSpaceSpan()
    {
        var span = new Span<char>(['a', 'b', 'c']);
        span.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void MixedSpan()
    {
        var span = new Span<char>([' ', 'a', '\t']);
        span.IsEmptyOrWhiteSpace().Should().BeFalse();
    }
}
