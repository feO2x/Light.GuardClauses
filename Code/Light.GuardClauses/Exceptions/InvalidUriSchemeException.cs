using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that an URI has an invalid scheme.
/// </summary>
[Serializable]
public class InvalidUriSchemeException : UriException
{
    /// <summary>
    /// Creates a new instance of <see cref="InvalidUriSchemeException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public InvalidUriSchemeException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

    /// <inheritdoc />
    protected InvalidUriSchemeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}