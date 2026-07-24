using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a <see cref="TimeSpan" /> must
    /// be positive or equal to <see cref="System.Threading.Timeout.InfiniteTimeSpan" />, using the optional parameter
    /// name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBePositiveOrInfinite(
        TimeSpan parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} must be positive or equal to Timeout.InfiniteTimeSpan, but it actually is {parameter}."
        );
}
