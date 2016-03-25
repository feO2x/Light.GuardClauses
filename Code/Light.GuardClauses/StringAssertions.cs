using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The StringAssertions class contains extension methods that make assertions on <see cref="string" /> instances.
    /// </summary>
    public static class StringAssertions
    {
        /// <summary>
        ///     Ensures that the specified string is not null or empty, or otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNullException" /> or <see cref="EmptyStringException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is either null or empty (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter == null)
                throw exception != null ? exception() : new ArgumentNullException(parameterName, message);

            if (parameter == string.Empty)
                throw exception != null ? exception() : (message == null ? new EmptyStringException(parameterName) : new EmptyStringException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that the specified string is not null, empty or contains only whitespace, or otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNullException" />, or the <see cref="EmptyStringException" />, or  the <see cref="StringIsOnlyWhiteSpaceException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is either null, empty, or whitespace (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="StringIsOnlyWhiteSpaceException">
        ///     Thrown when <paramref name="parameter" /> contains only whitespace and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrWhiteSpace(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message, exception);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in parameter)
            {
                if (char.IsWhiteSpace(character) == false)
                    return;
            }
            throw exception != null ? exception() : (message == null ? StringIsOnlyWhiteSpaceException.CreateDefault(parameterName, parameter) : new StringIsOnlyWhiteSpaceException(message, parameterName));
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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="pattern" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustMatch(this string parameter, Regex pattern, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            pattern.MustNotBeNull(nameof(pattern), "You called MustMatch wrongly by specifying null to pattern.");

            var match = pattern.Match(parameter);
            if (match.Success == false)
                throw exception != null ? exception() : (message == null ? new StringDoesNotMatchException(parameterName, parameter, pattern) : new StringDoesNotMatchException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> contains the specified text, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="text">The text that must be contained in <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="ignoreCaseSensitivity">
        ///     The value indicating whether the two strings should be compared without regarding case-sensitivity (defaults to false).
        /// </param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="StringException" /> or <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not contain the specified text (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> does not contain the specified text and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        ///     or
        ///     Thrown when <paramref name="text" /> is null.
        /// </exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="text" /> is an empty string.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContain(this string parameter, string text, string parameterName = null, bool ignoreCaseSensitivity = false, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);
            text.MustNotBeNullOrEmpty(nameof(text), $"You called MustContain wrongly by specifying {(text == null ? "null" : "an empty string")} for text.");

            if (ignoreCaseSensitivity)
            {
                parameter = parameter.ToLower();
                // ReSharper disable once PossibleNullReferenceException
                text = text.ToLower();
            }

            if (parameter.Contains(text) == false)
                throw exception != null ? exception() : new StringException(message ?? $"{parameterName ?? "The string"} must contain the text \"{text}\", but you specified \"{parameter}\".", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> does not contain the specified text, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="text">The text that should not be part of <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="ignoreCaseSensitivity">
        ///     The value indicating whether the two strings should be compared without regarding case-sensitivity (defaults to false).
        /// </param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="StringException" /> or <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does contain the specified text (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> contains <paramref name="text" /> an no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified or Thrown when <paramref name="text" /> is null.
        /// </exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="text" /> is an empty string.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContain(this string parameter, string text, string parameterName = null, bool ignoreCaseSensitivity = false, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);
            text.MustNotBeNullOrEmpty(nameof(text), $"You called MustNotContain wrongly by specifying {(text == null ? "null" : "an empty string")} for text.");

            if (ignoreCaseSensitivity)
            {
                parameter = parameter.ToLower();
                // ReSharper disable once PossibleNullReferenceException
                text = text.ToLower();
            }

            if (parameter.Contains(text))
                throw exception != null ? exception() : new StringException(message ?? $"{parameterName ?? "The string"} must not contain the text \"{text}\", but you specified \"{parameter}\".", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is a substring of <paramref name="text" />, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="text">The text that <paramref name="parameter" /> is compared against.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="ignoreCaseSensitivity">
        ///     The value indicating whether the two strings should be compared without regarding case-sensitivity (defaults to false).
        /// </param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="StringException" /> or <see cref="ArgumentNullException" /> for <paramref name="parameter" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no substring of the specified text (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> is not a substring of <paramref name="text" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="text" /> is null or Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="text" /> is an empty string or Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeSubstringOf(this string parameter, string text, string parameterName = null, bool ignoreCaseSensitivity = false, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message, exception);
            text.MustNotBeNullOrEmpty(nameof(text), $"You called MustBeSubstringOf wrongly by specifying {(text == null ? "null" : "an empty string")} for text.");

            if (ignoreCaseSensitivity)
            {
                parameter = parameter.ToLower();
                // ReSharper disable once PossibleNullReferenceException
                text = text.ToLower();
            }

            // ReSharper disable once PossibleNullReferenceException
            if (text.Contains(parameter) == false)
                throw exception != null ? exception() : new StringException(message ?? $"{parameterName ?? "The string"} must be a substring of \"{text}\", but you specified \"{parameter}\".", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is not a substring of <paramref name="text" />, or otherwise throws a <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="text">The text <paramref name="parameter" /> is compared with.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="ignoreCaseSensitivity">
        ///     The value indicating whether the two strings should be compared without regarding case-sensitivity (defaults to false).
        /// </param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="StringException" /> or <see cref="ArgumentNullException" /> for <paramref name="parameter" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a substring of the specified text (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> is a substring of <paramref name="text" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="text" /> is null or Thrown when <paramref name="parameter" /> is null and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="text" /> is an empty string or Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeSubstringOf(this string parameter, string text, string parameterName = null, bool ignoreCaseSensitivity = false, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameter, message, exception);
            text.MustNotBeNullOrEmpty(nameof(text), $"You called MustNotBeSubstringOf wrongly by specifying {(text == null ? "null" : "an empty string")} for text.");

            if (ignoreCaseSensitivity)
            {
                parameter = parameter.ToLower();
                // ReSharper disable once PossibleNullReferenceException
                text = text.ToLower();
            }

            // ReSharper disable once PossibleNullReferenceException
            if (text.Contains(parameter))
                throw exception != null ? exception() : new StringException(message ?? $"{parameterName ?? "The string"} must not be a substring of \"{text}\", but you specified \"{parameter}\".", parameterName);
        }
    }
}