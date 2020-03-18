using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a value of a value type is the default value.
    /// </summary>
    [Serializable]
    public class ArgumentDefaultException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="ArgumentDefaultException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public ArgumentDefaultException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        /// <inheritdoc />
        protected ArgumentDefaultException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}