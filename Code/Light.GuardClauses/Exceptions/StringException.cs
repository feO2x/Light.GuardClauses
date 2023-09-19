using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a string is in an invalid state.
/// </summary>
[Serializable]
public class StringException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="StringException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public StringException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

#if !NET8_0
    /// <inheritdoc />
    protected StringException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}