using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a collection contains an item that must not be part of it.
/// </summary>
[Serializable]
public class ExistingItemException : CollectionException
{
    /// <summary>
    /// Creates a new instance of <see cref="ExistingItemException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public ExistingItemException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

    /// <inheritdoc />
    protected ExistingItemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}