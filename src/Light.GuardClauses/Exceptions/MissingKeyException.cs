using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a key is not present in a dictionary.
/// </summary>
[Serializable]
public class MissingKeyException : CollectionException
{
    /// <summary>
    /// Creates a new instance of <see cref="MissingKeyException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public MissingKeyException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected MissingKeyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}
