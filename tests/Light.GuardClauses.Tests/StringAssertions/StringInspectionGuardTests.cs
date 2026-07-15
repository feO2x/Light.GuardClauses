using System;
using FluentAssertions;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

// The string-inspection guards exist for seven families (digits, letters-or-digits, upper case,
// lower case, Base64, hexadecimal, and no-white-space) across five receiver shapes (string,
// Span, ReadOnlySpan, Memory, ReadOnlyMemory). The theories below run one test case per family
// so that a failing case immediately identifies the defective guard family, while the shape
// helpers keep the five receiver shapes in a Single Point of Truth.
public static class StringInspectionGuardTests
{
    [Theory]
    [InlineData(GuardFamily.Digits, "1٢３")]
    [InlineData(GuardFamily.LettersOrDigits, "AzΩ٢")]
    [InlineData(GuardFamily.UpperCase, "AΩ 123!")]
    [InlineData(GuardFamily.LowerCase, "aω 123!")]
    [InlineData(GuardFamily.Base64, "T W\tF\ru\n")]
    [InlineData(GuardFamily.Hexadecimal, "09aAfF")]
    [InlineData(GuardFamily.NoWhiteSpace, "Az-09")]
    [InlineData(GuardFamily.Digits, "")]
    [InlineData(GuardFamily.LettersOrDigits, "")]
    [InlineData(GuardFamily.UpperCase, "")]
    [InlineData(GuardFamily.LowerCase, "")]
    [InlineData(GuardFamily.Base64, "")]
    [InlineData(GuardFamily.Hexadecimal, "")]
    [InlineData(GuardFamily.NoWhiteSpace, "")]
    public static void GuardReturnsEveryOriginalReceiverShape(GuardFamily family, string validValue) =>
        AssertEveryShape(validValue, family);

    [Theory]
    [InlineData(GuardFamily.Digits, "1٢３")]
    [InlineData(GuardFamily.LettersOrDigits, "AzΩ٢")]
    [InlineData(GuardFamily.UpperCase, "AΩ 123!")]
    [InlineData(GuardFamily.LowerCase, "aω 123!")]
    [InlineData(GuardFamily.Base64, "T W\tF\ru\n")]
    [InlineData(GuardFamily.Hexadecimal, "09aAfF")]
    [InlineData(GuardFamily.NoWhiteSpace, "Az-09")]
    public static void FactoryGuardReturnsEveryOriginalReceiverShape(GuardFamily family, string validValue) =>
        AssertEveryFactoryShape(validValue, family);

    [Theory]
    [InlineData(GuardFamily.Digits)]
    [InlineData(GuardFamily.LettersOrDigits)]
    [InlineData(GuardFamily.UpperCase)]
    [InlineData(GuardFamily.LowerCase)]
    [InlineData(GuardFamily.Base64)]
    [InlineData(GuardFamily.Hexadecimal)]
    [InlineData(GuardFamily.NoWhiteSpace)]
    public static void NullStringThrowsArgumentNullException(GuardFamily family)
    {
        var act = () => InvokeStringGuard(family, null);

        act.Should().ThrowExactly<ArgumentNullException>()
           .WithParameterName("value");
    }

    [Theory]
    [InlineData(GuardFamily.Digits, "a")]
    [InlineData(GuardFamily.LettersOrDigits, "!")]
    [InlineData(GuardFamily.UpperCase, "a")]
    [InlineData(GuardFamily.LowerCase, "A")]
    [InlineData(GuardFamily.Base64, "_")]
    [InlineData(GuardFamily.Hexadecimal, "g")]
    [InlineData(GuardFamily.NoWhiteSpace, " ")]
    public static void InvalidStringContentThrowsStringException(GuardFamily family, string invalidValue)
    {
        var act = () => InvokeStringGuard(family, invalidValue);

        act.Should().ThrowExactly<StringException>();
    }

    [Theory]
    [InlineData(GuardFamily.Digits, null)]
    [InlineData(GuardFamily.Digits, "a")]
    [InlineData(GuardFamily.LettersOrDigits, "!")]
    [InlineData(GuardFamily.UpperCase, "a")]
    [InlineData(GuardFamily.LowerCase, "A")]
    [InlineData(GuardFamily.Base64, "_")]
    [InlineData(GuardFamily.Hexadecimal, "g")]
    [InlineData(GuardFamily.NoWhiteSpace, null)]
    [InlineData(GuardFamily.NoWhiteSpace, " ")]
    public static void StringFactoryReceivesNullOrInvalidContent(GuardFamily family, string invalidValue) =>
        Test.CustomException(
            invalidValue,
            (value, factory) => InvokeStringFactoryGuard(family, value, factory)
        );

    [Theory]
    [InlineData(GuardFamily.Digits, "a")]
    [InlineData(GuardFamily.LettersOrDigits, "!")]
    [InlineData(GuardFamily.UpperCase, "a")]
    [InlineData(GuardFamily.LowerCase, "A")]
    [InlineData(GuardFamily.Base64, "_")]
    [InlineData(GuardFamily.Hexadecimal, "g")]
    [InlineData(GuardFamily.NoWhiteSpace, " ")]
    public static void BufferFactoryReceivesEveryInvalidReceiverShape(GuardFamily family, string invalidValue) =>
        AssertEveryBufferFactoryFailure(invalidValue, family);

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

    [Theory]
    [InlineData("!", "invalidReadOnlySpan must contain only Unicode letters or decimal digits.")]
    public static void DefaultReadOnlySpanFailure_LettersOrDigits(string invalidValue, string expectedMessage) =>
        AssertReadOnlySpanFailure(
            invalidValue,
            expectedMessage,
            (invalidReadOnlySpan, parameterName) => invalidReadOnlySpan.MustContainOnlyLettersOrDigits(parameterName)
        );

    [Theory]
    [InlineData("a", "invalidReadOnlySpan must contain no Unicode lowercase characters.")]
    public static void DefaultReadOnlySpanFailure_UpperCase(string invalidValue, string expectedMessage) =>
        AssertReadOnlySpanFailure(
            invalidValue,
            expectedMessage,
            (invalidReadOnlySpan, parameterName) => invalidReadOnlySpan.MustBeUpperCase(parameterName)
        );

    [Theory]
    [InlineData("A", "invalidReadOnlySpan must contain no Unicode uppercase characters.")]
    public static void DefaultReadOnlySpanFailure_LowerCase(string invalidValue, string expectedMessage) =>
        AssertReadOnlySpanFailure(
            invalidValue,
            expectedMessage,
            (invalidReadOnlySpan, parameterName) => invalidReadOnlySpan.MustBeLowerCase(parameterName)
        );

    [Theory]
    [InlineData("_", "invalidReadOnlySpan must be valid standard Base64.")]
    public static void DefaultReadOnlySpanFailure_Base64(string invalidValue, string expectedMessage) =>
        AssertReadOnlySpanFailure(
            invalidValue,
            expectedMessage,
            (invalidReadOnlySpan, parameterName) => invalidReadOnlySpan.MustBeBase64(parameterName)
        );

    [Theory]
    [InlineData("g", "invalidReadOnlySpan must contain only ASCII hexadecimal characters.")]
    public static void DefaultReadOnlySpanFailure_Hexadecimal(string invalidValue, string expectedMessage) =>
        AssertReadOnlySpanFailure(
            invalidValue,
            expectedMessage,
            (invalidReadOnlySpan, parameterName) => invalidReadOnlySpan.MustBeHexadecimal(parameterName)
        );

    [Theory]
    [InlineData("\u00A0abc")]
    [InlineData("ab\u2003cd")]
    [InlineData("abc\u2029")]
    public static void WhiteSpaceGuardUsesUnicodeClassificationAtEveryPosition(string invalidValue) =>
        invalidValue.Invoking(value => value.MustNotContainWhiteSpace())
                    .Should().ThrowExactly<StringException>();

    [Fact]
    public static void WhiteSpaceGuardAcceptsContentWithoutWhiteSpace() =>
        "abc-def".MustNotContainWhiteSpace().Should().Be("abc-def");

    private static string InvokeStringGuard(GuardFamily family, string value) =>
        family switch
        {
            GuardFamily.Digits => value.MustContainOnlyDigits(),
            GuardFamily.LettersOrDigits => value.MustContainOnlyLettersOrDigits(),
            GuardFamily.UpperCase => value.MustBeUpperCase(),
            GuardFamily.LowerCase => value.MustBeLowerCase(),
            GuardFamily.Base64 => value.MustBeBase64(),
            GuardFamily.Hexadecimal => value.MustBeHexadecimal(),
            GuardFamily.NoWhiteSpace => value.MustNotContainWhiteSpace(),
            _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
        };

    private static string InvokeStringFactoryGuard(
        GuardFamily family,
        string value,
        Func<string, Exception> exceptionFactory
    ) =>
        family switch
        {
            GuardFamily.Digits => value.MustContainOnlyDigits(exceptionFactory),
            GuardFamily.LettersOrDigits => value.MustContainOnlyLettersOrDigits(exceptionFactory),
            GuardFamily.UpperCase => value.MustBeUpperCase(exceptionFactory),
            GuardFamily.LowerCase => value.MustBeLowerCase(exceptionFactory),
            GuardFamily.Base64 => value.MustBeBase64(exceptionFactory),
            GuardFamily.Hexadecimal => value.MustBeHexadecimal(exceptionFactory),
            GuardFamily.NoWhiteSpace => value.MustNotContainWhiteSpace(exceptionFactory),
            _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
        };

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

    private static void AssertEveryFactoryShape(string value, GuardFamily family)
    {
        var characters = value.ToCharArray();
        var span = characters.AsSpan();
        ReadOnlySpan<char> readOnlySpan = characters;
        var memory = characters.AsMemory();
        ReadOnlyMemory<char> readOnlyMemory = characters;
        Func<string, Exception> stringFactory = _ => new ("The factory should not be invoked.");
        ReadOnlySpanExceptionFactory<char> bufferFactory = _ => new ("The factory should not be invoked.");

        switch (family)
        {
            case GuardFamily.Digits:
                value.MustContainOnlyDigits(stringFactory).Should().BeSameAs(value);
                (span.MustContainOnlyDigits(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustContainOnlyDigits(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustContainOnlyDigits(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustContainOnlyDigits(bufferFactory).Should().Be(readOnlyMemory);
                break;
            case GuardFamily.LettersOrDigits:
                value.MustContainOnlyLettersOrDigits(stringFactory).Should().BeSameAs(value);
                (span.MustContainOnlyLettersOrDigits(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustContainOnlyLettersOrDigits(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustContainOnlyLettersOrDigits(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustContainOnlyLettersOrDigits(bufferFactory).Should().Be(readOnlyMemory);
                break;
            case GuardFamily.UpperCase:
                value.MustBeUpperCase(stringFactory).Should().BeSameAs(value);
                (span.MustBeUpperCase(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustBeUpperCase(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustBeUpperCase(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustBeUpperCase(bufferFactory).Should().Be(readOnlyMemory);
                break;
            case GuardFamily.LowerCase:
                value.MustBeLowerCase(stringFactory).Should().BeSameAs(value);
                (span.MustBeLowerCase(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustBeLowerCase(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustBeLowerCase(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustBeLowerCase(bufferFactory).Should().Be(readOnlyMemory);
                break;
            case GuardFamily.Base64:
                value.MustBeBase64(stringFactory).Should().BeSameAs(value);
                (span.MustBeBase64(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustBeBase64(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustBeBase64(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustBeBase64(bufferFactory).Should().Be(readOnlyMemory);
                break;
            case GuardFamily.Hexadecimal:
                value.MustBeHexadecimal(stringFactory).Should().BeSameAs(value);
                (span.MustBeHexadecimal(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustBeHexadecimal(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustBeHexadecimal(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustBeHexadecimal(bufferFactory).Should().Be(readOnlyMemory);
                break;
            case GuardFamily.NoWhiteSpace:
                value.MustNotContainWhiteSpace(stringFactory).Should().BeSameAs(value);
                (span.MustNotContainWhiteSpace(bufferFactory) == span).Should().BeTrue();
                (readOnlySpan.MustNotContainWhiteSpace(bufferFactory) == readOnlySpan).Should().BeTrue();
                memory.MustNotContainWhiteSpace(bufferFactory).Should().Be(memory);
                readOnlyMemory.MustNotContainWhiteSpace(bufferFactory).Should().Be(readOnlyMemory);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(family), family, null);
        }
    }

    private static void AssertEveryBufferFactoryFailure(string invalidValue, GuardFamily family)
    {
        var characters = invalidValue.ToCharArray();

        switch (family)
        {
            case GuardFamily.Digits:
                Test.CustomSpanException(characters.AsSpan(), (value, factory) => value.MustContainOnlyDigits(factory));
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustContainOnlyDigits(factory)
                );
                Test.CustomMemoryException(
                    characters.AsMemory(),
                    (value, factory) => value.MustContainOnlyDigits(factory)
                );
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustContainOnlyDigits(factory)
                );
                break;
            case GuardFamily.LettersOrDigits:
                Test.CustomSpanException(
                    characters.AsSpan(),
                    (value, factory) => value.MustContainOnlyLettersOrDigits(factory)
                );
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustContainOnlyLettersOrDigits(factory)
                );
                Test.CustomMemoryException(
                    characters.AsMemory(),
                    (value, factory) => value.MustContainOnlyLettersOrDigits(factory)
                );
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustContainOnlyLettersOrDigits(factory)
                );
                break;
            case GuardFamily.UpperCase:
                Test.CustomSpanException(characters.AsSpan(), (value, factory) => value.MustBeUpperCase(factory));
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustBeUpperCase(factory)
                );
                Test.CustomMemoryException(characters.AsMemory(), (value, factory) => value.MustBeUpperCase(factory));
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustBeUpperCase(factory)
                );
                break;
            case GuardFamily.LowerCase:
                Test.CustomSpanException(characters.AsSpan(), (value, factory) => value.MustBeLowerCase(factory));
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustBeLowerCase(factory)
                );
                Test.CustomMemoryException(characters.AsMemory(), (value, factory) => value.MustBeLowerCase(factory));
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustBeLowerCase(factory)
                );
                break;
            case GuardFamily.Base64:
                Test.CustomSpanException(characters.AsSpan(), (value, factory) => value.MustBeBase64(factory));
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustBeBase64(factory)
                );
                Test.CustomMemoryException(characters.AsMemory(), (value, factory) => value.MustBeBase64(factory));
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustBeBase64(factory)
                );
                break;
            case GuardFamily.Hexadecimal:
                Test.CustomSpanException(characters.AsSpan(), (value, factory) => value.MustBeHexadecimal(factory));
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustBeHexadecimal(factory)
                );
                Test.CustomMemoryException(characters.AsMemory(), (value, factory) => value.MustBeHexadecimal(factory));
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustBeHexadecimal(factory)
                );
                break;
            case GuardFamily.NoWhiteSpace:
                Test.CustomSpanException(
                    characters.AsSpan(),
                    (value, factory) => value.MustNotContainWhiteSpace(factory)
                );
                Test.CustomSpanException(
                    (ReadOnlySpan<char>) characters,
                    (value, factory) => value.MustNotContainWhiteSpace(factory)
                );
                Test.CustomMemoryException(
                    characters.AsMemory(),
                    (value, factory) => value.MustNotContainWhiteSpace(factory)
                );
                Test.CustomMemoryException(
                    (ReadOnlyMemory<char>) characters,
                    (value, factory) => value.MustNotContainWhiteSpace(factory)
                );
                break;
            default: throw new ArgumentOutOfRangeException(nameof(family), family, null);
        }
    }

    private static void AssertReadOnlySpanFailure(
        string invalidValue,
        string expectedMessage,
        ReadOnlySpanGuard guard
    )
    {
        var act = () =>
        {
            ReadOnlySpan<char> invalidReadOnlySpan = invalidValue;
            guard(invalidReadOnlySpan, nameof(invalidReadOnlySpan));
        };

        act.Should().ThrowExactly<StringException>()
           .WithParameterName("invalidReadOnlySpan")
           .WithMessage($"{expectedMessage}*");
    }

    private delegate ReadOnlySpan<char> ReadOnlySpanGuard(ReadOnlySpan<char> value, string parameterName);

    public enum GuardFamily
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
