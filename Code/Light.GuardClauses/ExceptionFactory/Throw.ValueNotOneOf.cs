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
    /// Throws the default <see cref="ValueIsNotOneOfException" /> indicating that a value is not one of a specified
    /// collection of items, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ValueNotOneOf<TItem>(
        TItem parameter,
        IEnumerable<TItem> items,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ValueIsNotOneOfException(
            parameterName,
            message ??
            new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be one of the following items")
                               .AppendItemsWithNewLine(items)
                               .AppendLine($"but it actually is {parameter.ToStringOrNull()}.")
                               .ToString()
        );
}
