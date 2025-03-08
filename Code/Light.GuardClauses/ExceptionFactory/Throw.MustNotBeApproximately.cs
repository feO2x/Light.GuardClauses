using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value must not be approximately
    /// equal to another value within a specified tolerance, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeApproximately<T>(
        T parameter,
        T other,
        T tolerance,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} must not be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {parameter}."
        );
}