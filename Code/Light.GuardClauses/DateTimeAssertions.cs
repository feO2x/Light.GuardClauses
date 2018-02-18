using System;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    /// <summary>
    /// Provides assertion extension methods for the <see cref="DateTime" /> type.
    /// </summary>
    public static class DateTimeAssertions
    {
        /// <summary>
        /// Checks if the specified value is a UTC date time, or otherwise throws an <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="dateTime">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="InvalidDateTimeException" /> (optional).</param>
        /// <param name="exception">
        /// The exception that is thrown when <paramref name="dateTime" />'s kind is not <see cref="DateTimeKind.Utc" /> (optional).
        /// Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="dateTime" />'s is not <see cref="DateTimeKind.Utc" />.</exception>
        public static DateTime MustBeUtc(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Utc) return dateTime;

            throw exception != null ? exception() : new InvalidDateTimeException(dateTime, message ?? $"The specified date time \"{dateTime:O}\" must be of kind {DateTimeKind.Utc}, but actually is {dateTime.Kind}.", parameterName);
        }

        /// <summary>
        /// Checks if the specified value is a local date time, or otherwise throws an <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="dateTime">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="InvalidDateTimeException" /> (optional).</param>
        /// <param name="exception">
        /// The exception that is thrown when <paramref name="dateTime" />'s kind is not <see cref="DateTimeKind.Local" /> (optional).
        /// Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="dateTime" />'s is not <see cref="DateTimeKind.Local" />.</exception>
        public static DateTime MustBeLocal(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Local) return dateTime;

            throw exception != null ? exception() : new InvalidDateTimeException(dateTime, message ?? $"The specified date time \"{dateTime:O}\" must be of kind {DateTimeKind.Local}, but actually is {dateTime.Kind}.", parameterName);
        }

        /// <summary>
        /// Checks if the specified value is date time with <see cref="DateTimeKind.Unspecified" />, or otherwise throws an <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="dateTime">The value to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="InvalidDateTimeException" /> (optional).</param>
        /// <param name="exception">
        /// The exception that is thrown when <paramref name="dateTime" />'s kind is not <see cref="DateTimeKind.Unspecified" /> (optional).
        /// Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="InvalidDateTimeException">Thrown when <paramref name="dateTime" />'s is not <see cref="DateTimeKind.Unspecified" />.</exception>
        public static DateTime MustBeUnspecified(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified) return dateTime;

            throw exception != null ? exception() : new InvalidDateTimeException(dateTime, message ?? $"The specified date time \"{dateTime:O}\" must be of kind {DateTimeKind.Unspecified}, but actually is {dateTime.Kind}.", parameterName);
        }
    }
}