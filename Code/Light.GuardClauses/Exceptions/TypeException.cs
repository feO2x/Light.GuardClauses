using System;
using System.Reflection;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicated that a <see cref="Type" /> or <see cref="TypeInfo" /> object is not in a valid state.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class TypeException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message" /> is null.</exception>
        /// <exception cref="EmptyStringException">Thrown when <paramref name="message" /> is an empty string.</exception>
        public TypeException(string message, string parameterName, Exception innerException = null)
            : base(message.MustNotBeNullOrEmpty(), parameterName, innerException) { }
    }
}