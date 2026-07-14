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
    /// Throws the default <see cref="MissingKeyException" /> indicating that a dictionary is not containing the
    /// specified key, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void MissingKey<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> parameter,
        TKey key,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new MissingKeyException(
            parameterName,
            message ??
            new StringBuilder()
               .AppendLine(
                    $"{parameterName ?? "The dictionary"} must contain key {key.ToStringOrNull()}, but it actually does not."
                )
               .AppendCollectionContent(parameter.Keys, "Keys of the dictionary:")
               .ToString()
        );
}
