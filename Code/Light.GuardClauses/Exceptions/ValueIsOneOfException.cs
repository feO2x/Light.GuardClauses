using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an item is part of a collection.
    /// </summary>
    [Serializable]
    public class ValueIsOneOfException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="ValueIsOneOfException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public ValueIsOneOfException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        /// <inheritdoc />
        protected ValueIsOneOfException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
