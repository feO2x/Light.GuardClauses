using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that an URI is relative instead of absolute.
/// </summary>
[Serializable]
public class RelativeUriException : UriException
{
    /// <summary>
    /// Creates a new instance of <see cref="RelativeUriException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public RelativeUriException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

    /// <inheritdoc />
    protected RelativeUriException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}