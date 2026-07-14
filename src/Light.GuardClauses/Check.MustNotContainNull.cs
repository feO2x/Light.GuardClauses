using System;
using System.Collections;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the collection does not contain a null item, or otherwise throws an <see cref="ExistingItemException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ExistingItemException">Thrown when <paramref name="parameter" /> contains a null item.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    /// <remarks>
    /// This method inspects the collection once, stops at the first null item, runs in O(n) time, and uses constant
    /// additional space. <see cref="IList" /> receivers are inspected by index without allocating an enumerator; other
    /// receivers are enumerated once. Empty collections succeed. Non-generic access boxes value-type items.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustNotContainNull<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable
    {
        var position = FindNullItem(parameter.MustNotBeNull(parameterName, message));
        if (position >= 0)
        {
            Throw.NullItem(position, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection does not contain a null item, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or contains a null item.</exception>
    /// <remarks>
    /// This method inspects the collection once, stops at the first null item, runs in O(n) time, and uses constant
    /// additional space. <see cref="IList" /> receivers are inspected by index without allocating an enumerator; other
    /// receivers are enumerated once. Empty collections succeed. Non-generic access boxes value-type items.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static TCollection MustNotContainNull<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        Func<TCollection?, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable
    {
        if (parameter is null)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        if (FindNullItem(parameter) >= 0)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the <see cref="ImmutableArray{T}" /> does not contain a null item, or otherwise throws an <see cref="ExistingItemException" />.
    /// </summary>
    /// <param name="parameter">The immutable array to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ExistingItemException">Thrown when <paramref name="parameter" /> contains a null item.</exception>
    /// <remarks>
    /// This method inspects an initialized array by index without allocating an enumerator, stops at the first null
    /// item, runs in O(n) time, and uses constant additional space. Empty and default immutable arrays succeed.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> MustNotContainNull<T>(
        this ImmutableArray<T> parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    )
    {
        if (parameter.IsDefault)
        {
            return parameter;
        }

        for (var position = 0; position < parameter.Length; ++position)
        {
            if (parameter[position] is null)
            {
                Throw.NullItem(position, parameterName, message);
            }
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the <see cref="ImmutableArray{T}" /> does not contain a null item, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The immutable array to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> contains a null item.</exception>
    /// <remarks>
    /// This method inspects an initialized array by index without allocating an enumerator, stops at the first null
    /// item, runs in O(n) time, and uses constant additional space. Empty and default immutable arrays succeed.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static ImmutableArray<T> MustNotContainNull<T>(
        this ImmutableArray<T> parameter,
        Func<ImmutableArray<T>, Exception> exceptionFactory
    )
    {
        if (parameter.IsDefault)
        {
            return parameter;
        }

        for (var position = 0; position < parameter.Length; ++position)
        {
            if (parameter[position] is null)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }
        }

        return parameter;
    }

    private static int FindNullItem(IEnumerable parameter)
    {
        if (parameter is IList list)
        {
            for (var position = 0; position < list.Count; ++position)
            {
                if (list[position] is null)
                {
                    return position;
                }
            }

            return -1;
        }

        var currentPosition = 0;
        foreach (var item in parameter)
        {
            if (item is null)
            {
                return currentPosition;
            }

            ++currentPosition;
        }

        return -1;
    }
}
