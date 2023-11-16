using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that an item is not part of a collection.
/// </summary>
[Serializable]
public class ValueIsNotOneOfException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="CollectionException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public ValueIsNotOneOfException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

#if !NET8_0
    /// <inheritdoc />
    protected ValueIsNotOneOfException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}