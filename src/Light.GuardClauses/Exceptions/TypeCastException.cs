using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a value cannot be cast to another type.
/// </summary>
[Serializable]
public class TypeCastException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="TypeCastException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public TypeCastException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected TypeCastException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}