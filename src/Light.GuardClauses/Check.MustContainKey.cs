using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the dictionary contains the specified key, or otherwise throws a <see cref="MissingKeyException" />.
    /// The check is performed via <see cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey" />, so the dictionary is
    /// never enumerated and its key comparer is respected.
    /// </summary>
    /// <param name="parameter">The dictionary to be checked.</param>
    /// <param name="key">The key that must be present in the dictionary.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="MissingKeyException">Thrown when <paramref name="parameter" /> does not contain <paramref name="key" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static IReadOnlyDictionary<TKey, TValue> MustContainKey<TKey, TValue>(
        [NotNull] [ValidatedNotNull] this IReadOnlyDictionary<TKey, TValue>? parameter,
        TKey key,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (!parameter.MustNotBeNull(parameterName, message).ContainsKey(key))
        {
            Throw.MissingKey(parameter, key, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the dictionary contains the specified key, or otherwise throws your custom exception.
    /// The check is performed via <see cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey" />, so the dictionary is
    /// never enumerated and its key comparer is respected.
    /// </summary>
    /// <param name="parameter">The dictionary to be checked.</param>
    /// <param name="key">The key that must be present in the dictionary.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="key" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="key" />, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static IReadOnlyDictionary<TKey, TValue> MustContainKey<TKey, TValue>(
        [NotNull] [ValidatedNotNull] this IReadOnlyDictionary<TKey, TValue>? parameter,
        TKey key,
        Func<IReadOnlyDictionary<TKey, TValue>?, TKey, Exception> exceptionFactory
    )
    {
        if (parameter is null || !parameter.ContainsKey(key))
        {
            Throw.CustomException(exceptionFactory, parameter, key);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the dictionary contains the specified key, or otherwise throws a <see cref="MissingKeyException" />.
    /// The check is performed via <see cref="Dictionary{TKey, TValue}.ContainsKey" />, so the dictionary is
    /// never enumerated and its key comparer is respected.
    /// </summary>
    /// <param name="parameter">The dictionary to be checked.</param>
    /// <param name="key">The key that must be present in the dictionary.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="MissingKeyException">Thrown when <paramref name="parameter" /> does not contain <paramref name="key" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Dictionary<TKey, TValue> MustContainKey<TKey, TValue>(
        [NotNull] [ValidatedNotNull] this Dictionary<TKey, TValue>? parameter,
        TKey key,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TKey : notnull
    {
        if (!parameter.MustNotBeNull(parameterName, message).ContainsKey(key))
        {
            Throw.MissingKey(parameter, key, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the dictionary contains the specified key, or otherwise throws your custom exception.
    /// The check is performed via <see cref="Dictionary{TKey, TValue}.ContainsKey" />, so the dictionary is
    /// never enumerated and its key comparer is respected.
    /// </summary>
    /// <param name="parameter">The dictionary to be checked.</param>
    /// <param name="key">The key that must be present in the dictionary.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="key" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="key" />, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static Dictionary<TKey, TValue> MustContainKey<TKey, TValue>(
        [NotNull] [ValidatedNotNull] this Dictionary<TKey, TValue>? parameter,
        TKey key,
        Func<Dictionary<TKey, TValue>?, TKey, Exception> exceptionFactory
    ) where TKey : notnull
    {
        if (parameter is null || !parameter.ContainsKey(key))
        {
            Throw.CustomException(exceptionFactory, parameter, key);
        }

        return parameter;
    }
}
