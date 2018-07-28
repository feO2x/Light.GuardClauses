using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a string is not matching a regular expression.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class StringDoesNotMatchException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref="StringDoesNotMatchException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public StringDoesNotMatchException(string parameterName = null, string message = null) : base(parameterName, message) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
        /// <inheritdoc />
        protected StringDoesNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
