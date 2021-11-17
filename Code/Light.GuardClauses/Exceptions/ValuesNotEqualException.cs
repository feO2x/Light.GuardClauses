using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that two values are not equal.
/// </summary>
[Serializable]
public class ValuesNotEqualException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="ValuesNotEqualException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public ValuesNotEqualException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

    /// <inheritdoc />
    protected ValuesNotEqualException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}