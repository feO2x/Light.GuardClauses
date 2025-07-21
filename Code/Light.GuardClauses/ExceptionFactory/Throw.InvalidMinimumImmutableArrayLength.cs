using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that an <see cref="ImmutableArray{T}" /> has less than a
    /// minimum number of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidMinimumImmutableArrayLength<T>(
        ImmutableArray<T> parameter,
        int length,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The immutable array"} must have at least a length of {length}, but it actually {(parameter.IsDefault ? "has no length because it is the default instance" : $"has a length of {parameter.Length}")}."
        );
    }
}
