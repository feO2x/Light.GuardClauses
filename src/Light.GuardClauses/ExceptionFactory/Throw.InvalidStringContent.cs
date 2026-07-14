using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws a <see cref="StringException" /> for character content that violates a required invariant.
    /// </summary>
    /// <param name="parameterName">The name of the parameter that contains the invalid string (optional).</param>
    /// <param name="message">The message that will be passed to the exception (optional).</param>
    /// <param name="requirement">A description of the requirement that the string violates.</param>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidStringContent(string? parameterName, string? message, string requirement) =>
        throw new StringException(
            parameterName,
            message ?? $"{parameterName ?? "The string"} {requirement}."
        );
}
