using System;
using System.Reflection;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicated that a <see cref="Type" /> or <see cref="TypeInfo" /> object is not valid.
    /// </summary>
#if !NETSTANDARD1_0
    [Serializable]
#endif
    public class TypeException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TypeException" />.
        /// </summary>
        /// <param name="message">The message of the exception (optional).</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        public TypeException(string parameterName, string message)
            : base(message, parameterName) { }
    }
}