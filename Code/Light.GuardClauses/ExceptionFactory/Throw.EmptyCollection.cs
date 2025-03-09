using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="EmptyCollectionException" /> indicating that a collection has no items, using the
    /// optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void EmptyCollection(string? parameterName = null, string? message = null) =>
        throw new EmptyCollectionException(
            parameterName,
            message ?? $"{parameterName ?? "The collection"} must not be an empty collection, but it actually is."
        );
}
