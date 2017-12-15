using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a value in a dictionary is not present.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class ValueNotFoundException : DictionaryException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ValueNotFoundException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public ValueNotFoundException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}