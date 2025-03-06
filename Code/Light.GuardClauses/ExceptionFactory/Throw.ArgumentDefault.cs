using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentDefaultException" /> indicating that a value is the default value of its
    /// type, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ArgumentDefault(string? parameterName = null, string? message = null) =>
        throw new ArgumentDefaultException(
            parameterName,
            message ?? $"{parameterName ?? "The value"} must not be the default value."
        );
}
