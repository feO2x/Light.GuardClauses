using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that two references point to the same object.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class SameObjectReferenceException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="SameObjectReferenceException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public SameObjectReferenceException(string parameterName = null, string message = null) : base(message, parameterName) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
        /// <inheritdoc />
        protected SameObjectReferenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
