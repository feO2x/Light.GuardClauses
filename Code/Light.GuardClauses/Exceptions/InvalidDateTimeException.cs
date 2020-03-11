using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a <see cref="DateTime"/> value is invalid.
    /// </summary>
    [Serializable]
    public class InvalidDateTimeException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public InvalidDateTimeException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        /// <inheritdoc />
        protected InvalidDateTimeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
