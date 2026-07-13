using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="StringException" /> indicating that a string is not equal to "\n" or "\r\n".
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NotNewLine(string? parameter, string? parameterName, string? message) =>
        throw new StringException(
            parameterName,
            message ??
            $"{parameterName ?? "The string"} must be either \"\\n\" or \"\\r\\n\", but it actually is {parameter.ToStringOrNull()}."
        );
}
