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
    /// Throws the default <see cref="SubstringException" /> indicating that a string does not contain another string as
    /// a substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotContain(
        string parameter,
        string substring,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new SubstringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must contain {substring.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}."
        );

    /// <summary>
    /// Throws the default <see cref="SubstringException" /> indicating that a string does not contain another string as
    /// a substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotContain(
        string parameter,
        string substring,
        StringComparison comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new SubstringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must contain {substring.ToStringOrNull()} ({comparisonType}), but it actually is {parameter.ToStringOrNull()}."
        );

    /// <summary>
    /// Throws the default <see cref="SubstringException" /> indicating that a string does contain another string as a
    /// substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringContains(
        string parameter,
        string substring,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new SubstringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must not contain {substring.ToStringOrNull()} as a substring, but it actually is {parameter.ToStringOrNull()}."
        );

    /// <summary>
    /// Throws the default <see cref="SubstringException" /> indicating that a string does contain another string as a
    /// substring, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringContains(
        string parameter,
        string substring,
        StringComparison comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new SubstringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must not contain {substring.ToStringOrNull()} as a substring ({comparisonType}), but it actually is {parameter.ToStringOrNull()}."
        );
}
