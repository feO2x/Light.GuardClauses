using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="NullableHasNoValueException" /> indicating that a <see cref="Nullable{T}" /> has
    /// no value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NullableHasNoValue(string? parameterName = null, string? message = null) =>
        throw new NullableHasNoValueException(
            parameterName,
            message ?? $"{parameterName ?? "The nullable"} must have a value, but it actually is null."
        );
}
