using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a dictionary contains a key that must not be part of it.
/// </summary>
[Serializable]
public class ExistingKeyException : CollectionException
{
    /// <summary>
    /// Creates a new instance of <see cref="ExistingKeyException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public ExistingKeyException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected ExistingKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}
