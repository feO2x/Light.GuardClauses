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
    /// Ensures that the collection has the same number of items as <paramref name="otherCollection" />, or otherwise
    /// throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="otherCollection">The collection whose count is compared with <paramref name="parameter" />.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original collection.</returns>
    /// <exception cref="InvalidCollectionCountException">Thrown when the collections have different counts.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="otherCollection" /> is null.</exception>
    /// <remarks>
    /// The collections may have different concrete and element types, and their contents are not compared. Count-property
    /// fast paths do not enumerate. Other enumerable implementations are enumerated at most once each, and their
    /// enumerators are disposed. Comparing a collection with itself does not enumerate it. The operation uses O(n) time
    /// for enumerated inputs and constant additional space.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; otherCollection:null => halt")]
    public static TCollection MustHaveSameCountAs<TCollection, TOtherCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        [NotNull] [ValidatedNotNull] TOtherCollection? otherCollection,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
        where TCollection : class, IEnumerable
        where TOtherCollection : class, IEnumerable
    {
        var validatedParameter = parameter.MustNotBeNull(parameterName, message);
        var validatedOtherCollection = otherCollection.MustNotBeNull(message: message);

        if (ReferenceEquals(validatedParameter, validatedOtherCollection))
        {
            return validatedParameter;
        }

        var count = validatedParameter.Count();
        var otherCount = validatedOtherCollection.Count();
        if (count != otherCount)
        {
            Throw.CollectionCountsNotEqual(count, otherCount, parameterName, message);
        }

        return validatedParameter;
    }

    /// <summary>
    /// Ensures that the collection has the same number of items as <paramref name="otherCollection" />, or otherwise
    /// throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="otherCollection">The collection whose count is compared with <paramref name="parameter" />.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. Both original collections are passed to this delegate.</param>
    /// <returns>The original collection.</returns>
    /// <exception cref="Exception">Your custom exception thrown when either collection is null or the collections have different counts.</exception>
    /// <remarks>
    /// The collections may have different concrete and element types, and their contents are not compared. Count-property
    /// fast paths do not enumerate. Other enumerable implementations are enumerated at most once each, and their
    /// enumerators are disposed. Comparing a collection with itself does not enumerate it. The operation uses O(n) time
    /// for enumerated inputs and constant additional space.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation(
        "parameter:null => halt; parameter:notnull => notnull; otherCollection:null => halt; exceptionFactory:null => halt"
    )]
    public static TCollection MustHaveSameCountAs<TCollection, TOtherCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        [NotNull] [ValidatedNotNull] TOtherCollection? otherCollection,
        Func<TCollection?, TOtherCollection?, Exception> exceptionFactory
    )
        where TCollection : class, IEnumerable
        where TOtherCollection : class, IEnumerable
    {
        if (parameter is null || otherCollection is null)
        {
            Throw.CustomException(exceptionFactory, parameter, otherCollection);
        }

        if (ReferenceEquals(parameter, otherCollection))
        {
            return parameter;
        }

        var count = parameter.Count();
        var otherCount = otherCollection.Count();
        if (count != otherCount)
        {
            Throw.CustomException(exceptionFactory, parameter, otherCollection);
        }

        return parameter;
    }
}
