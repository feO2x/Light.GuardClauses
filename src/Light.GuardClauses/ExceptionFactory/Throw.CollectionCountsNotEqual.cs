using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that two collections have different
    /// counts, using the already observed counts and optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CollectionCountsNotEqual(
        int count,
        int otherCount,
        string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The collection"} must have the same count as the comparison collection, but its count is {count} and the comparison collection's count is {otherCount}."
        );
}
