using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a string only consists of whitespace.
    /// </summary>
    public class StringIsOnlyWhiteSpaceException : StringException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="StringIsOnlyWhiteSpaceException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public StringIsOnlyWhiteSpaceException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="StringDoesNotMatchException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="actualValue">The actual value of the string.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        /// <returns>The new exception instance.</returns>
        public static StringIsOnlyWhiteSpaceException CreateDefault(string parameterName, string actualValue, Exception innerException = null)
        {
            return new StringIsOnlyWhiteSpaceException($"{parameterName ?? "The value"} must be a string that must have content other than only whitespace, but you specified \"{actualValue}\"", parameterName, innerException);
        }
    }
}