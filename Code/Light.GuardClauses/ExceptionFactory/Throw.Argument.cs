using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws an <see cref="ArgumentException" /> using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void Argument(string? parameterName = null, string? message = null) =>
        throw new ArgumentException(message ?? $"{parameterName ?? "The value"} is invalid.", parameterName);
}
