using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

public static class TextExtensionsTests
{
    [Fact]
    public static void NestedExceptionMessagesAreAppendedInOrder()
    {
        var exception = new InvalidOperationException("outer", new ArgumentException("inner"));

        var result = new StringBuilder().AppendExceptionMessages(exception);

        result.ToString().Should().Be($"outer{Environment.NewLine}{Environment.NewLine}inner{Environment.NewLine}");
        exception.GetAllExceptionMessages().Should().Be(result.ToString());
    }

    [Fact]
    public static void EmptyTextDoesNotEqualNonEmptyTextWhenIgnoringWhiteSpace() =>
        string.Empty.EqualsOrdinalIgnoreWhiteSpace("content").Should().BeFalse();
}
