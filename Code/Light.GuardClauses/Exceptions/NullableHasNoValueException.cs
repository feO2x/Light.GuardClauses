using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a Nullable has no value although it should have one.
    /// </summary>
    public class NullableHasNoValueException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="NullableHasNoValueException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public NullableHasNoValueException(string parameterName, Exception innerException = null)
            : base($"{parameterName} must have a value, but you specified a nullable that has none.", parameterName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NullableHasNoValueException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public NullableHasNoValueException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}