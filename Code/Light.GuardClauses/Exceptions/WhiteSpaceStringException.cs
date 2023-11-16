using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that a string contains only white space.
/// </summary>
[Serializable]
public class WhiteSpaceStringException : StringException
{
    /// <summary>
    /// Creates a new instance of <see cref="WhiteSpaceStringException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public WhiteSpaceStringException(string? parameterName = null, string? message = null) : base(parameterName, message) { }

#if !NET8_0
    /// <inheritdoc />
    protected WhiteSpaceStringException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif
}