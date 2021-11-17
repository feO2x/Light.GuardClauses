using System;
using System.Runtime.Serialization;

namespace Light.GuardClauses.Exceptions;

/// <summary>
/// This exception indicates that two values are equal.
/// </summary>
[Serializable]
public class ValuesEqualException : ArgumentException
{
    /// <summary>
    /// Creates a new instance of <see cref="ValuesEqualException" />.
    /// </summary>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message of the exception (optional).</param>
    public ValuesEqualException(string? parameterName = null, string? message = null) : base(message, parameterName) { }

    /// <inheritdoc />
    protected ValuesEqualException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}