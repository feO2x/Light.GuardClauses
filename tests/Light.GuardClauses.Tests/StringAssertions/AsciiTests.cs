using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class AsciiTests
{
    [Theory]
    [InlineData('\0', true)]
    [InlineData('\u007F', true)]
    [InlineData('\u0080', false)]
    [InlineData('é', false)]
    public static void CharactersAreClassified(char value, bool expected) => value.IsAscii().Should().Be(expected);

    [Theory]
    [InlineData((byte) 0, true)]
    [InlineData((byte) 127, true)]
    [InlineData((byte) 128, false)]
    [InlineData(byte.MaxValue, false)]
    public static void BytesAreClassified(byte value, bool expected) => value.IsAscii().Should().Be(expected);

    [Fact]
    public static void StringsUseExpectedNullEmptyAndContentSemantics()
    {
        ((string) null).IsAscii().Should().BeFalse();
        string.Empty.IsAscii().Should().BeTrue();
        "ASCII\0\u007F".IsAscii().Should().BeTrue();
        "ASCIIé".IsAscii().Should().BeFalse();
    }

    [Fact]
    public static void EveryBufferShapeUsesTheSameSemantics()
    {
        var validCharacters = "ASCII".ToCharArray();
        var invalidCharacters = "é".ToCharArray();
        var validBytes = new byte[] { 0, 65, 127 };
        var invalidBytes = new byte[] { 65, 128 };

        validCharacters.AsSpan().IsAscii().Should().BeTrue();
        ((ReadOnlySpan<char>) validCharacters).IsAscii().Should().BeTrue();
        validCharacters.AsMemory().IsAscii().Should().BeTrue();
        ((ReadOnlyMemory<char>) validCharacters).IsAscii().Should().BeTrue();
        invalidCharacters.AsSpan().IsAscii().Should().BeFalse();
        ((ReadOnlySpan<char>) invalidCharacters).IsAscii().Should().BeFalse();
        invalidCharacters.AsMemory().IsAscii().Should().BeFalse();
        ((ReadOnlyMemory<char>) invalidCharacters).IsAscii().Should().BeFalse();

        validBytes.AsSpan().IsAscii().Should().BeTrue();
        ((ReadOnlySpan<byte>) validBytes).IsAscii().Should().BeTrue();
        validBytes.AsMemory().IsAscii().Should().BeTrue();
        ((ReadOnlyMemory<byte>) validBytes).IsAscii().Should().BeTrue();
        invalidBytes.AsSpan().IsAscii().Should().BeFalse();
        ((ReadOnlySpan<byte>) invalidBytes).IsAscii().Should().BeFalse();
        invalidBytes.AsMemory().IsAscii().Should().BeFalse();
        ((ReadOnlyMemory<byte>) invalidBytes).IsAscii().Should().BeFalse();

        Span<char>.Empty.IsAscii().Should().BeTrue();
        ReadOnlySpan<byte>.Empty.IsAscii().Should().BeTrue();
        Memory<char>.Empty.IsAscii().Should().BeTrue();
        ReadOnlyMemory<byte>.Empty.IsAscii().Should().BeTrue();
    }

    [Fact]
    public static void ScalarAndStringGuardsReturnValues()
    {
        'A'.MustBeAscii().Should().Be('A');
        ((byte) 127).MustBeAscii().Should().Be(127);
        var text = "ASCII";
        text.MustBeAscii().Should().BeSameAs(text);
    }

    [Fact]
    public static void ScalarAndStringFailuresSupportDefaultMessagesFactoriesAndNull()
    {
        const char invalidCharacter = 'é';
        const byte invalidByte = 128;
        const string invalidText = "é";
        string nullText = null;

        ((Action) (() => invalidCharacter.MustBeAscii())).Should().ThrowExactly<ArgumentException>()
                                                         .WithParameterName(nameof(invalidCharacter));
        ((Action) (() => invalidByte.MustBeAscii(message: "custom"))).Should().ThrowExactly<ArgumentException>()
                                                                     .WithParameterName(nameof(invalidByte))
                                                                     .WithMessage("*custom*");
        ((Action) (() => invalidText.MustBeAscii())).Should().ThrowExactly<StringException>()
                                                    .WithParameterName(nameof(invalidText));
        ((Action) (() => nullText.MustBeAscii())).Should().Throw<ArgumentNullException>()
                                                 .WithParameterName(nameof(nullText));
        Test.CustomException(invalidCharacter, (value, factory) => value.MustBeAscii(factory));
        Test.CustomException(invalidByte, (value, factory) => value.MustBeAscii(factory));
        Test.CustomException(nullText, (value, factory) => value.MustBeAscii(factory));
    }

    [Fact]
    public static void BufferGuardsReturnEveryOriginalShape()
    {
        var characters = "ASCII".ToCharArray();
        var bytes = new byte[] { 65, 83, 67, 73, 73 };
        var characterSpan = characters.AsSpan();
        ReadOnlySpan<char> readOnlyCharacterSpan = characters;
        var characterMemory = characters.AsMemory();
        ReadOnlyMemory<char> readOnlyCharacterMemory = characters;
        var byteSpan = bytes.AsSpan();
        ReadOnlySpan<byte> readOnlyByteSpan = bytes;
        var byteMemory = bytes.AsMemory();
        ReadOnlyMemory<byte> readOnlyByteMemory = bytes;

        (characterSpan.MustBeAscii() == characterSpan).Should().BeTrue();
        (readOnlyCharacterSpan.MustBeAscii() == readOnlyCharacterSpan).Should().BeTrue();
        characterMemory.MustBeAscii().Should().Be(characterMemory);
        readOnlyCharacterMemory.MustBeAscii().Should().Be(readOnlyCharacterMemory);
        (byteSpan.MustBeAscii() == byteSpan).Should().BeTrue();
        (readOnlyByteSpan.MustBeAscii() == readOnlyByteSpan).Should().BeTrue();
        byteMemory.MustBeAscii().Should().Be(byteMemory);
        readOnlyByteMemory.MustBeAscii().Should().Be(readOnlyByteMemory);
    }

    [Fact]
    public static void BufferDefaultFailuresCaptureExpressionsAndMessages()
    {
        var spanAct = () =>
        {
            var invalidCharacterSpan = "é".ToCharArray().AsSpan();
            invalidCharacterSpan.MustBeAscii();
        };
        var readOnlySpanAct = () =>
        {
            ReadOnlySpan<byte> invalidByteSpan = [128];
            invalidByteSpan.MustBeAscii(message: "custom");
        };
        var readOnlyCharacterSpanAct = () =>
        {
            ReadOnlySpan<char> invalidCharacterSpan = "é";
            invalidCharacterSpan.MustBeAscii();
        };
        var invalidCharacterMemory = "é".ToCharArray().AsMemory();
        ReadOnlyMemory<char> invalidReadOnlyCharacterMemory = "é".ToCharArray();
        ReadOnlyMemory<byte> invalidByteMemory = new byte[] { 128 };

        spanAct.Should().ThrowExactly<StringException>().WithParameterName("invalidCharacterSpan");
        readOnlyCharacterSpanAct.Should().ThrowExactly<StringException>()
                                .WithParameterName("invalidCharacterSpan");
        readOnlySpanAct.Should().ThrowExactly<ArgumentException>().WithParameterName("invalidByteSpan")
                       .WithMessage("*custom*");
        ((Action) (() => invalidCharacterMemory.MustBeAscii())).Should().ThrowExactly<StringException>()
                                                               .WithParameterName(nameof(invalidCharacterMemory));
        ((Action) (() => invalidReadOnlyCharacterMemory.MustBeAscii())).Should().ThrowExactly<StringException>()
                                                                       .WithParameterName(
                                                                            nameof(invalidReadOnlyCharacterMemory)
                                                                        );
        ((Action) (() => invalidByteMemory.MustBeAscii())).Should().ThrowExactly<ArgumentException>()
                                                          .WithParameterName(nameof(invalidByteMemory));
    }

    [Fact]
    public static void BufferFactoriesReceiveEveryShape()
    {
        Test.CustomSpanException("é".ToCharArray().AsSpan(), (value, factory) => value.MustBeAscii(factory));
        Test.CustomSpanException(
            (ReadOnlySpan<char>) "é".ToCharArray(),
            (value, factory) => value.MustBeAscii(factory)
        );
        Test.CustomMemoryException("é".ToCharArray().AsMemory(), (value, factory) => value.MustBeAscii(factory));
        Test.CustomMemoryException(
            (ReadOnlyMemory<char>) "é".ToCharArray(),
            (value, factory) => value.MustBeAscii(factory)
        );

        Test.CustomSpanException(new byte[] { 128 }.AsSpan(), (value, factory) => value.MustBeAscii(factory));
        Test.CustomSpanException(
            (ReadOnlySpan<byte>) new byte[] { 128 },
            (value, factory) => value.MustBeAscii(factory)
        );
        Test.CustomMemoryException(new byte[] { 128 }.AsMemory(), (value, factory) => value.MustBeAscii(factory));
        Test.CustomMemoryException(
            (ReadOnlyMemory<byte>) new byte[] { 128 },
            (value, factory) => value.MustBeAscii(factory)
        );
    }

    [Fact]
    public static void CustomFactoriesAreNotInvokedForAsciiValues()
    {
        var characters = "ASCII".ToCharArray();
        var bytes = "ASCII"u8.ToArray();
        var characterSpan = characters.AsSpan();
        ReadOnlySpan<char> readOnlyCharacterSpan = characters;
        var byteSpan = bytes.AsSpan();
        ReadOnlySpan<byte> readOnlyByteSpan = bytes;

        'A'.MustBeAscii(_ => throw new InvalidOperationException()).Should().Be('A');
        ((byte) 127).MustBeAscii(_ => throw new InvalidOperationException()).Should().Be(127);
        "ASCII".MustBeAscii(_ => throw new InvalidOperationException()).Should().Be("ASCII");
        characterSpan.MustBeAscii(_ => throw new InvalidOperationException()).SequenceEqual(characterSpan).Should()
                     .BeTrue();
        readOnlyCharacterSpan.MustBeAscii(_ => throw new InvalidOperationException())
                             .SequenceEqual(readOnlyCharacterSpan).Should().BeTrue();
        characters.AsMemory().MustBeAscii(_ => throw new InvalidOperationException()).Should()
                  .Be(characters.AsMemory());
        ((ReadOnlyMemory<char>) characters).MustBeAscii(_ => throw new InvalidOperationException())
                                           .Should().Be((ReadOnlyMemory<char>) characters);
        byteSpan.MustBeAscii(_ => throw new InvalidOperationException()).SequenceEqual(byteSpan).Should().BeTrue();
        readOnlyByteSpan.MustBeAscii(_ => throw new InvalidOperationException()).SequenceEqual(readOnlyByteSpan)
                        .Should().BeTrue();
        bytes.AsMemory().MustBeAscii(_ => throw new InvalidOperationException()).Should().Be(bytes.AsMemory());
        ((ReadOnlyMemory<byte>) bytes).MustBeAscii(_ => throw new InvalidOperationException())
                                      .Should().Be((ReadOnlyMemory<byte>) bytes);
    }
}
