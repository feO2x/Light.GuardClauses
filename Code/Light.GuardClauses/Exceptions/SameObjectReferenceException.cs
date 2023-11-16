using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that two references point to the same object.
/// </summary>
[Serializable]
public class SameObjectReferenceException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="SameObjectReferenceException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public SameObjectReferenceException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

#if !NET8_0
    /// <inheritdoc />
    protected SameObjectReferenceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}