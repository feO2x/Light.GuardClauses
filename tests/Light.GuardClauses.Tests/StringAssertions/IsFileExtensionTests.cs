using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class IsFileExtensionTests
{
    [Theory]
    [MemberData(nameof(ValidFileExtensionData))]
    public static void ValidFileExtensions_String(string extension) =>
        extension.IsFileExtension().Should().BeTrue();
        
    [Theory]
    [MemberData(nameof(ValidFileExtensionData))]
    public static void ValidFileExtensions_Span(string extension)
    {
        var span = extension.ToCharArray().AsSpan();
        span.IsFileExtension().Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(ValidFileExtensionData))]
    public static void ValidFileExtensions_Memory(string extension)
    {
        var memory = extension.ToCharArray().AsMemory();
        memory.IsFileExtension().Should().BeTrue();
    }
    
    [Theory]
    [MemberData(nameof(ValidFileExtensionData))]
    public static void ValidFileExtensions_ReadOnlyMemory(string extension)
    {
        var readOnlyMemory = extension.AsMemory();
        readOnlyMemory.IsFileExtension().Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidFileExtensionData))]
    public static void InvalidFileExtensions_String(string extension) =>
        extension.IsFileExtension().Should().BeFalse();
        
    [Theory]
    [MemberData(nameof(InvalidFileExtensionData))]
    public static void InvalidFileExtensions_Span(string extension)
    {
        var span = extension?.ToCharArray() ?? Span<char>.Empty;
        span.IsFileExtension().Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(InvalidFileExtensionData))]
    public static void InvalidFileExtensions_Memory(string extension)
    {
        var memory = extension?.ToCharArray() ?? Memory<char>.Empty;
        memory.IsFileExtension().Should().BeFalse();
    }
    
    [Theory]
    [MemberData(nameof(InvalidFileExtensionData))]
    public static void InvalidFileExtensions_ReadOnlyMemory(string extension)
    {
        var readOnlyMemory = extension?.ToCharArray() ?? ReadOnlyMemory<char>.Empty;
        readOnlyMemory.IsFileExtension().Should().BeFalse();
    }
    
    public static TheoryData<string> ValidFileExtensionData() =>
    [
        ".txt",
        ".tar.gz",
        ".docx",
        ".config",
    ];
    
    public static TheoryData<string> InvalidFileExtensionData() =>
    [
        null,
        string.Empty,
        "txt", // No leading period
        ".", // Just a period
        ".txt!", // Invalid character
        ".txt ", // Contains space
        ".doc/", // Invalid character
        "..", // Just periods
        "...",
        "....",
        ".txt.", // Invalid - ends with period
        ".docx.",
    ];
}