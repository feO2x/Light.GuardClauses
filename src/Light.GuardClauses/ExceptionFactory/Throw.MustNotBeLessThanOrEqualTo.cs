using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a comparable value must not be
    /// less than or equal to the given boundary value, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeLessThanOrEqualTo<T>(
        T parameter,
        T boundary,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} must not be less than or equal to {boundary}, but it actually is {parameter}."
        );
}
