using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentNullException" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ArgumentNull(string? parameterName = null, string? message = null) =>
        throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
}
