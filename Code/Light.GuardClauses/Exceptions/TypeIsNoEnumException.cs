using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that that a type is no enum type.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class TypeIsNoEnumException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="TypeIsNoEnumException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public TypeIsNoEnumException(string parameterName = null, string message = null) : base(message, parameterName) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
        /// <inheritdoc />
        protected TypeIsNoEnumException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
