using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that an Email address is invalid.
/// </summary>
[Serializable]
public class InvalidEmailAddressException : StringException
{
    /// <summary>
    /// Creates a new instance of <see cref="InvalidEmailAddressException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public InvalidEmailAddressException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

#if !NET8_0
    /// <inheritdoc />
    protected InvalidEmailAddressException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}