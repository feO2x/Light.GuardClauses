using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a collection count is outside a range.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CollectionCountNotInRange(
        IEnumerable parameter,
        int actualCount,
        Range<int> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The collection"} must have its count between {range.CreateRangeDescriptionText("and")}, but it actually has count {actualCount}."
        );
}
