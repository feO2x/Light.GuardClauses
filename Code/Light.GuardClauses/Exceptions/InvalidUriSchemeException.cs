using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that an <see cref="Uri" /> has an invalid scheme.
    /// </summary>
#if !NETSTANDARD1_0
    [Serializable]
#endif
    public class InvalidUriSchemeException : UriException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InvalidUriSchemeException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public InvalidUriSchemeException(string parameterName = null, string message = null)
            : base(parameterName, message) { }
    }
}