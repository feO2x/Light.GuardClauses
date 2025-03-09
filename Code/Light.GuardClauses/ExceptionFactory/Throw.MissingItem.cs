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
    /// Throws the default <see cref="MissingItemException" /> indicating that a collection is not containing the
    /// specified item, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MissingItem<TItem>(
        IEnumerable<TItem> parameter,
        TItem item,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new MissingItemException(
            parameterName,
            message ??
            new StringBuilder()
               .AppendLine(
                    $"{parameterName ?? "The collection"} must contain {item.ToStringOrNull()}, but it actually does not."
                )
               .AppendCollectionContent(parameter)
               .ToString()
        );
}
