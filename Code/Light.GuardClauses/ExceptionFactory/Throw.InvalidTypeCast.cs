using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="TypeCastException" /> indicating that a reference cannot be downcast, using the
    /// optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidTypeCast(
        object? parameter,
        Type targetType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new TypeCastException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} {parameter.ToStringOrNull()} cannot be cast to \"{targetType}\"."
        );
}
