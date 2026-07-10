using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using
    /// <see cref="DateTimeKind.Utc" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeUtcDateTime(
        DateTime parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidDateTimeException(
            parameterName,
            message ??
            $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Utc}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\"."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using
    /// <see cref="DateTimeKind.Local" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeLocalDateTime(
        DateTime parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidDateTimeException(
            parameterName,
            message ??
            $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Local}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\"."
        );

    /// <summary>
    /// Throws the default <see cref="InvalidDateTimeException" /> indicating that a date time is not using
    /// <see cref="DateTimeKind.Unspecified" />, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeUnspecifiedDateTime(
        DateTime parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidDateTimeException(
            parameterName,
            message ??
            $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Unspecified}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\"."
        );
}
