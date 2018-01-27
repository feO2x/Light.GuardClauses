using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a value is not null although it should have been.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class ArgumentNotNullException : ArgumentException
    {
        /// <summary>
        ///     Create a new instance of <see cref="ArgumentNotNullException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that was not null (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="innerException">The exception that led to this one (optional).</param>
        public ArgumentNotNullException(string parameterName = null, string message = null, Exception innerException = null)
            : base(message, parameterName, innerException) { }
    }
}