using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="EmptyGuidException" /> indicating that a GUID is empty, using the optional
    /// parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EmptyGuid(string? parameterName = null, string? message = null) =>
        throw new EmptyGuidException(
            parameterName,
            message ?? $"{parameterName ?? "The value"} must be a valid GUID, but it actually is an empty one."
        );
}
