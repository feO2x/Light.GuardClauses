using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a GUID is empty.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class EmptyGuidException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref="EmptyGuidException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">The message of the exception.</param>
        public EmptyGuidException(string parameterName = null, string message = null)
            : base(message, parameterName) { }
    }
}