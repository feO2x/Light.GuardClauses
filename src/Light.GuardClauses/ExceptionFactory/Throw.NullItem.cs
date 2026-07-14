using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ExistingItemException" /> indicating that a collection contains a null item.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void NullItem(int position, string? parameterName = null, string? message = null) =>
        throw new ExistingItemException(
            parameterName,
            message ??
            $"{parameterName ?? "The collection"} must not contain null items, but a null item was found at position {position}."
        );
}
