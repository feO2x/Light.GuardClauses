using System;

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    ///     This exception indicates that a Nullable has a value although it should not have one.
    /// </summary>
#if (NETSTANDARD2_0 || NET45)
    [Serializable]
#endif
    public class NullableHasValueException : ArgumentException
    {
        /// <summary>
        ///     Creates new instance of <see cref="NullableHasValueException" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">The message of the exception.</param>
        public NullableHasValueException(string parameterName = null, string message = null)
            : base(message, parameterName) { }
    }
}