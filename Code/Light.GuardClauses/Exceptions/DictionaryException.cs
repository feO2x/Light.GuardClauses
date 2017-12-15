using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates an error with a dictionary.
    /// </summary>
#if NETSTANDARD2_0
    [Serializable]
#endif
    public class DictionaryException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="DictionaryException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public DictionaryException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}