using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates a type mismatch resulting from a downcast.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class TypeMismatchException : ArgumentException
    {
        /// <summary>
        ///     Creates a new instance of <see cref="TypeMismatchException" />.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public TypeMismatchException(string message, string parameterName, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}