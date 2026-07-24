using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ArgumentException" /> indicating that the specified type is not a concrete class,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MustBeConcreteClass(
        Type parameter,
        string? parameterName = null,
        string? message = null
    ) =>
        throw new ArgumentException(
            message ?? $"Type \"{parameter}\" must be a non-abstract class, but it is not.",
            parameterName
        );
}
