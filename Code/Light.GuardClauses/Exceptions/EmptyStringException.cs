using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a string is empty.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class EmptyStringException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref="EmptyStringException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public EmptyStringException(string parameterName, Exception innerException = null)
            : base($"{parameterName ?? "The value"} must not be an empty string, but you specified one.", parameterName, innerException) { }

        /// <summary>
        /// Creates a new instance of <see cref="EmptyStringException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public EmptyStringException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}