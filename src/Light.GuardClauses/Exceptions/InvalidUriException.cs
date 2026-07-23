using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a string is not a valid URI.
/// </summary>
[Serializable]
public class InvalidUriException : UriException
{
    /// <summary>
    /// Creates a new instance of <see cref="InvalidUriException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public InvalidUriException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

#if !NET8_0_OR_GREATER
    /// <inheritdoc />
    protected InvalidUriException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}
