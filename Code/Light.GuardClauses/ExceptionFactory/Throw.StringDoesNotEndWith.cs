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
    /// Throws the default <see cref="SubstringException" /> indicating that a string does not end with another one,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void StringDoesNotEndWith(
        string parameter,
        string other,
        StringComparison comparisonType,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new SubstringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must end with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}."
        );
}
