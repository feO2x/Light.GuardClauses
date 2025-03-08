using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="SameObjectReferenceException" /> indicating that two references point to the same
    /// object, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void SameObjectReference<T>(
        T? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where T : class =>
        throw new SameObjectReferenceException(
            parameterName,
            message ??
            $"{parameterName ?? "The reference"} must not point to object \"{parameter}\", but it actually does."
        );
}
