using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a value is not defined in the corresponding enum type.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class EnumValueNotDefinedException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="EnumValueNotDefinedException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="actualValue">The actual value.</param>
        /// <param name="enumType">The type of the enum.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public EnumValueNotDefinedException(string parameterName, object actualValue, Type enumType, Exception innerException = null)
            : base($"{parameterName ?? "The value"} should be a constant of enum {enumType.FullName}, but you specified {actualValue}.", parameterName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EnumValueNotDefinedException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        public EnumValueNotDefinedException(string message, string parameterName) : base(message, parameterName) { }
    }
}