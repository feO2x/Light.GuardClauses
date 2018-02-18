using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a <see cref="DateTime" /> value is invalid.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class InvalidDateTimeException : ArgumentException
    {
        /// <summary>
        /// Gets the invalid <see cref="DateTime" /> value associated with this exception.
        /// </summary>
        public readonly DateTime InvalidDateTime;

        /// <summary>
        /// Initializes a new instance of <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="invalidDateTime">The invalid <see cref="DateTime" /> value that is the cause of this exception.</param>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public InvalidDateTimeException(DateTime invalidDateTime, string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException)
        {
            InvalidDateTime = invalidDateTime;
        }
    }
}