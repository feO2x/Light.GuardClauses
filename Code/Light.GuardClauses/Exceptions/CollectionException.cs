using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that the state of a collection is invalid.
    /// </summary>
    [Serializable]
    public class CollectionException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="CollectionException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public CollectionException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

        /// <inheritdoc />
        protected CollectionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}