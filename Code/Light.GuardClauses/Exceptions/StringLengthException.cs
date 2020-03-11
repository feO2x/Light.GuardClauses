using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions {
    /// <summary>
    /// This exception indicates that a string has an invalid length.
    /// </summary>
    [Serializable]
    public class StringLengthException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref="StringLengthException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public StringLengthException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

        /// <inheritdoc />
        protected StringLengthException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}