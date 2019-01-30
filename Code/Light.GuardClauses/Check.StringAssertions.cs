using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;

#endif

namespace Light.GuardClauses
{
    public static partial class Check
    {
        /// <summary>
        /// Checks if the specified string is null or empty.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("=> false, string:notnull; => true, string:canbenull")]
        public static bool IsNullOrEmpty(this string @string) => string.IsNullOrEmpty(@string);

        /// <summary>
        /// Ensures that the specified string is not null or empty, or otherwise throws an <see cref="ArgumentNullException" /> or <see cref="EmptyStringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                Throw.ArgumentNull(parameterName, message);
            if (parameter.Length == 0)
                Throw.EmptyString(parameterName, message);

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified string is not null or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is an empty string or null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static string MustNotBeNullOrEmpty(this string parameter, Func<string, Exception> exceptionFactory)
        {
            if (string.IsNullOrEmpty(parameter))
                Throw.CustomException(exceptionFactory, parameter);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified string is null, empty, or contains only white space.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("=> false, string:notnull; => true, string:canbenull")]
        public static bool IsNullOrWhiteSpace(this string @string)
#if NET35 || NET35_CF
        {
            if (string.IsNullOrEmpty(@string))
                return true;

            foreach (var character in @string)
            {
                if (!char.IsWhiteSpace(character))
                    return false;
            }

            return true;
        }
#else
            => string.IsNullOrWhiteSpace(@string);
#endif

        /// <summary>
        /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws an <see cref="ArgumentNullException" />, an <see cref="EmptyStringException" />, or a <see cref="WhiteSpaceStringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="WhiteSpaceStringException">Thrown when <paramref name="parameter" /> contains only white space.</exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is an empty string.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotBeNullOrWhiteSpace(this string parameter, string parameterName = null, string message = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message);

            foreach (var character in parameter)
            {
                if (!char.IsWhiteSpace(character))
                    return parameter;
            }

            Throw.WhiteSpaceString(parameter, parameterName, message);
            return null;
        }

        /// <summary>
        /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null, empty, or contains only white space.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory: null => halt")]
        public static string MustNotBeNullOrWhiteSpace(this string parameter, Func<string, Exception> exceptionFactory)
        {
            if (parameter.IsNullOrWhiteSpace())
                Throw.CustomException(exceptionFactory, parameter);
            return null;
        }

        /// <summary>
        /// Checks if the specified character is a white space character.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsWhiteSpace(this char character) => char.IsWhiteSpace(character);

        /// <summary>
        /// Checks if the specified character is a letter.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsLetter(this char character) => char.IsLetter(character);

        /// <summary>
        /// Checks if the specified character is a letter or digit.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsLetterOrDigit(this char character) => char.IsLetterOrDigit(character);

        /// <summary>
        /// Checks if the specified character is a digit.
        /// </summary>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsDigit(this char character) => char.IsDigit(character);

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws a <see cref="ValuesNotEqualException" />.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustBe(this string parameter, string other, StringComparison comparisonType, string parameterName = null, string message = null)
        {
            if (!string.Equals(parameter, other, comparisonType))
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustBe(this string parameter, string other, StringComparison comparisonType, Func<string, string, Exception> exceptionFactory)
        {
            if (!string.Equals(parameter, other, comparisonType))
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws a <see cref="ValuesNotEqualException" />.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesNotEqualException">Thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustBe(this string parameter, string other, StringComparisonType comparisonType, string parameterName = null, string message = null)
        {
            if (!parameter.Equals(other, comparisonType))
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name="comparisonType" />, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
        public static string MustBe(this string parameter, string other, StringComparisonType comparisonType, Func<string, string, Exception> exceptionFactory)
        {
            if (!parameter.Equals(other, comparisonType))
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name="comparisonType" />, or otherwise throws a <see cref="ValuesEqualException" />.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesEqualException">Thrown when <paramref name="parameter" /> is equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustNotBe(this string parameter, string other, StringComparison comparisonType, string parameterName = null, string message = null)
        {
            if (string.Equals(parameter, other, comparisonType))
                Throw.ValuesEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name="comparisonType" />, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustNotBe(this string parameter, string other, StringComparison comparisonType, Func<string, string, Exception> exceptionFactory)
        {
            if (string.Equals(parameter, other, comparisonType))
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name="comparisonType" />, or otherwise throws a <see cref="ValuesEqualException" />.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValuesEqualException">Thrown when <paramref name="parameter" /> is equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustNotBe(this string parameter, string other, StringComparisonType comparisonType, string parameterName = null, string message = null)
        {
            if (parameter.Equals(other, comparisonType))
                Throw.ValuesEqual(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name="comparisonType" />, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The first string to be compared.</param>
        /// <param name="other">The second string to be compared.</param>
        /// <param name="comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="other" /> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustNotBe(this string parameter, string other, StringComparisonType comparisonType, Func<string, string, Exception> exceptionFactory)
        {
            if (parameter.Equals(other, comparisonType))
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string matches the specified regular expression, or otherwise throws a <see cref="StringDoesNotMatchException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="regex">The regular expression used for pattern matching.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="StringDoesNotMatchException">Thrown when <paramref name="parameter" /> does not match the specified regular expression.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="regex" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; regex:null => halt")]
        public static string MustMatch(this string parameter, Regex regex, string parameterName = null, string message = null)
        {
            if (!regex.MustNotBeNull(nameof(regex), message).IsMatch(parameter.MustNotBeNull(parameterName, message)))
                Throw.StringDoesNotMatch(parameter, regex, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string matches the specified regular expression, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="regex">The regular expression used for pattern matching.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="regex" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> does not match the specified regular expression,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="regex" /> is null.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static string MustMatch(this string parameter, Regex regex, Func<string, Regex, Exception> exceptionFactory)
        {
            if (parameter == null || regex == null || !regex.IsMatch(parameter))
                Throw.CustomException(exceptionFactory, parameter, regex);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified strings are equal, using the given comparison rules.
        /// </summary>
        /// <param name="string">The first string to compare.</param>
        /// <param name="value">The second string to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
        /// <returns>True if the two strings are considered equal, else false.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType"/> is no valid enum value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool Equals(this string @string, string value, StringComparisonType comparisonType)
        {
            if ((int) comparisonType < 6)
                return string.Equals(@string, value, (StringComparison) comparisonType);
            if (comparisonType == StringComparisonType.OrdinalIgnoreWhiteSpace)
                return @string.EqualsOrdinalIgnoreWhiteSpace(value);
            if (comparisonType == StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)
                return @string.EqualsOrdinalIgnoreCaseIgnoreWhiteSpace(value);

            Throw.EnumValueNotDefined(comparisonType, nameof(comparisonType));
            return false;
        }

        /// <summary>
        /// Ensures that the string contains the specified substring, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The substring that must be part of <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> does not contain <paramref name="value" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain(this string parameter, string value, string parameterName = null, string message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).Contains(value.MustNotBeNull(nameof(value), message)))
                Throw.StringDoesNotContain(parameter, value, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The substring that must be part of <paramref name="parameter" />.</param>
        /// <param name="exceptionFactory">The delegate that creates you custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="value" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain(this string parameter, string value, Func<string, string, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || !parameter.Contains(value))
                Throw.CustomException(exceptionFactory, parameter, value);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified value, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The substring that must be part of <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> does not contain <paramref name="value" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain(this string parameter, string value, StringComparison comparisonType, string parameterName = null, string message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IndexOf(value.MustNotBeNull(nameof(value), message), comparisonType) < 0)
                Throw.StringDoesNotContain(parameter, value, comparisonType, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The substring that must be part of <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="exceptionFactory">The delegate that creates you custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="value" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null,
        /// or when <paramref name="comparisonType" /> is not a valid value from the <see cref="StringComparison" /> enum.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain(this string parameter, string value, StringComparison comparisonType, Func<string, string, StringComparison, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || !comparisonType.IsValidEnumValue() || parameter.IndexOf(value, comparisonType) < 0)
                Throw.CustomException(exceptionFactory, parameter, value, comparisonType);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> contains <paramref name="value" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain(this string parameter, string value, string parameterName = null, string message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Contains(value.MustNotBeNull(nameof(value), message)))
                Throw.StringContains(parameter, value, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception (optional). <paramref name="parameter" /> and <paramref name="value" /> are passed to this </param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="value" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain(this string parameter, string value, Func<string, string, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || parameter.Contains(value))
                Throw.CustomException(exceptionFactory, parameter, value);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="parameter" /> contains <paramref name="value" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain(this string parameter, string value, StringComparison comparisonType, string parameterName = null, string message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IndexOf(value.MustNotBeNull(nameof(value), message), comparisonType) >= 0)
                Throw.StringContains(parameter, value, comparisonType, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The string that must not be part of <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception (optional). <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="value" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null,
        /// or when <paramref name="comparisonType" /> is not a valid value of the <see cref="StringComparison" /> enum.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain(this string parameter, string value, StringComparison comparisonType, Func<string, string, StringComparison, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || !comparisonType.IsValidEnumValue() || parameter.IndexOf(value, comparisonType) >= 0)
                Throw.CustomException(exceptionFactory, parameter, value, comparisonType);
            return parameter;
        }

        /// <summary>
        /// Checks if the string contains the specified value using the given comparison type.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
        /// <param name="value">The other string.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if <paramref name="string" /> contains <paramref name="value" />, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="string" /> or <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("string:null => halt; value:null => halt")]
        public static bool Contains(this string @string, string value, StringComparison comparisonType) =>
            @string.MustNotBeNull(nameof(@string)).IndexOf(value, comparisonType) >= 0;

        /// <summary>
        /// Checks if the string is a substring of the other string.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <param name="other">The other string.</param>
        /// <returns>True if <paramref name="value" /> is a substring of <paramref name="other" />, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> or <paramref name="other" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("value:null => halt; other:null => halt")]
        public static bool IsSubstringOf(this string value, string other) =>
            other.MustNotBeNull(nameof(other)).Contains(value);

        /// <summary>
        /// Checks if the string is a substring of the other string.
        /// </summary>
        /// <param name="value">The string to be checked.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if <paramref name="value" /> is a substring of <paramref name="other" />, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> or <paramref name="other" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("value:null => halt; other:null => halt")]
        public static bool IsSubstringOf(this string value, string other, StringComparison comparisonType) =>
            other.MustNotBeNull(nameof(other)).IndexOf(value, comparisonType) != -1;


        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must contain <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="value" /> does not contain <paramref name="parameter" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf(this string parameter, string value, string parameterName = null, string message = null)
        {
            if (!value.MustNotBeNull(nameof(value), message).Contains(parameter.MustNotBeNull(parameterName, message)))
                Throw.NotSubstring(parameter, value, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must contain <paramref name="parameter" />.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="value" /> does not contain <paramref name="parameter" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf(this string parameter, string value, Func<string, string, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || !value.Contains(parameter))
                Throw.CustomException(exceptionFactory, parameter, value);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must contain <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="value" /> does not contain <paramref name="parameter" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf(this string parameter, string value, StringComparison comparisonType, string parameterName = null, string message = null)
        {
            if (value.MustNotBeNull(nameof(value), message).IndexOf(parameter.MustNotBeNull(parameterName, message), comparisonType) == -1)
                Throw.NotSubstring(parameter, value, comparisonType, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must contain <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" />, <paramref name="value" />, and <paramref name="comparisonType" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="value" /> does not contain <paramref name="parameter" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null,
        /// or when <paramref name="comparisonType" /> is not a valid value of the <see cref="StringComparison" /> enum.
        /// </exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf(this string parameter, string value, StringComparison comparisonType, Func<string, string, StringComparison, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || !comparisonType.IsValidEnumValue() || value.IndexOf(parameter, comparisonType) == -1)
                Throw.CustomException(exceptionFactory, parameter, value, comparisonType);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="value" /> contains <paramref name="parameter" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf(this string parameter, string value, string parameterName = null, string message = null)
        {
            if (value.MustNotBeNull(nameof(value), message).Contains(parameter.MustNotBeNull(parameterName, message)))
                Throw.Substring(parameter, value, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="value" /> contains <paramref name="parameter" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf(this string parameter, string value, Func<string, string, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || value.Contains(parameter))
                Throw.CustomException(exceptionFactory, parameter, value);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref="SubstringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="SubstringException">Thrown when <paramref name="value" /> contains <paramref name="parameter" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="value" /> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf(this string parameter, string value, StringComparison comparisonType, string parameterName = null, string message = null)
        {
            if (value.MustNotBeNull(nameof(value), message).IndexOf(parameter.MustNotBeNull(nameof(parameter), message), comparisonType) != -1)
                Throw.Substring(parameter, value, comparisonType, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="value">The other string that must not contain <paramref name="parameter" />.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="value" /> are passed to this delegate.</param>
        /// <exception cref="Exception">
        /// Your custom exception thrown when <paramref name="value" /> contains <paramref name="parameter" />,
        /// or when <paramref name="parameter" /> is null,
        /// or when <paramref name="value" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf(this string parameter, string value, StringComparison comparisonType, Func<string, string, StringComparison, Exception> exceptionFactory)
        {
            if (parameter == null || value == null || !comparisonType.IsValidEnumValue() || value.IndexOf(parameter, comparisonType) != -1)
                Throw.CustomException(exceptionFactory, parameter, value, comparisonType);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified string is an email address.
        ///
        /// For more information about mail address patterns see <see cref="!:https://emailregex.com/">here</see> 
        /// </summary>
        /// <param name="emailAddress">The string to be checked if it is an email address.</param>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsEmailAddress(this string emailAddress)
        {
            if (emailAddress == null) return false;

            var regex = new Regex(
                @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((((\w+\-?)+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
                RegexOptions.CultureInvariant | RegexOptions.ECMAScript
            );

            return regex.IsMatch(emailAddress);
        }
    }
}