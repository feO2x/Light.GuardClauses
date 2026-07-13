using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that that a GUID is empty.
/// </summary>
[Serializable]
public class EmptyGuidException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="EmptyGuidException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public EmptyGuidException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected EmptyGuidException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}