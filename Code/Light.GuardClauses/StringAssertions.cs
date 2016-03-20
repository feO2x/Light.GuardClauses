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
        ///     The message that will be injected into the <see cref="ArgumentNullException" /> or
        ///     <see cref="EmptyStringException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is either null or empty (optional). Please
        ///     note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify
        ///     exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null, Exception exception = null)
        {
            if (parameter == null)
                throw exception ?? new ArgumentNullException(parameterName, message);

            if (parameter == string.Empty)
                throw exception ?? (message == null ? new EmptyStringException(parameterName) : new EmptyStringException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that the specified string is not null, empty or contains only whitespace, or otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="ArgumentNullException" />, or
        ///     the <see cref="EmptyStringException" />, or  the <see cref="StringIsOnlyWhiteSpaceException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is either null, empty, or
        ///     whitespace (optional). Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both
        ///     ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="EmptyStringException">
        ///     Thrown when <paramref name="parameter" /> is empty and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="StringIsOnlyWhiteSpaceException">
        ///     Thrown when <paramref name="parameter" /> contains only whitespace and no <paramref name="exception" /> is
        ///     specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrWhiteSpace(this string parameter, string parameterName = null, string message = null, Exception exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message, exception);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in parameter)
            {
                if (char.IsWhiteSpace(character) == false)
                    return;
            }
            throw exception ?? (message == null ? StringIsOnlyWhiteSpaceException.CreateDefault(parameterName, parameter) : new StringIsOnlyWhiteSpaceException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> matches the specified regular expression, or otherwise throws an
        ///     <see cref="StringDoesNotMatchException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="pattern">The regular expression used to evaluate <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="StringDoesNotMatchException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not match the
        ///     <paramref name="pattern" /> (optional). Please note that <paramref name="message" /> and
        ///     <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringDoesNotMatchException">
        ///     Thrown when <paramref name="parameter" /> does not match the
        ///     <paramref name="pattern" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="pattern" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustMatch(this string parameter, Regex pattern, string parameterName = null, string message = null, Exception exception = null)
        {
            pattern.MustNotBeNull(nameof(pattern), "You called MustMatch wrongly by specifying null to pattern.");

            var match = pattern.Match(parameter);
            if (match.Success == false)
                throw exception ?? (message == null ? new StringDoesNotMatchException(parameterName, parameter, pattern) : new StringDoesNotMatchException(message, parameterName));
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> contains the specified text, or otherwise throws a
        ///     <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="containedText">The text that must be contained in <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="StringException" /> or
        ///     <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not contain the
        ///     specified text (optional). Please note that <paramref name="message" /> and
        ///     <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> does not contain the specified text and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no
        ///     <paramref name="exception" /> is specified.
        ///     or
        ///     Thrown when <paramref name="containedText" /> is null.
        /// </exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="containedText" /> is an empty string.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContain(this string parameter, string containedText, string parameterName = null, string message = null, Exception exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);
            containedText.MustNotBeNullOrEmpty(nameof(containedText), $"You called MustContain wrongly by specifying {(containedText == null ? "null" : "an empty string")} to containedText.");

            if (parameter.Contains(containedText) == false)
                throw exception ?? new StringException(message ?? $"{parameterName ?? "The string"} must contain the text \"{containedText}\", but you specified \"{parameter}\".", parameterName);
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> does not contain the specified text, or otherwise throws a
        ///     <see cref="StringException" />.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="textToCompare">The text that should not be part of <paramref name="parameter" />.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that should be injected into the <see cref="StringException" /> or
        ///     <see cref="ArgumentNullException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does contain the
        ///     specified text (optional). Please note that <paramref name="message" /> and
        ///     <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="StringException">
        ///     Thrown when <paramref name="parameter" /> contains <paramref name="textToCompare" />
        ///     an no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null and no
        ///     <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContain(this string parameter, string textToCompare, string parameterName = null, string message = null, Exception exception = null)
        {
            parameter.MustNotBeNull(parameterName, message, exception);

            if (parameter.Contains(textToCompare))
                throw exception ?? new StringException(message ?? $"{parameterName ?? "The string"} must not contain the text \"{textToCompare}\", but you specified \"{parameter}\".", parameterName);
        }
    }
}