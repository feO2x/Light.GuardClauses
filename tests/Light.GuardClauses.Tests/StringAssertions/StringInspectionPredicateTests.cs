using System;
using System.Buffers.Text;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class StringInspectionPredicateTests
{
    private static readonly Base64Validator PortableBase64Validator =
        typeof(Check).GetMethod("IsBase64Portable", BindingFlags.NonPublic | BindingFlags.Static)!
                     .CreateDelegate<Base64Validator>();

    [Fact]
    public static void NullAndEmptyInputsHaveTheDefinedSemantics()
    {
        string nullText = null;

        nullText.ContainsOnlyDigits().Should().BeFalse();
        nullText.ContainsOnlyLettersOrDigits().Should().BeFalse();
        nullText.IsUpperCase().Should().BeFalse();
        nullText.IsLowerCase().Should().BeFalse();
        nullText.IsBase64().Should().BeFalse();
        nullText.IsHexadecimal().Should().BeFalse();

        string.Empty.ContainsOnlyDigits().Should().BeTrue();
        string.Empty.ContainsOnlyLettersOrDigits().Should().BeTrue();
        string.Empty.IsUpperCase().Should().BeTrue();
        string.Empty.IsLowerCase().Should().BeTrue();
        string.Empty.IsBase64().Should().BeTrue();
        string.Empty.IsHexadecimal().Should().BeTrue();

        Span<char>.Empty.ContainsOnlyDigits().Should().BeTrue();
        ReadOnlySpan<char>.Empty.ContainsOnlyLettersOrDigits().Should().BeTrue();
        Memory<char>.Empty.IsUpperCase().Should().BeTrue();
        ReadOnlyMemory<char>.Empty.IsLowerCase().Should().BeTrue();
        Span<char>.Empty.IsBase64().Should().BeTrue();
        ReadOnlyMemory<char>.Empty.IsHexadecimal().Should().BeTrue();
    }

    [Fact]
    public static void EveryPredicateSupportsEveryReceiverShape()
    {
        var digits = "1٢３".ToCharArray();
        var lettersOrDigits = "AzΩ٢".ToCharArray();
        var upperCase = "AΩ 123!".ToCharArray();
        var lowerCase = "aω 123!".ToCharArray();
        var base64 = "T W\tF\ru\n".ToCharArray();
        var hexadecimal = "09aAfF".ToCharArray();

        digits.AsSpan().ContainsOnlyDigits().Should().BeTrue();
        ((ReadOnlySpan<char>) digits).ContainsOnlyDigits().Should().BeTrue();
        digits.AsMemory().ContainsOnlyDigits().Should().BeTrue();
        ((ReadOnlyMemory<char>) digits).ContainsOnlyDigits().Should().BeTrue();

        lettersOrDigits.AsSpan().ContainsOnlyLettersOrDigits().Should().BeTrue();
        ((ReadOnlySpan<char>) lettersOrDigits).ContainsOnlyLettersOrDigits().Should().BeTrue();
        lettersOrDigits.AsMemory().ContainsOnlyLettersOrDigits().Should().BeTrue();
        ((ReadOnlyMemory<char>) lettersOrDigits).ContainsOnlyLettersOrDigits().Should().BeTrue();

        upperCase.AsSpan().IsUpperCase().Should().BeTrue();
        ((ReadOnlySpan<char>) upperCase).IsUpperCase().Should().BeTrue();
        upperCase.AsMemory().IsUpperCase().Should().BeTrue();
        ((ReadOnlyMemory<char>) upperCase).IsUpperCase().Should().BeTrue();

        lowerCase.AsSpan().IsLowerCase().Should().BeTrue();
        ((ReadOnlySpan<char>) lowerCase).IsLowerCase().Should().BeTrue();
        lowerCase.AsMemory().IsLowerCase().Should().BeTrue();
        ((ReadOnlyMemory<char>) lowerCase).IsLowerCase().Should().BeTrue();

        base64.AsSpan().IsBase64().Should().BeTrue();
        ((ReadOnlySpan<char>) base64).IsBase64().Should().BeTrue();
        base64.AsMemory().IsBase64().Should().BeTrue();
        ((ReadOnlyMemory<char>) base64).IsBase64().Should().BeTrue();

        hexadecimal.AsSpan().IsHexadecimal().Should().BeTrue();
        ((ReadOnlySpan<char>) hexadecimal).IsHexadecimal().Should().BeTrue();
        hexadecimal.AsMemory().IsHexadecimal().Should().BeTrue();
        ((ReadOnlyMemory<char>) hexadecimal).IsHexadecimal().Should().BeTrue();
    }

    [Theory]
    [InlineData("x12", "1x2", "12x")]
    public static void InvalidContentIsDetectedAtTheBeginningMiddleAndEnd(string beginning, string middle, string end)
    {
        beginning.ContainsOnlyDigits().Should().BeFalse();
        middle.ContainsOnlyDigits().Should().BeFalse();
        end.ContainsOnlyDigits().Should().BeFalse();

        "-Az".ContainsOnlyLettersOrDigits().Should().BeFalse();
        "A-z".ContainsOnlyLettersOrDigits().Should().BeFalse();
        "Az-".ContainsOnlyLettersOrDigits().Should().BeFalse();

        "aAB".IsUpperCase().Should().BeFalse();
        "AaB".IsUpperCase().Should().BeFalse();
        "ABa".IsUpperCase().Should().BeFalse();

        "Aab".IsLowerCase().Should().BeFalse();
        "aAb".IsLowerCase().Should().BeFalse();
        "abA".IsLowerCase().Should().BeFalse();

        "x09A".IsHexadecimal().Should().BeFalse();
        "0x9A".IsHexadecimal().Should().BeFalse();
        "09Ax".IsHexadecimal().Should().BeFalse();
    }

    [Fact]
    public static void CasingAllowsUncasedContentAndUsesUnicodeClassification()
    {
        "123 !\t中".IsUpperCase().Should().BeTrue();
        "123 !\t中".IsLowerCase().Should().BeTrue();
        "ÄΩ".IsUpperCase().Should().BeTrue();
        "äω".IsLowerCase().Should().BeTrue();
        "Äω".IsUpperCase().Should().BeFalse();
        "äΩ".IsLowerCase().Should().BeFalse();
    }

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
        const string corpusCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=_- \t\r\n\v\u00A0!";

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
            AssertEqual(char.IsDigit(character), ((ReadOnlySpan<char>) value).ContainsOnlyDigits(), character, "digit");
            AssertEqual(char.IsLetterOrDigit(character), ((ReadOnlySpan<char>) value).ContainsOnlyLettersOrDigits(), character, "letter or digit");
            AssertEqual(!char.IsLower(character), ((ReadOnlySpan<char>) value).IsUpperCase(), character, "upper case");
            AssertEqual(!char.IsUpper(character), ((ReadOnlySpan<char>) value).IsLowerCase(), character, "lower case");
            AssertEqual(IsAsciiHexDigit(character), ((ReadOnlySpan<char>) value).IsHexadecimal(), character, "hexadecimal");
            AssertEqual(!char.IsWhiteSpace(character), IsAcceptedByWhiteSpaceGuard(value), character, "white space");
        }
    }

    private static void AssertPortableBase64MatchesFramework(ReadOnlySpan<char> value)
    {
        var expected = Base64.IsValid(value);
        var actual = PortableBase64Validator(value);
        if (actual != expected)
        {
            throw new XunitException($"Portable Base64 mismatch for '{value.ToString()}': expected {expected}, actual {actual}.");
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
            result[index * 2 + 1] = (char) (index % 4 switch
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
