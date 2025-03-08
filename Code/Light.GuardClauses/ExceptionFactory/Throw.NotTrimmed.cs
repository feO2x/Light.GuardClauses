using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="StringException" /> indicating that a string is not trimmed.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotTrimmed(string? parameter, string? parameterName, string? message) =>
        throw new StringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be trimmed, but it actually is {parameter.ToStringOrNull()}."
        );
}
