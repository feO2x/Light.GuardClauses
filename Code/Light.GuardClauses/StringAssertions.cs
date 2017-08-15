using System;
using System.Text.RegularExpressions;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The <see cref="StringAssertions" /> class contains extension methods that perform assertions on <see cref="string" /> instances.
    /// </summary>
    public static class StringAssertions
    {
        /// <summary>
        ///     Ensures that the specified string is not null or empty, or otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="EmptyStringException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is either null or empty (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        public static string MustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter == string.Empty)
                throw exception?.Invoke() ?? (message == null ? new EmptyStringException(parameterName) : new EmptyStringException(message, parameterName));

            return parameter;
        }

        /// <summary>
        ///     Checks if the specified string is null or empty.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <returns>True if the string is null or empty, else false.</returns>
        public static bool IsNullOrEmpty(this string parameter)
        {
            return string.IsNullOrEmpty(parameter);
        }

        /// <summary>
        ///     Ensures that the specified string is not null, empty or contains only whitespace, or otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the the <see cref="EmptyStringException" /> or  the <see cref="StringIsOnlyWhiteSpaceException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is empty, or whitespace (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="StringIsOnlyWhiteSpaceException">
        ///     Thrown when <paramref name="parameter" /> contains only whitespace and no <paramref name="exception" /> is specified.
        /// </exception>
        public static string MustNotBeNullOrWhiteSpace(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message, exception);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in parameter)
            {
                if (char.IsWhiteSpace(character) == false)
                    return parameter;
            }
            throw exception?.Invoke() ?? (message == null ? StringIsOnlyWhiteSpaceException.CreateDefault(parameterName, parameter) : new StringIsOnlyWhiteSpaceException(message, parameterName));
        }

        /// <summary>
        ///     Checks if the specified string is null, empty, or contains only white space characters.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <returns>True if the string is either null, or an empty string, or if it contains only white space characters.</returns>
        public static bool IsNullOrWhiteSpace(this string parameter)
        {
            return string.IsNullOrWhiteSpace(parameter);
        }

        /// <summary>
        ///     Checks if the specified string is equivalent to the other string. This is done by calling <see cref="string.Equals(string, string, StringComparison)" />
        ///     with comparison type <see cref="StringComparison.OrdinalIgnoreCase" />. If you are not satisfied with this default behavior, then provide
        ///     your own <paramref name="comparisonType" /> value.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparisonType">
        ///     The value indicating how strings are compared (optional). The default value is <see cref="StringComparison.OrdinalIgnoreCase" /> to use the
        ///     culture independent character sorting rules and ignore casing.
        /// </param>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="comparisonType" /> is no valid enum value.</exception>
        /// <returns>True if both strings are null, or equivalent according to the <paramref name="comparisonType" />.</returns>
        public static bool IsEquivalentTo(this string @string, string other, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            comparisonType.MustBeValidEnumValue(nameof(comparisonType));

            return string.Equals(@string, other, comparisonType);
        }

        /// <summary>
        ///     Ensures that the string is equivalent to the specified other string. This is done by calling <see cref="string.Equals(string, string, StringComparison)" />
        ///     with comparison type <see cref="StringComparison.OrdinalIgnoreCase" />. If you are not satisfied with this default behavior, then provide
        ///     your own <paramref name="comparisonType" /> value.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparisonType">
        ///     The value indicating how strings are compared (optional). The default value is <see cref="StringComparison.OrdinalIgnoreCase" /> to use the
        ///     culture independent character sorting rules and ignore casing.
        /// </param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not equivalent to <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not equivalent to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="other" /> is null.</exception>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="comparisonType" /> is no valid enum value.</exception>
        public static string MustBeEquivalentTo(this string parameter, string other, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            other.MustNotBeNull(nameof(other));

            if (parameter.IsEquivalentTo(other, comparisonType)) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must be equivalent to \"{other}\" (using {comparisonType}), but it is not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string is not equivalent to the specified other string. This is done by calling <see cref="string.Equals(string, string, StringComparison)" />
        ///     with comparison type <see cref="StringComparison.OrdinalIgnoreCase" />. If you are not satisfied with this default behavior, then provide
        ///     your own <paramref name="comparisonType" /> value.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="other">The other string.</param>
        /// <param name="comparisonType">
        ///     The value indicating how strings are compared (optional). The default value is <see cref="StringComparison.OrdinalIgnoreCase" /> to use the
        ///     culture independent character sorting rules and ignore casing.
        /// </param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is equivalent to <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is not equivalent to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="other" /> is null.</exception>
        /// <exception cref="EnumValueNotDefinedException">Thrown when <paramref name="comparisonType" /> is no valid enum value.</exception>
        public static string MustNotBeEquivalentTo(this string parameter, string other, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            other.MustNotBeNull(nameof(other));

            if (parameter.IsEquivalentTo(other, comparisonType) == false) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must not be equivalent to \"{other}\" (using {comparisonType}), but it is.", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> matches the specified regular expression, or otherwise throws an <see cref="StringDoesNotMatchException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="pattern">The regular expression used to evaluate <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringDoesNotMatchException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not match the <paramref name="pattern" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringDoesNotMatchException">
        ///     Thrown when <paramref name="parameter" /> does not match the <paramref name="pattern" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="pattern" /> is null.</exception>
        public static string MustMatch(this string parameter, Regex pattern, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            pattern.MustNotBeNull(nameof(pattern));

            if (pattern.IsMatch(parameter))
                return parameter;

            throw exception?.Invoke() ?? (message == null ? new StringDoesNotMatchException(parameterName, parameter, pattern) : new StringDoesNotMatchException(message, parameterName));
        }

        /// <summary>
        ///     Checks if the specified string matches the given regular expression.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
        /// <param name="pattern">The regular expression the string is checked against.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public static bool IsMatching(this string @string, Regex pattern)
        {
            @string.MustNotBeNull(nameof(@string));
            pattern.MustNotBeNull(nameof(pattern));

            return pattern.IsMatch(@string);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified <paramref name="text" /> occurs within the string.
        /// </summary>
        /// <param name="string">The string to be checked.</param>
        /// <param name="text">The text that should be contained within <paramref name="string" />.</param>
        /// <param name="ignoreCase">
        ///     The value indicating whether case sensitivity is ignored or active during search (optional).
        ///     By default, the search is performed case-sensitive. You can also specify a simple boolean value here
        ///     to turn on case-insensitivity, as there is a implicit conversion from <see cref="bool" /> to <see cref="IgnoreCaseInfo" />.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="string" /> or <paramref name="text" /> is null.</exception>
        public static bool IsContaining(this string @string, string text, IgnoreCaseInfo ignoreCase = default(IgnoreCaseInfo))
        {
            @string.MustNotBeNull(nameof(@string));
            text.MustNotBeNull(nameof(text));

            return ignoreCase.StringContains(@string, text);
        }

        /// <summary>
        ///     Ensures that the string contains the specified text, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should be contained in the string.</param>
        /// <param name="ignoreCase">
        ///     The value indicating whether case sensitivity is ignored or active during search (optional).
        ///     By default, the search is performed case-sensitive. You can also specify a simple boolean value here
        ///     to turn on case-insensitivity, as there is a implicit conversion from <see cref="bool" /> to <see cref="IgnoreCaseInfo" />.
        /// </param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not contain <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> does not contain <paramref name="text" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustContain(this string parameter, string text, IgnoreCaseInfo ignoreCase = default(IgnoreCaseInfo), string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.IsContaining(text, ignoreCase)) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must contain \"{text}\", but it does not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string does not contain the specified text, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should not be contained in the string.</param>
        /// <param name="ignoreCase">
        ///     The value indicating whether case sensitivity is ignored or active during search (optional).
        ///     By default, the search is performed case-sensitive. You can also specify a simple boolean value here
        ///     to turn on case-insensitivity, as there is a implicit conversion from <see cref="bool" /> to <see cref="IgnoreCaseInfo" />.
        /// </param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> contains <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> contains <paramref name="text" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustNotContain(this string parameter, string text, IgnoreCaseInfo ignoreCase = default(IgnoreCaseInfo), string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.IsContaining(text, ignoreCase) == false) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must not contain \"{text}\", but it does.", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is a substring of <paramref name="text" />, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="text">The text that <paramref name="parameter" /> is compared against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="ignoreCase">
        ///     The value indicating whether case sensitivity is ignored or active during search (optional).
        ///     By default, the search is performed case-sensitive. You can also specify a simple boolean value here
        ///     to turn on case-insensitivity, as there is a implicit conversion from <see cref="bool" /> to <see cref="IgnoreCaseInfo" />.
        /// </param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="StringException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no substring of the specified text (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> is not a substring of <paramref name="text" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.
        /// </exception>
        public static string MustBeSubstringOf(this string parameter, string text, IgnoreCaseInfo ignoreCase = default(IgnoreCaseInfo), string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (text.IsContaining(parameter, ignoreCase)) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must be a substring of \"{text}\", but it is not.", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is not a substring of <paramref name="text" />, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="text">The text <paramref name="parameter" /> is compared with.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="ignoreCase">
        ///     The value indicating whether case sensitivity is ignored or active during search (optional).
        ///     By default, the search is performed case-sensitive. You can also specify a simple boolean value here
        ///     to turn on case-insensitivity, as there is a implicit conversion from <see cref="bool" /> to <see cref="IgnoreCaseInfo" />.
        /// </param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="StringException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a substring of the specified text (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> is a substring of <paramref name="text" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustNotBeSubstringOf(this string parameter, string text, IgnoreCaseInfo ignoreCase = default(IgnoreCaseInfo), string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameter);
            text.MustNotBeNullOrEmpty(nameof(text));

            if (text.IsContaining(parameter, ignoreCase) == false) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must not be a substring of \"{text}\", but it is.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string has the specified length, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="length">The length that the string should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> has not the specified <paramref name="length" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> does not hat the specified <paramref name="length" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="length" /> is less than zero.</exception>
        public static string MustHaveLength(this string parameter, int length, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            length.MustNotBeLessThan(0, nameof(length));

            if (parameter.Length == length)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must have a length of {length}, but it actually has a length of {parameter.Length}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string starts with the specified text (case-sensitivitiy is respected), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should be at the beginning of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not start with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> does not start with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustStartWith(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.StartsWith(text, StringComparison.CurrentCulture))
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must start with \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string starts with the specified text (case-sensitivitiy is ignored), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should be at the beginning of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not start with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> does not start with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustStartWithEquivalentOf(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must start with the equivalent of \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string does not start with the specified text (case-sensitivity is respected), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should not be at the beginning of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> starts with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> starts with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustNotStartWith(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.StartsWith(text, StringComparison.CurrentCulture) == false)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must not start with \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string does not start with the specified text (case-sensitivitiy is ignored), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should not be at the beginning of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> starts with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> starts with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustNotStartWithEquivalentOf(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.StartsWith(text, StringComparison.CurrentCultureIgnoreCase) == false)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must not start with the equivalent of \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string ends with the specified text (case-sensitivity is respected), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should be at the end of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not end with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> does not end with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustEndWith(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.EndsWith(text, StringComparison.CurrentCulture))
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must end with \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string ends with the specified text (case-sensitivity is ignored), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should be at the end of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not end with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> does not end with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustEndWithEquivalentOf(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.EndsWith(text, StringComparison.CurrentCultureIgnoreCase))
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must end with the equivalent of \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string does not end with the specified text (case-sensitivity is respected), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should not be at the end of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> ends with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> ends with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustNotEndWith(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.EndsWith(text, StringComparison.CurrentCulture) == false)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must not end with \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string does not end with the specified text (case-sensitivity is ignored), or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="text">The text that should not be at the end of the string.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> ends with <paramref name="text" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> ends with <paramref name="text" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="text" /> is null.</exception>
        public static string MustNotEndWithEquivalentOf(this string parameter, string text, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.EndsWith(text, StringComparison.CurrentCultureIgnoreCase) == false)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must not end with equivalent of \"{text}\", but you specified {parameter}.", parameterName);
        }

        /// <summary>
        ///     Ensures that the specified string contains only letters, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> contains other characters than letters (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> contains other characters than letters.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is empty.</exception>
        public static string MustContainOnlyLetters(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName);

            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < parameter.Length; i++)
            {
                if (char.IsLetter(parameter[i]) == false)
                    throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must contain only letters, but you specified \"{parameter}\".", parameterName);
            }
            return parameter;
        }

        /// <summary>
        ///     Checks if the specified string contains only characters (using <see cref="char.IsLetter(char)" />
        ///     Empty strings or null will return false.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <returns>True if the string is not null or empty and contains only characters, else false.</returns>
        public static bool ContainsOnlyLetters(this string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
                return false;

            for (var i = 0; i < parameter.Length; i++)
            {
                if (char.IsLetter(parameter[i]) == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     Ensures that the specified string contains only letters and digits, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> contains other characters than letters or digits (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> contains other characters than letters or digits.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="parameter" /> is empty.</exception>
        public static string MustContainOnlyLettersAndDigits(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName);

            // ReSharper disable once LoopCanBeConvertedToQuery
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < parameter.Length; i++)
            {
                if (char.IsLetterOrDigit(parameter[i]) == false)
                    throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} must contain only letters or digits, but you specified \"{parameter}\".", parameterName);
            }

            return parameter;
        }

        /// <summary>
        ///     Checks if the specified string contains only upper- and lowercase characters as well as digits.
        ///     Empty strings or null will return false.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <returns>True if the string is not null or empty and contains only uppercase, lowercase, or digit characters, else false.</returns>
        public static bool ContainsOnlyLettersAndDigits(this string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
                return false;

            for (var i = 0; i < parameter.Length; i++)
            {
                if (char.IsLetterOrDigit(parameter[i]) == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     Ensures that the string has at least the specified length, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="minimumLength">The minimnum length that the string should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is shorter than the specified length (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you
        ///     specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is shorter than the specified length.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minimumLength" /> is less than zero.</exception>
        public static string MustHaveMinimumLength(this string parameter, int minimumLength, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            minimumLength.MustNotBeLessThan(0, nameof(minimumLength));

            if (parameter.Length >= minimumLength)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} \"{parameter}\" must have a minimum length of {minimumLength}, but it actually is only {parameter.Length} characters long.", parameterName);
        }

        /// <summary>
        ///     Ensures that the string has at most the specified length, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The string to be checked.</param>
        /// <param name="maximumLength">The maximum length that the string should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is longer than specified length (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you
        ///     specify exception.
        /// </param>
        /// <exception cref="StringException">Thrown when <paramref name="parameter" /> is longer than the specified length.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maximumLength" /> is less than zero.</exception>
        public static string MustHaveMaximumLength(this string parameter, int maximumLength, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            maximumLength.MustNotBeLessThan(0, nameof(maximumLength));

            if (parameter.Length <= maximumLength)
                return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"{parameterName ?? "The string"} \"{parameter}\" must have a maximum length of {maximumLength}, but it actually is {parameter.Length} characters long.", parameterName);
        }
    }
}