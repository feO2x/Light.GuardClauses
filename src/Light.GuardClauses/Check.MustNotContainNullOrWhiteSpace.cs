using System;
using System.Collections.Generic;
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
    /// Ensures that the collection does not contain a null, empty, or white-space-only string, or otherwise throws an <see cref="ExistingItemException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ExistingItemException">Thrown when <paramref name="parameter" /> contains a null, empty, or white-space-only string.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    /// <remarks>
    /// This method inspects the collection once, stops at the first invalid item, runs in O(n) time, and uses constant
    /// additional space. <see cref="IList{T}" /> and <see cref="IReadOnlyList{T}" /> receivers are inspected by index
    /// without allocating an enumerator; other receivers are enumerated once. Empty collections succeed. White space
    /// is classified with the same Unicode semantics as <see cref="string.IsNullOrWhiteSpace(string?)" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustNotContainNullOrWhiteSpace<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable<string?>
    {
        var position = FindNullOrWhiteSpaceItem(
            parameter.MustNotBeNull(parameterName, message),
            out var invalidItem
        );
        if (position >= 0)
        {
            Throw.NullOrWhiteSpaceItem(invalidItem, position, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection does not contain a null, empty, or white-space-only string, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is null or contains a null, empty, or white-space-only string.</exception>
    /// <remarks>
    /// This method inspects the collection once, stops at the first invalid item, runs in O(n) time, and uses constant
    /// additional space. <see cref="IList{T}" /> and <see cref="IReadOnlyList{T}" /> receivers are inspected by index
    /// without allocating an enumerator; other receivers are enumerated once. Empty collections succeed. White space
    /// is classified with the same Unicode semantics as <see cref="string.IsNullOrWhiteSpace(string?)" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static TCollection MustNotContainNullOrWhiteSpace<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        Func<TCollection?, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable<string?>
    {
        if (parameter is null)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        if (FindNullOrWhiteSpaceItem(parameter, out _) >= 0)
        {
            Throw.CustomException(exceptionFactory, parameter);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the immutable array does not contain a null, empty, or white-space-only string, or otherwise throws an <see cref="ExistingItemException" />.
    /// </summary>
    /// <param name="parameter">The immutable array to be checked.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <exception cref="ExistingItemException">Thrown when <paramref name="parameter" /> contains a null, empty, or white-space-only string.</exception>
    /// <remarks>
    /// This method inspects an initialized array by index without allocating an enumerator, stops at the first invalid
    /// item, runs in O(n) time, and uses constant additional space. Empty and default immutable arrays succeed. White
    /// space is classified with the same Unicode semantics as <see cref="string.IsNullOrWhiteSpace(string?)" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<string?> MustNotContainNullOrWhiteSpace(
        this ImmutableArray<string?> parameter,
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
            var item = parameter[position];
            if (item.IsNullOrWhiteSpace())
            {
                Throw.NullOrWhiteSpaceItem(item, position, parameterName, message);
            }
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the immutable array does not contain a null, empty, or white-space-only string, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The immutable array to be checked.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> is passed to this delegate.</param>
    /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> contains a null, empty, or white-space-only string.</exception>
    /// <remarks>
    /// This method inspects an initialized array by index without allocating an enumerator, stops at the first invalid
    /// item, runs in O(n) time, and uses constant additional space. Empty and default immutable arrays succeed. White
    /// space is classified with the same Unicode semantics as <see cref="string.IsNullOrWhiteSpace(string?)" />.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("exceptionFactory:null => halt")]
    public static ImmutableArray<string?> MustNotContainNullOrWhiteSpace(
        this ImmutableArray<string?> parameter,
        Func<ImmutableArray<string?>, Exception> exceptionFactory
    )
    {
        if (parameter.IsDefault)
        {
            return parameter;
        }

        for (var position = 0; position < parameter.Length; ++position)
        {
            if (parameter[position].IsNullOrWhiteSpace())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }
        }

        return parameter;
    }

    private static int FindNullOrWhiteSpaceItem(IEnumerable<string?> parameter, out string? invalidItem) =>
        parameter switch
        {
            IList<string?> list => FindNullOrWhiteSpaceItem(list, out invalidItem),
            IReadOnlyList<string?> readOnlyList => FindNullOrWhiteSpaceItem(readOnlyList, out invalidItem),
            _ => FindNullOrWhiteSpaceItemViaForeach(parameter, out invalidItem),
        };

    private static int FindNullOrWhiteSpaceItemViaForeach(IEnumerable<string?> parameter, out string? invalidItem)
    {
        var currentPosition = 0;
        foreach (var item in parameter)
        {
            if (item.IsNullOrWhiteSpace())
            {
                invalidItem = item;
                return currentPosition;
            }

            ++currentPosition;
        }

        invalidItem = null;
        return -1;
    }

    private static int FindNullOrWhiteSpaceItem(IList<string?> list, out string? invalidItem)
    {
        for (var position = 0; position < list.Count; ++position)
        {
            var item = list[position];
            if (item.IsNullOrWhiteSpace())
            {
                invalidItem = item;
                return position;
            }
        }

        invalidItem = null;
        return -1;
    }

    private static int FindNullOrWhiteSpaceItem(IReadOnlyList<string?> list, out string? invalidItem)
    {
        for (var position = 0; position < list.Count; ++position)
        {
            var item = list[position];
            if (item.IsNullOrWhiteSpace())
            {
                invalidItem = item;
                return position;
            }
        }

        invalidItem = null;
        return -1;
    }
}
