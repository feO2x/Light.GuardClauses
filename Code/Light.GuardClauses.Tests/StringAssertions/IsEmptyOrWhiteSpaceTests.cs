using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class IsEmptyOrWhiteSpaceTests
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
        var span = new Span<char>(" \t\n".ToCharArray());
        span.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void NonWhiteSpaceSpan()
    {
        var span = new Span<char>("abc".ToCharArray());
        span.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void MixedSpan()
    {
        var span = new Span<char>(" a\t".ToCharArray());
        span.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void EmptyReadOnlySpan()
    {
        var readOnlySpan = string.Empty.AsSpan();
        readOnlySpan.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void WhiteSpaceReadOnlySpan()
    {
        var readOnlySpan = " \t\n".AsSpan();
        readOnlySpan.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void NonWhiteSpaceReadOnlySpan()
    {
        var readOnlySpan = "abc".AsSpan();
        readOnlySpan.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void MixedReadOnlySpan()
    {
        var readOnlySpan = " a\t".AsSpan();
        readOnlySpan.IsEmptyOrWhiteSpace().Should().BeFalse();
    }
}
