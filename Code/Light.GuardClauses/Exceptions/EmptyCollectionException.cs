using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a collection is empty although it should have at least one item.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class EmptyCollectionException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="EmptyCollectionException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The inner exception that led to this one (optional).</param>
        public EmptyCollectionException(string parameterName, Exception innerException = null)
            : base($"{parameterName ?? "The value"} must not be an empty collection.", parameterName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyCollectionException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The inner exception that led to this one (optional).</param>
        public EmptyCollectionException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}