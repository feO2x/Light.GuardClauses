using System;
using System.Buffers.Text;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using Xunit.Sdk;
using GuardFamily = Light.GuardClauses.Tests.StringAssertions.StringInspectionGuardTests.GuardFamily;

namespace Light.GuardClauses.Tests.StringAssertions;

// The string-inspection predicates exist for six families (digits, letters-or-digits, upper case,
// lower case, Base64, and hexadecimal) across five receiver shapes (string, Span, ReadOnlySpan,
// Memory, ReadOnlyMemory). The theories below run one test case per family so that a failing case
// immediately identifies the defective predicate family, while the shape helper keeps the five
// receiver shapes in a Single Point of Truth.
public static class StringInspectionPredicateTests
{
    private static readonly Base64Validator PortableBase64Validator =
        typeof(Check).GetMethod("IsBase64Portable", BindingFlags.NonPublic | BindingFlags.Static)!
                     .CreateDelegate<Base64Validator>();

    [Theory]
    [InlineData(GuardFamily.Digits)]
    [InlineData(GuardFamily.LettersOrDigits)]
    [InlineData(GuardFamily.UpperCase)]
    [InlineData(GuardFamily.LowerCase)]
    [InlineData(GuardFamily.Base64)]
    [InlineData(GuardFamily.Hexadecimal)]
    public static void NullStringsFailEveryPredicate(GuardFamily family) =>
        InvokeStringPredicate(family, null).Should().BeFalse();

    [Theory]
    [InlineData(GuardFamily.Digits)]
    [InlineData(GuardFamily.LettersOrDigits)]
    [InlineData(GuardFamily.UpperCase)]
    [InlineData(GuardFamily.LowerCase)]
    [InlineData(GuardFamily.Base64)]
    [InlineData(GuardFamily.Hexadecimal)]
    public static void EmptyInputsPassEveryPredicateAndReceiverShape(GuardFamily family) =>
        AssertEveryShape(family, string.Empty, true);

    [Theory]
    [InlineData(GuardFamily.Digits, "1٢３")]
    [InlineData(GuardFamily.LettersOrDigits, "AzΩ٢")]
    [InlineData(GuardFamily.UpperCase, "AΩ 123!")]
    [InlineData(GuardFamily.LowerCase, "aω 123!")]
    [InlineData(GuardFamily.Base64, "T W\tF\ru\n")]
    [InlineData(GuardFamily.Hexadecimal, "09aAfF")]
    public static void ValidContentPassesEveryReceiverShape(GuardFamily family, string validValue) =>
        AssertEveryShape(family, validValue, true);

    [Theory]
    [InlineData(GuardFamily.Digits, "x12")]
    [InlineData(GuardFamily.Digits, "1x2")]
    [InlineData(GuardFamily.Digits, "12x")]
    [InlineData(GuardFamily.LettersOrDigits, "-Az")]
    [InlineData(GuardFamily.LettersOrDigits, "A-z")]
    [InlineData(GuardFamily.LettersOrDigits, "Az-")]
    [InlineData(GuardFamily.UpperCase, "aAB")]
    [InlineData(GuardFamily.UpperCase, "AaB")]
    [InlineData(GuardFamily.UpperCase, "ABa")]
    [InlineData(GuardFamily.LowerCase, "Aab")]
    [InlineData(GuardFamily.LowerCase, "aAb")]
    [InlineData(GuardFamily.LowerCase, "abA")]
    [InlineData(GuardFamily.Hexadecimal, "x09A")]
    [InlineData(GuardFamily.Hexadecimal, "0x9A")]
    [InlineData(GuardFamily.Hexadecimal, "09Ax")]
    public static void InvalidContentIsDetectedAtTheBeginningMiddleAndEnd(GuardFamily family, string invalidValue) =>
        InvokeStringPredicate(family, invalidValue).Should().BeFalse();

    [Theory]
    [InlineData(GuardFamily.UpperCase, "123 !\t中", true)]
    [InlineData(GuardFamily.LowerCase, "123 !\t中", true)]
    [InlineData(GuardFamily.UpperCase, "ÄΩ", true)]
    [InlineData(GuardFamily.LowerCase, "äω", true)]
    [InlineData(GuardFamily.UpperCase, "Äω", false)]
    [InlineData(GuardFamily.LowerCase, "äΩ", false)]
    public static void CasingAllowsUncasedContentAndUsesUnicodeClassification(
        GuardFamily family,
        string value,
        bool expected
    ) =>
        InvokeStringPredicate(family, value).Should().Be(expected);

    [Theory]
    [InlineData("", true)]
    [InlineData(" \t\r\n", true)]
    [InlineData("TWFu", true)]
    [InlineData("TWE=", true)]
    [InlineData("TQ==", true)]
    [InlineData(" T W F u ", true)]
    [InlineData("T\tW\rE\n=", true)]
    [InlineData("A", false)]
    [InlineData("AAA", false)]
    [InlineData("AAAAA", false)]
    [InlineData("=AAA", false)]
    [InlineData("A=AA", false)]
    [InlineData("AA=A", false)]
    [InlineData("A===", false)]
    [InlineData("AA==AA==", false)]
    [InlineData("!AAA", false)]
    [InlineData("A!AA", false)]
    [InlineData("AAA!", false)]
    [InlineData("SGVsbG8_", false)]
    [InlineData("SGVsbG8-", false)]
    [InlineData("AA\v==", false)]
    [InlineData("AA\u00A0==", false)]
    public static void Base64AcceptsOnlyTheDefinedAlphabetStructureAndWhiteSpace(string value, bool expected) =>
        value.IsBase64().Should().Be(expected);

    [Fact]
    public static void PortableBase64ValidatorMatchesNet10OverDeterministicCorpus()
    {
        var random = new Random(153);
        const string corpusCharacters =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=_- \t\r\n\v\u00A0!";

        for (var iteration = 0; iteration < 10_000; ++iteration)
        {
            var characters = new char[random.Next(0, 65)];
            for (var index = 0; index < characters.Length; ++index)
            {
                characters[index] = corpusCharacters[random.Next(corpusCharacters.Length)];
            }

            AssertPortableBase64MatchesFramework(characters);
        }

        for (var length = 0; length <= 128; ++length)
        {
            var bytes = new byte[length];
            random.NextBytes(bytes);
            var valid = Convert.ToBase64String(bytes);
            AssertPortableBase64MatchesFramework(valid);
            AssertPortableBase64MatchesFramework(InsertWhiteSpace(valid));
        }
    }

    [Fact]
    public static void EveryUtf16CodeUnitMatchesTheDefiningClassifiers()
    {
        Span<char> value = stackalloc char[1];

        for (var codeUnit = (int) char.MinValue; codeUnit <= char.MaxValue; ++codeUnit)
        {
            var character = (char) codeUnit;
            value[0] = character;
            AssertEqual(char.IsDigit(character), character.IsDigit(), character, "digit character");
            AssertEqual(char.IsLetter(character), character.IsLetter(), character, "letter character");
            AssertEqual(char.IsDigit(character), ((ReadOnlySpan<char>) value).ContainsOnlyDigits(), character, "digit");
            AssertEqual(
                char.IsLetterOrDigit(character),
                ((ReadOnlySpan<char>) value).ContainsOnlyLettersOrDigits(),
                character,
                "letter or digit"
            );
            AssertEqual(!char.IsLower(character), ((ReadOnlySpan<char>) value).IsUpperCase(), character, "upper case");
            AssertEqual(!char.IsUpper(character), ((ReadOnlySpan<char>) value).IsLowerCase(), character, "lower case");
            AssertEqual(
                IsAsciiHexDigit(character),
                ((ReadOnlySpan<char>) value).IsHexadecimal(),
                character,
                "hexadecimal"
            );
            AssertEqual(!char.IsWhiteSpace(character), IsAcceptedByWhiteSpaceGuard(value), character, "white space");
        }
    }

    private static bool InvokeStringPredicate(GuardFamily family, string value) =>
        family switch
        {
            GuardFamily.Digits => value.ContainsOnlyDigits(),
            GuardFamily.LettersOrDigits => value.ContainsOnlyLettersOrDigits(),
            GuardFamily.UpperCase => value.IsUpperCase(),
            GuardFamily.LowerCase => value.IsLowerCase(),
            GuardFamily.Base64 => value.IsBase64(),
            GuardFamily.Hexadecimal => value.IsHexadecimal(),
            _ => throw new ArgumentOutOfRangeException(nameof(family), family, null),
        };

    private static void AssertEveryShape(GuardFamily family, string value, bool expected)
    {
        var characters = value.ToCharArray();
        var span = characters.AsSpan();
        ReadOnlySpan<char> readOnlySpan = characters;
        var memory = characters.AsMemory();
        ReadOnlyMemory<char> readOnlyMemory = characters;

        switch (family)
        {
            case GuardFamily.Digits:
                value.ContainsOnlyDigits().Should().Be(expected);
                span.ContainsOnlyDigits().Should().Be(expected);
                readOnlySpan.ContainsOnlyDigits().Should().Be(expected);
                memory.ContainsOnlyDigits().Should().Be(expected);
                readOnlyMemory.ContainsOnlyDigits().Should().Be(expected);
                break;
            case GuardFamily.LettersOrDigits:
                value.ContainsOnlyLettersOrDigits().Should().Be(expected);
                span.ContainsOnlyLettersOrDigits().Should().Be(expected);
                readOnlySpan.ContainsOnlyLettersOrDigits().Should().Be(expected);
                memory.ContainsOnlyLettersOrDigits().Should().Be(expected);
                readOnlyMemory.ContainsOnlyLettersOrDigits().Should().Be(expected);
                break;
            case GuardFamily.UpperCase:
                value.IsUpperCase().Should().Be(expected);
                span.IsUpperCase().Should().Be(expected);
                readOnlySpan.IsUpperCase().Should().Be(expected);
                memory.IsUpperCase().Should().Be(expected);
                readOnlyMemory.IsUpperCase().Should().Be(expected);
                break;
            case GuardFamily.LowerCase:
                value.IsLowerCase().Should().Be(expected);
                span.IsLowerCase().Should().Be(expected);
                readOnlySpan.IsLowerCase().Should().Be(expected);
                memory.IsLowerCase().Should().Be(expected);
                readOnlyMemory.IsLowerCase().Should().Be(expected);
                break;
            case GuardFamily.Base64:
                value.IsBase64().Should().Be(expected);
                span.IsBase64().Should().Be(expected);
                readOnlySpan.IsBase64().Should().Be(expected);
                memory.IsBase64().Should().Be(expected);
                readOnlyMemory.IsBase64().Should().Be(expected);
                break;
            case GuardFamily.Hexadecimal:
                value.IsHexadecimal().Should().Be(expected);
                span.IsHexadecimal().Should().Be(expected);
                readOnlySpan.IsHexadecimal().Should().Be(expected);
                memory.IsHexadecimal().Should().Be(expected);
                readOnlyMemory.IsHexadecimal().Should().Be(expected);
                break;
            default: throw new ArgumentOutOfRangeException(nameof(family), family, null);
        }
    }

    private static void AssertPortableBase64MatchesFramework(ReadOnlySpan<char> value)
    {
        var expected = Base64.IsValid(value);
        var actual = PortableBase64Validator(value);
        if (actual != expected)
        {
            throw new XunitException(
                $"Portable Base64 mismatch for '{value.ToString()}': expected {expected}, actual {actual}."
            );
        }
    }

    private static string InsertWhiteSpace(string value)
    {
        if (value.Length == 0)
        {
            return " \t\r\n";
        }

        var result = new char[value.Length * 2];
        for (var index = 0; index < value.Length; ++index)
        {
            result[index * 2] = value[index];
            result[index * 2 + 1] = (char) (index %
                                            4 switch
                                            {
                                                0 => ' ',
                                                1 => '\t',
                                                2 => '\r',
                                                _ => '\n',
                                            });
        }

        return new (result);
    }

    private static bool IsAcceptedByWhiteSpaceGuard(ReadOnlySpan<char> value)
    {
        try
        {
            value.MustNotContainWhiteSpace();
            return true;
        }
        catch (StringException)
        {
            return false;
        }
    }

    private static bool IsAsciiHexDigit(char value) =>
        value is >= '0' and <= '9' or >= 'A' and <= 'F' or >= 'a' and <= 'f';

    private static void AssertEqual(bool expected, bool actual, char value, string classification)
    {
        if (actual != expected)
        {
            throw new XunitException(
                $"UTF-16 code unit U+{(int) value:X4} had an unexpected {classification} classification: expected {expected}, actual {actual}."
            );
        }
    }

    private delegate bool Base64Validator(ReadOnlySpan<char> value);
}
