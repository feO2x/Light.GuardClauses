using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a Nullable has no value although it should have one.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class NullableHasNoValueException : ArgumentNullException
    {
        /// <summary>
        /// Creates a new instance of <see cref="NullableHasNoValueException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message of the exception (optional).</param>
        public NullableHasNoValueException(string parameterName = null, string message = null)
            : base(parameterName, message) { }
    }
}