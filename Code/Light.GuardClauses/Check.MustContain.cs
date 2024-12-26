using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the collection contains the specified item, or otherwise throws a <see cref="MissingItemException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="item">The item that must be part of the collection.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="MissingItemException">Thrown when <paramref name="parameter" /> does not contain <paramref name="item" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustContain<TCollection, TItem>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        TItem item,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable<TItem>
    {
        if (parameter is ICollection<TItem> collection)
        {
            if (!collection.Contains(item))
            {
                Throw.MissingItem(parameter, item, parameterName, message);
            }

            return parameter;
        }

        if (!parameter.MustNotBeNull(parameterName, message).Contains(item))
        {
            Throw.MissingItem(parameter, item, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection contains the specified item, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="item">The item that must be part of the collection.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="item" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="item" />, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustContain<TCollection, TItem>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        TItem item,
        Func<TCollection?, TItem, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable<TItem>
    {
        if (parameter is ICollection<TItem> collection)
        {
            if (!collection.Contains(item))
            {
                Throw.CustomException(exceptionFactory, parameter, item);
            }

            return parameter;
        }

        if (parameter is null || !parameter.Contains(item))
        {
            Throw.CustomException(exceptionFactory, parameter, item);
        }

        return parameter;
    }
}
