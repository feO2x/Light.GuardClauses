using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a Nullable has a value although it should not have one.
    /// </summary>
    public class NullableHasValueException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="NullableHasValueException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="actualValue">The actual value of the Nullable.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public NullableHasValueException(string parameterName, object actualValue, Exception innerException = null)
            : base($"{parameterName ?? "The Nullable"} must have no value, but you specified a Nullable with value {actualValue}.", parameterName, innerException) { }

        /// <summary>
        ///     Creates new instance of <see cref="NullableHasValueException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public NullableHasValueException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}