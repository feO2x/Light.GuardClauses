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

    [Fact]
    public static void EmptyMemory()
    {
        var memory = new Memory<char>([]);
        memory.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void WhiteSpaceMemory()
    {
        var memory = new Memory<char>(" \t\n".ToCharArray());
        memory.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void NonWhiteSpaceMemory()
    {
        var memory = new Memory<char>("abc".ToCharArray());
        memory.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void MixedMemory()
    {
        var memory = new Memory<char>(" a\t".ToCharArray());
        memory.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void EmptyReadOnlyMemory()
    {
        var readOnlyMemory = new ReadOnlyMemory<char>([]);
        readOnlyMemory.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void WhiteSpaceReadOnlyMemory()
    {
        var readOnlyMemory = new ReadOnlyMemory<char>(" \t\n".ToCharArray());
        readOnlyMemory.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Fact]
    public static void NonWhiteSpaceReadOnlyMemory()
    {
        var readOnlyMemory = new ReadOnlyMemory<char>("abc".ToCharArray());
        readOnlyMemory.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Fact]
    public static void MixedReadOnlyMemory()
    {
        var readOnlyMemory = new ReadOnlyMemory<char>(" a\t".ToCharArray());
        readOnlyMemory.IsEmptyOrWhiteSpace().Should().BeFalse();
    }
}
