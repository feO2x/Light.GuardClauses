using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="StringLengthException" /> indicating that a string is not shorter than the given
    /// length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotShorterThan(
        string parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringLengthException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be shorter than {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="StringLengthException" /> indicating that a string is not shorter or equal to the
    /// given length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotShorterThanOrEqualTo(
        string parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringLengthException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be shorter or equal to {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="StringLengthException" /> indicating that a string has a different length than the
    /// specified one, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringLengthNotEqualTo(
        string parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringLengthException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must have length {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="StringLengthException" /> indicating that a string is not longer than the given
    /// length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotLongerThan(
        string parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringLengthException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be longer than {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="StringLengthException" /> indicating that a string is not longer than or equal to
    /// the given length, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringNotLongerThanOrEqualTo(
        string parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringLengthException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be longer than or equal to {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="StringLengthException" /> indicating that a string's length is not in within the
    /// given range, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringLengthNotInRange(
        string parameter,
        Range<int> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new StringLengthException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must have its length in between {range.CreateRangeDescriptionText("and")}, but it actually has length {parameter.Length}."
        );
}
