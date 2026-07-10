using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ExistingItemException" /> indicating that a collection contains the specified item
    /// that should not be part of it, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ExistingItem<TItem>(
        IEnumerable<TItem> parameter,
        TItem item,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ExistingItemException(
            parameterName,
            message ??
            new StringBuilder()
               .AppendLine(
                    $"{parameterName ?? "The collection"} must not contain {item.ToStringOrNull()}, but it actually does."
                )
               .AppendCollectionContent(parameter)
               .ToString()
        );
}
