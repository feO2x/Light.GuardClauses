using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that the data is in invalid state.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class InvalidStateException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidStateException" />.
        /// </summary>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="innerException">The exception that is the cause of this one (optional).</param>
        public InvalidStateException(string message = null, Exception innerException = null) : base(message, innerException) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
        /// <inheritdoc />
        protected InvalidStateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
