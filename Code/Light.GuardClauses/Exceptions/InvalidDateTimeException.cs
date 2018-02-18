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
        /// Initializes a new instance of <see cref="InvalidDateTimeException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">The message of the exception.</param>
        public InvalidDateTimeException(string parameterName = null, string message = null)
            : base(message, parameterName) { }
    }
}