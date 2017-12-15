using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a value is not null although it should have been.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class ArgumentNotNullException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ArgumentNotNullException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was not null.</param>
        /// <param name="actualValue">The actual value of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public ArgumentNotNullException(string parameterName, object actualValue, Exception innerException = null)
            : base($"{parameterName ?? "The specified value "} must be null, but you specified a valid reference to {actualValue}.", parameterName, innerException) { }

        /// <summary>
        ///     Create a new instance of <see cref="ArgumentNotNullException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter that was not null.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public ArgumentNotNullException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}