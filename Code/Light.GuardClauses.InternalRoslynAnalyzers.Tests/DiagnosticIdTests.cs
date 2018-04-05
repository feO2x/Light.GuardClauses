using System.Globalization;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.InternalRoslynAnalyzers.Tests
{
    public static class DiagnosticIdTests
    {
        [Theory]
        [InlineData("Light.GuardClauses.NonDefaultParameterName", false)] // No '.' character allowed
        [InlineData("LightGuardClauses-NonDefaultParameterName", false)]  // No '-' character allowed
        [InlineData("LightGuardClauses_NonDefaultParameterName", true)]   // Underscores are allowed
        public static void ValidDiagnosticIdentifiers(string identifier, bool expected) =>
            UnicodeCharacterUtilities.IsValidIdentifier(identifier).Should().Be(expected);
    }

    public static class UnicodeCharacterUtilities
    {
        /// <summary>
        /// Check that the name is a valid Unicode identifier.
        /// </summary>
        public static bool IsValidIdentifier(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (!IsIdentifierStartCharacter(name[0]))
                return false;

            var nameLength = name.Length;
            for (var i = 1; i < nameLength; i++) //NB: start at 1
            {
                if (!IsIdentifierPartCharacter(name[i]))
                    return false;
            }

            return true;
        }

        public static bool IsIdentifierStartCharacter(char ch)
        {
            // identifier-start-character:
            //   letter-character
            //   _ (the underscore character U+005F)

            if (ch < 'a') // '\u0061'
            {
                if (ch < 'A') // '\u0041'

                    return false;

                return ch <= 'Z' // '\u005A'
                       || ch == '_'; // '\u005F'
            }

            if (ch <= 'z') // '\u007A'

                return true;

            if (ch <= '\u007F') // max ASCII
                return false;

            return IsLetterChar(CharUnicodeInfo.GetUnicodeCategory(ch));
        }

        public static bool IsIdentifierPartCharacter(char ch)
        {
            // identifier-part-character:
            //   letter-character
            //   decimal-digit-character
            //   connecting-character
            //   combining-character
            //   formatting-character

            if (ch < 'a') // '\u0061'
            {
                if (ch < 'A') // '\u0041'
                    return ch >= '0' // '\u0030'
                           && ch <= '9'; // '\u0039'

                return ch <= 'Z' // '\u005A'
                       || ch == '_'; // '\u005F'
            }

            if (ch <= 'z') // '\u007A'
                return true;

            if (ch <= '\u007F') // max ASCII
                return false;

            var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
            return IsLetterChar(cat)
                   || IsDecimalDigitChar(cat)
                   || IsConnectingChar(cat)
                   || IsCombiningChar(cat)
                   || IsFormattingChar(cat);
        }

        private static bool IsLetterChar(UnicodeCategory cat)
        {
            // letter-character:
            //   A Unicode character of classes Lu, Ll, Lt, Lm, Lo, or Nl 
            //   A Unicode-escape-sequence representing a character of classes Lu, Ll, Lt, Lm, Lo, or Nl

            switch (cat)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.LetterNumber:
                    return true;
            }

            return false;
        }

        private static bool IsDecimalDigitChar(UnicodeCategory cat)
        {
            // decimal-digit-character:
            //   A Unicode character of the class Nd 
            //   A unicode-escape-sequence representing a character of the class Nd

            return cat == UnicodeCategory.DecimalDigitNumber;
        }

        private static bool IsConnectingChar(UnicodeCategory cat)
        {
            // connecting-character:  
            //   A Unicode character of the class Pc
            //   A unicode-escape-sequence representing a character of the class Pc

            return cat == UnicodeCategory.ConnectorPunctuation;
        }

        private static bool IsFormattingChar(UnicodeCategory cat)
        {
            // formatting-character:  
            //   A Unicode character of the class Cf
            //   A unicode-escape-sequence representing a character of the class Cf

            return cat == UnicodeCategory.Format;
        }

        private static bool IsCombiningChar(UnicodeCategory cat)
        {
            // combining-character:
            //   A Unicode character of classes Mn or Mc 
            //   A Unicode-escape-sequence representing a character of classes Mn or Mc

            switch (cat)
            {
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.SpacingCombiningMark:
                    return true;
            }

            return false;
        }
    }
}