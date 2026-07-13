using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="EmptyStringException" /> indicating that a string is empty, using the optional
    /// parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EmptyString(string? parameterName = null, string? message = null) =>
        throw new EmptyStringException(
            parameterName,
            message ?? $"{parameterName ?? "The string"} must not be an empty string, but it actually is."
        );
}