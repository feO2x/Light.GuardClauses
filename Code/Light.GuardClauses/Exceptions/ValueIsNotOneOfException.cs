using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an item is not part of a collection.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class ValueIsNotOneOfException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="CollectionException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public ValueIsNotOneOfException(string parameterName = null, string message = null) : base(message, parameterName) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
        /// <inheritdoc />
        protected ValueIsNotOneOfException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
