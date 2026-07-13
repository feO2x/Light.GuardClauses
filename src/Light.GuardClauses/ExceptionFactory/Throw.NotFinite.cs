using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException" /> indicating that a floating-point value is not finite.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotFinite<T>(
        T parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ?? $"{parameterName ?? "The value"} must be finite, but it actually is {parameter}."
        );
}
