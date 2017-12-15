using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a GUID was empty.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class EmptyGuidException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="EmptyGuidException" /> with the default exception message.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public EmptyGuidException(string parameterName, Exception innerException = null)
            : base($"{parameterName ?? "The value"} must be a valid GUID, but you specified an empty one.", parameterName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyGuidException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public EmptyGuidException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}