using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an item is not present in a collection.
    /// </summary>
    [Serializable]
    public class MissingItemException : CollectionException
    {
        /// <summary>
        /// Creates a new instance of <see cref="MissingItemException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public MissingItemException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

        /// <inheritdoc />
        protected MissingItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
