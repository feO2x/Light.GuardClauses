using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a collection has an invalid number of items.
/// </summary>
[Serializable]
public class InvalidCollectionCountException : CollectionException
{
    /// <summary>
    /// Creates a new instance of <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public InvalidCollectionCountException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

    /// <inheritdoc />
    protected InvalidCollectionCountException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}