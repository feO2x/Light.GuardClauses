using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// Indicates that data is in invalid state.
    /// </summary>
#if !NETSTANDARD1_0
    [Serializable]
#endif
    public class InvalidStateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InvalidStateException" />.
        /// </summary>
        /// <param name="message">The error message that explains the exception (optional).</param>
        /// <param name="innerException">The exception that is the cause of this exception (optional).</param>
        public InvalidStateException(string message = null, Exception innerException = null)
            : base(message, innerException) { }
    }
}