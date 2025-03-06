using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span has an invalid length,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidSpanLength<T>(
        ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The read-only span"} must have length {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not longer than the
    /// specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeLongerThan<T>(
        ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The span"} must be longer than {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not longer than and
    /// not equal to the specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeLongerThanOrEqualTo<T>(
        ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The span"} must be longer than or equal to {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not shorter than the
    /// specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeShorterThan<T>(
        ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The span"} must be shorter than {length}, but it actually has length {parameter.Length}."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a span is not shorter than the
    /// specified length.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SpanMustBeShorterThanOrEqualTo<T>(
        ReadOnlySpan<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The span"} must be shorter than or equal to {length}, but it actually has length {parameter.Length}."
        );
}
