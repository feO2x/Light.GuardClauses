using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that an immutable array has an invalid length,
    /// using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidImmutableArrayLength<T>(
        ImmutableArray<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        var actualLength = parameter.IsDefault ? 0 : parameter.Length;
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The immutable array"} must have length {length}, but it actually has length {actualLength}."
        );
    }
}
