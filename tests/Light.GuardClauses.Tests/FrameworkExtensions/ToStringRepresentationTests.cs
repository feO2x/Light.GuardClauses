using System;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

public static class ToStringRepresentationTests
{
    [Theory]
    [InlineData(42)]
    [InlineData(30)]
    [InlineData(-1502)]
    public static void UnquotedValue(int value) => value.ToStringRepresentation().Should().Be(value.ToString());

    [Theory]
    [DefaultVariablesData]
    public static void QuotedValues(string value) => value.ToStringRepresentation().Should().Be($"\"{value}\"");

    [Fact]
    public static void EveryPrimitiveTypeConfiguredAsUnquotedIsNotQuoted()
    {
        1L.ToStringRepresentation().Should().Be("1");
        ((short) 2).ToStringRepresentation().Should().Be("2");
        ((sbyte) 3).ToStringRepresentation().Should().Be("3");
        4u.ToStringRepresentation().Should().Be("4");
        5ul.ToStringRepresentation().Should().Be("5");
        ((ushort) 6).ToStringRepresentation().Should().Be("6");
        ((byte) 7).ToStringRepresentation().Should().Be("7");
        true.ToStringRepresentation().Should().Be(bool.TrueString);
        8d.ToStringRepresentation().Should().Be("8");
        9m.ToStringRepresentation().Should().Be("9");
        10f.ToStringRepresentation().Should().Be("10");
    }

    [Fact]
    public static void EmptyAndLongRepresentationsAreHandled()
    {
        new EmptyRepresentation().ToStringRepresentation().Should().BeEmpty();

        var longText = new string('a', 127);
        longText.ToStringRepresentation().Should().Be($"\"{longText}\"");
    }

    [Fact]
    public static void NullValuesAreHandled()
    {
        string value = null;

        value.ToStringOrNull("missing").Should().Be("missing");
        ((Action) (() => value.ToStringRepresentation())).Should().Throw<ArgumentNullException>()
                                                         .WithParameterName(nameof(value));
    }

    private sealed class EmptyRepresentation
    {
        public override string ToString() => string.Empty;
    }
}
