using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="WhiteSpaceStringException" /> indicating that a string contains only white space,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void WhiteSpaceString(
        string parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new WhiteSpaceStringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must not contain only white space, but it actually is \"{parameter}\"."
        );

    /// <summary>
    /// Throws the default <see cref="WhiteSpaceStringException" /> indicating that a character span contains only
    /// white space, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void WhiteSpaceSpan(
        ReadOnlySpan<char> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new WhiteSpaceStringException(
            parameterName,
            message ??
            $"{parameterName ?? "The character span"} must not contain only white space, but it actually has length {parameter.Length}."
        );
}
