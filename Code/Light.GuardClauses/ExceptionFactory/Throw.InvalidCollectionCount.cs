using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="InvalidCollectionCountException" /> indicating that a collection has an invalid
    /// number of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void InvalidCollectionCount(
        IEnumerable parameter,
        int count,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new InvalidCollectionCountException(
            parameterName,
            message ??
            $"{parameterName ?? "The collection"} must have count {count}, but it actually has count {parameter.Count()}."
        );
}
