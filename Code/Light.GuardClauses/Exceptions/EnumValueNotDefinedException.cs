using System;
#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
using System.Runtime.Serialization;
#endif

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a value is not defined in the corresponding enum type.
    /// </summary>
#if (NETSTANDARD2_0 || NET45 || NET40)
    [Serializable]
#endif
    public class EnumValueNotDefinedException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="EnumValueNotDefinedException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">The message of the exception.</param>
        public EnumValueNotDefinedException(string parameterName = null, string message = null) : base(message, parameterName) { }

#if (NETSTANDARD2_0 || NET45 || NET40 || NET35_FULL)
        /// <inheritdoc />
        protected EnumValueNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
    }
}
