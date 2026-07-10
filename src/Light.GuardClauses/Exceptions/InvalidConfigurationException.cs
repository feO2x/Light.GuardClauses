using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that configuration data is invalid.
/// </summary>
[Serializable]
public class InvalidConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="InvalidConfigurationException"/>.
    /// </summary>
    /// <param name="message">The message of the exception (optional).</param>
    /// <param name="innerException">The exception that is the cause of this one (optional).</param>
    public InvalidConfigurationException(string? message = null, Exception? innerException = null) : base(message, innerException) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}