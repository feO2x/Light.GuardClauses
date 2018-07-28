﻿using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an URI is invalid.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class UriException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="UriException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public UriException(string parameterName = null, string message = null) : base(message, parameterName) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35)
        /// <inheritdoc />
        protected UriException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
