using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="StringException" /> indicating that a string is not a valid file extension.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotFileExtension(string? parameter, string? parameterName, string? message) =>
        throw new StringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be a valid file extension, but it actually is {parameter.ToStringOrNull()}."
        );

    /// <summary>
    /// Throws the default <see cref="StringException" /> indicating that a string is not a valid file extension.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotFileExtension(ReadOnlySpan<char> parameter, string? parameterName, string? message) =>
        throw new StringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be a valid file extension, but it actually is {parameter.ToString()}."
        );
}
