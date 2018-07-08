using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a <see cref="DateTime"/> value is invalid.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class InvalidDateTimeException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public InvalidDateTimeException(string parameterName = null, string message = null) : base(message, parameterName) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
        /// <inheritdoc />
        protected InvalidDateTimeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
