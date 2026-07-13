using System;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the collection has the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="count">The number of items the collection must have.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter" /> does not have the specified number of items.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustHaveCount<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        int count,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable
    {
        if (parameter!.Count(parameterName, message) != count)
        {
            Throw.InvalidCollectionCount(parameter, count, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection has the specified number of items, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="count">The number of items the collection must have.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="count" /> are passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not have the specified number of items, or when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustHaveCount<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        int count,
        Func<TCollection?, int, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable
    {
        if (parameter is null || parameter.Count() != count)
        {
            Throw.CustomException(exceptionFactory, parameter, count);
        }

        return parameter;
    }
}
