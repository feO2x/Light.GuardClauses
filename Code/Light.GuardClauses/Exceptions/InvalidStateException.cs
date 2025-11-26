using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that the data is in invalid state.
/// </summary>
[Serializable]
public class InvalidStateException : Exception
{
    /// <summary>
    /// Creates a new instance of <see cref="InvalidStateException" />.
    /// </summary>
    /// <param name="message">The message of the exception (optional).</param>
    /// <param name="innerException">The exception that is the cause of this one (optional).</param>
    public InvalidStateException(string? message = null, Exception? innerException = null) : base(message, innerException) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected InvalidStateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}