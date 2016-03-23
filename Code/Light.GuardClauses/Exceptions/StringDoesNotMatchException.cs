using System;
using System.Text.RegularExpressions;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a string does not match a certain regular  expression.
    /// </summary>
    public class StringDoesNotMatchException : StringException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="StringDoesNotMatchException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="actualValue">The value of the parameter.</param>
        /// <param name="regularExpression">The regular expression used for matching.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public StringDoesNotMatchException(string parameterName, string actualValue, Regex regularExpression, Exception innerException = null)
            : base($"{parameterName ?? "The string"} must match the regular expression {regularExpression}, but you specified {actualValue}.", parameterName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="StringDoesNotMatchException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public StringDoesNotMatchException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}