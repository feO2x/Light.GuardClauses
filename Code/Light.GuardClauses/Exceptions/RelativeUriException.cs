using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a <see cref="Uri" /> is relative instead of absolute.
    /// </summary>
#if !NETSTANDARD1_0
    [Serializable]
#endif
    public class RelativeUriException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RelativeUriException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public RelativeUriException(string parameterName = null, string message = null)
            : base(message, parameterName) { }
    }
}