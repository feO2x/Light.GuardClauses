using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws an <see cref="InvalidEmailAddressException" /> using the optional message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidEmailAddress(
        string parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidEmailAddressException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be a valid email address, but it actually is \"{parameter}\"."
        );

    /// <summary>
    /// Throws an <see cref="InvalidEmailAddressException" /> using the optional message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidEmailAddress(
        ReadOnlySpan<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidEmailAddressException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be a valid email address, but it actually is \"{parameter.ToString()}\"."
        );
}
