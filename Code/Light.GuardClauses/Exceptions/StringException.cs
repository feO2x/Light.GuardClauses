using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that there is something wrong with a string.
    /// </summary>
    public class StringException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="StringException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public StringException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}