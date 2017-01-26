using System;
using System.Diagnostics;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Provides assertion extension methods for the <see cref="DateTime" /> type.
    /// </summary>
    public static class DateTimeAssertions
    {
        /// <summary>
        ///     Checks if the specified value is a UTC date time, or otherwise throws an <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="dateTime">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="InvalidDateTimeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="dateTime" />'s kind is not <see cref="DateTimeKind.Utc" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="dateTime" />'s is not <see cref="DateTimeKind.Utc" />.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeUtc(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Utc) return;

            throw exception != null ? exception() : new InvalidDateTimeException(dateTime, message ?? $"The specified date time \"{dateTime:O}\" must be of kind UTC, but actually is {dateTime.Kind}.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified value is a local date time, or otherwise throws an <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="dateTime">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="InvalidDateTimeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="dateTime" />'s kind is not <see cref="DateTimeKind.Local" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="dateTime" />'s is not <see cref="DateTimeKind.Local" />.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeLocal(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Local) return;

            throw exception != null ? exception() : new InvalidDateTimeException(dateTime, message ?? $"The specified date time \"{dateTime:O}\" must be of kind local, but actually is {dateTime.Kind}.", parameterName);
        }
    }
}