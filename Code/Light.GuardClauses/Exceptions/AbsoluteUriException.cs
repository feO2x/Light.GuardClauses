using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that an URI is absolute instead of relative.
/// </summary>
[Serializable]
public class AbsoluteUriException : UriException
{
    /// <summary>
    /// Creates a new instance of <see cref="AbsoluteUriException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public AbsoluteUriException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

    /// <inheritdoc />
    protected AbsoluteUriException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}