using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentException" /> indicating that values of the candidate type cannot be
    /// assigned to variables of the required type, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeAssignableTo(
        Type parameter,
        Type requiredType,
        string? parameterName = null,
        string? message = null
    ) =>
        throw new ArgumentException(
            message ??
            $"Values of type \"{parameter}\" must be assignable to variables of type \"{requiredType}\", but they are not.",
            parameterName
        );
}
