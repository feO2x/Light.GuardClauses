using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a <see cref="Nullable{T}" /> has no value.
/// </summary>
[Serializable]
public class NullableHasNoValueException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="NullableHasNoValueException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public NullableHasNoValueException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

#if !NET8_0
    /// <inheritdoc />
    protected NullableHasNoValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}