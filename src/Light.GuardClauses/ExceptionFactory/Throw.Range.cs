using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value is not within a specified
    /// range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeInRange<T>(
        T parameter,
        Range<T> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} must be between {range.CreateRangeDescriptionText("and")}, but it actually is {parameter}."
        );

    /// <summary>
    /// Throws the default <see cref="ArgumentOutOfRangeException" /> indicating that a value is within a specified
    /// range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustNotBeInRange<T>(
        T parameter,
        Range<T> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : IComparable<T> =>
        throw new ArgumentOutOfRangeException(
            parameterName,
            message ??
            $"{parameterName ?? "The value"} must not be between {range.CreateRangeDescriptionText("and")}, but it actually is {parameter}."
        );
}
