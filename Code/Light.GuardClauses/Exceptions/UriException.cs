using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an URI is invalid.
    /// </summary>
#if !NETSTANDARD1_0
    [Serializable]
#endif
    public class UriException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="UriException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public UriException(string parameterName = null, string message = null)
            : base(message, parameterName) { }
    }
}