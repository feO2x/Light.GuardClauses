using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the default <see cref="ExistingKeyException" /> indicating that a dictionary contains the specified key
    /// that should not be part of it, using the optional parameter name and message.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void ExistingKey<TKey, TValue>(
        IReadOnlyDictionary<TKey, TValue> parameter,
        TKey key,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) =>
        throw new ExistingKeyException(
            parameterName,
            message ??
            $"{parameterName ?? "The dictionary"} must not contain key {key.ToStringOrNull()}, but it actually does."
        );
}
