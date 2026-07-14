using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class StringInspectionGuardTests
{
    [Fact]
    public static void EveryGuardReturnsEveryOriginalReceiverShape()
    {
        AssertDigitReturns("1٢３");
        AssertLetterOrDigitReturns("AzΩ٢");
        AssertUpperCaseReturns("AΩ 123!");
        AssertLowerCaseReturns("aω 123!");
        AssertBase64Returns("T W\tF\ru\n");
        AssertHexadecimalReturns("09aAfF");
        AssertNoWhiteSpaceReturns("Az-09");

        string.Empty.MustContainOnlyDigits().Should().BeEmpty();
        Span<char>.Empty.MustContainOnlyLettersOrDigits().IsEmpty.Should().BeTrue();
        ReadOnlySpan<char>.Empty.MustBeUpperCase().IsEmpty.Should().BeTrue();
        Memory<char>.Empty.MustBeLowerCase().IsEmpty.Should().BeTrue();
        ReadOnlyMemory<char>.Empty.MustBeBase64().IsEmpty.Should().BeTrue();
        Span<char>.Empty.MustBeHexadecimal().IsEmpty.Should().BeTrue();
        ReadOnlySpan<char>.Empty.MustNotContainWhiteSpace().IsEmpty.Should().BeTrue();
    }

    [Fact]
    public static void NullStringsThrowArgumentNullExceptionForEveryDefaultGuard()
    {
        string nullText = null;

        ((Action) (() => nullText.MustContainOnlyDigits())).Should().ThrowExactly<ArgumentNullException>()
                                                           .WithParameterName(nameof(nullText));
        ((Action) (() => nullText.MustContainOnlyLettersOrDigits())).Should().ThrowExactly<ArgumentNullException>();
        ((Action) (() => nullText.MustBeUpperCase())).Should().ThrowExactly<ArgumentNullException>();
        ((Action) (() => nullText.MustBeLowerCase())).Should().ThrowExactly<ArgumentNullException>();
        ((Action) (() => nullText.MustBeBase64())).Should().ThrowExactly<ArgumentNullException>();
        ((Action) (() => nullText.MustBeHexadecimal())).Should().ThrowExactly<ArgumentNullException>();
        ((Action) (() => nullText.MustNotContainWhiteSpace())).Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public static void InvalidStringContentThrowsStringExceptionForEveryGuard()
    {
        AssertStringFailure("a", value => value.MustContainOnlyDigits());
        AssertStringFailure("!", value => value.MustContainOnlyLettersOrDigits());
        AssertStringFailure("a", value => value.MustBeUpperCase());
        AssertStringFailure("A", value => value.MustBeLowerCase());
        AssertStringFailure("_", value => value.MustBeBase64());
        AssertStringFailure("g", value => value.MustBeHexadecimal());
        AssertStringFailure(" ", value => value.MustNotContainWhiteSpace());
    }

    [Fact]
    public static void DefaultBufferFailuresCaptureExpressionsAndSupportCustomMessages()
    {
        var spanAct = () =>
        {
            var invalidSpan = "a".ToCharArray().AsSpan();
            invalidSpan.MustContainOnlyDigits();
        };
        var readOnlySpanAct = () =>
        {
            ReadOnlySpan<char> invalidReadOnlySpan = "a";
            invalidReadOnlySpan.MustContainOnlyDigits(message: "custom");
        };
        var invalidMemory = "a".ToCharArray().AsMemory();
        ReadOnlyMemory<char> invalidReadOnlyMemory = "a".ToCharArray();

        spanAct.Should().ThrowExactly<StringException>().WithParameterName("invalidSpan");
        readOnlySpanAct.Should().ThrowExactly<StringException>()
                       .WithParameterName("invalidReadOnlySpan")
                       .WithMessage("*custom*");
        ((Action) (() => invalidMemory.MustContainOnlyDigits())).Should().ThrowExactly<StringException>()
                                                                .WithParameterName(nameof(invalidMemory));
        ((Action) (() => invalidReadOnlyMemory.MustContainOnlyDigits())).Should().ThrowExactly<StringException>()
                                                                        .WithParameterName(
                                                                             nameof(invalidReadOnlyMemory)
                                                                         );
    }

    [Fact]
    public static void EveryStringFactoryReceivesNullAndInvalidContent()
    {
        string nullText = null;
        Test.CustomException(nullText, (value, factory) => value.MustContainOnlyDigits(factory));
        Test.CustomException("a", (value, factory) => value.MustContainOnlyDigits(factory));
        Test.CustomException("!", (value, factory) => value.MustContainOnlyLettersOrDigits(factory));
        Test.CustomException("a", (value, factory) => value.MustBeUpperCase(factory));
        Test.CustomException("A", (value, factory) => value.MustBeLowerCase(factory));
        Test.CustomException("_", (value, factory) => value.MustBeBase64(factory));
        Test.CustomException("g", (value, factory) => value.MustBeHexadecimal(factory));
        Test.CustomException(" ", (value, factory) => value.MustNotContainWhiteSpace(factory));
    }

    [Fact]
    public static void BufferFactoriesReceiveEveryReceiverShape()
    {
        Test.CustomSpanException(
            "a".ToCharArray().AsSpan(),
            (value, factory) => value.MustContainOnlyDigits(factory)
        );
        Test.CustomSpanException(
            (ReadOnlySpan<char>) "!".ToCharArray(),
            (value, factory) => value.MustContainOnlyLettersOrDigits(factory)
        );
        Test.CustomMemoryException(
            "a".ToCharArray().AsMemory(),
            (value, factory) => value.MustBeUpperCase(factory)
        );
        Test.CustomMemoryException(
            (ReadOnlyMemory<char>) "A".ToCharArray(),
            (value, factory) => value.MustBeLowerCase(factory)
        );

        Test.CustomSpanException("_".ToCharArray().AsSpan(), (value, factory) => value.MustBeBase64(factory));
        Test.CustomSpanException(
            (ReadOnlySpan<char>) "g".ToCharArray(),
            (value, factory) => value.MustBeHexadecimal(factory)
        );
        Test.CustomMemoryException(
            " ".ToCharArray().AsMemory(),
            (value, factory) => value.MustNotContainWhiteSpace(factory)
        );
    }

    [Fact]
    public static void WhiteSpaceGuardUsesUnicodeClassificationAtEveryPosition()
    {
        "\u00A0abc".Invoking(value => value.MustNotContainWhiteSpace()).Should().ThrowExactly<StringException>();
        "ab\u2003cd".Invoking(value => value.MustNotContainWhiteSpace()).Should().ThrowExactly<StringException>();
        "abc\u2029".Invoking(value => value.MustNotContainWhiteSpace()).Should().ThrowExactly<StringException>();
        "abc-def".MustNotContainWhiteSpace().Should().Be("abc-def");
    }

    private static void AssertDigitReturns(string value) => AssertEveryShape(value, GuardFamily.Digits);

    private static void AssertLetterOrDigitReturns(string value) =>
        AssertEveryShape(value, GuardFamily.LettersOrDigits);

    private static void AssertUpperCaseReturns(string value) => AssertEveryShape(value, GuardFamily.UpperCase);

    private static void AssertLowerCaseReturns(string value) => AssertEveryShape(value, GuardFamily.LowerCase);

    private static void AssertBase64Returns(string value) => AssertEveryShape(value, GuardFamily.Base64);

    private static void AssertHexadecimalReturns(string value) => AssertEveryShape(value, GuardFamily.Hexadecimal);

    private static void AssertNoWhiteSpaceReturns(string value) => AssertEveryShape(value, GuardFamily.NoWhiteSpace);

    private static void AssertEveryShape(string value, GuardFamily family)
    {
        var characters = value.ToCharArray();
        var span = characters.AsSpan();
        ReadOnlySpan<char> readOnlySpan = characters;
        var memory = characters.AsMemory();
        ReadOnlyMemory<char> readOnlyMemory = characters;

        switch (family)
        {
            case GuardFamily.Digits:
                value.MustContainOnlyDigits().Should().BeSameAs(value);
                (span.MustContainOnlyDigits() == span).Should().BeTrue();
                (readOnlySpan.MustContainOnlyDigits() == readOnlySpan).Should().BeTrue();
                memory.MustContainOnlyDigits().Should().Be(memory);
                readOnlyMemory.MustContainOnlyDigits().Should().Be(readOnlyMemory);
                break;
            case GuardFamily.LettersOrDigits:
                value.MustContainOnlyLettersOrDigits().Should().BeSameAs(value);
                (span.MustContainOnlyLettersOrDigits() == span).Should().BeTrue();
                (readOnlySpan.MustContainOnlyLettersOrDigits() == readOnlySpan).Should().BeTrue();
                memory.MustContainOnlyLettersOrDigits().Should().Be(memory);
                readOnlyMemory.MustContainOnlyLettersOrDigits().Should().Be(readOnlyMemory);
                break;
            case GuardFamily.UpperCase:
                value.MustBeUpperCase().Should().BeSameAs(value);
                (span.MustBeUpperCase() == span).Should().BeTrue();
                (readOnlySpan.MustBeUpperCase() == readOnlySpan).Should().BeTrue();
                memory.MustBeUpperCase().Should().Be(memory);
                readOnlyMemory.MustBeUpperCase().Should().Be(readOnlyMemory);
                break;
            case GuardFamily.LowerCase:
                value.MustBeLowerCase().Should().BeSameAs(value);
                (span.MustBeLowerCase() == span).Should().BeTrue();
                (readOnlySpan.MustBeLowerCase() == readOnlySpan).Should().BeTrue();
                memory.MustBeLowerCase().Should().Be(memory);
                readOnlyMemory.MustBeLowerCase().Should().Be(readOnlyMemory);
                break;
            case GuardFamily.Base64:
                value.MustBeBase64().Should().BeSameAs(value);
                (span.MustBeBase64() == span).Should().BeTrue();
                (readOnlySpan.MustBeBase64() == readOnlySpan).Should().BeTrue();
                memory.MustBeBase64().Should().Be(memory);
                readOnlyMemory.MustBeBase64().Should().Be(readOnlyMemory);
                break;
            case GuardFamily.Hexadecimal:
                value.MustBeHexadecimal().Should().BeSameAs(value);
                (span.MustBeHexadecimal() == span).Should().BeTrue();
                (readOnlySpan.MustBeHexadecimal() == readOnlySpan).Should().BeTrue();
                memory.MustBeHexadecimal().Should().Be(memory);
                readOnlyMemory.MustBeHexadecimal().Should().Be(readOnlyMemory);
                break;
            case GuardFamily.NoWhiteSpace:
                value.MustNotContainWhiteSpace().Should().BeSameAs(value);
                (span.MustNotContainWhiteSpace() == span).Should().BeTrue();
                (readOnlySpan.MustNotContainWhiteSpace() == readOnlySpan).Should().BeTrue();
                memory.MustNotContainWhiteSpace().Should().Be(memory);
                readOnlyMemory.MustNotContainWhiteSpace().Should().Be(readOnlyMemory);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(family), family, null);
        }
    }

    private static void AssertStringFailure(string invalidValue, Action<string> guard) =>
        guard.Invoking(assertion => assertion(invalidValue))
             .Should().ThrowExactly<StringException>();

    private enum GuardFamily
    {
        Digits,
        LettersOrDigits,
        UpperCase,
        LowerCase,
        Base64,
        Hexadecimal,
        NoWhiteSpace,
    }
}
