using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses.FrameworkExtensions;

/// <summary>
/// Provides extension methods for the <see cref="IEnumerable{T}" /> interface.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Tries to cast the specified enumerable to an <see cref="IList{T}" />, or
    /// creates a new <see cref="List{T}" /> containing the enumerable items.
    /// </summary>
    /// <typeparam name="T">The item type of the enumerable.</typeparam>
    /// <param name="source">The enumerable to be transformed.</param>
    /// <returns>The list containing the items of the enumerable.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("source:null => halt; source:notnull => notnull")]
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static IList<T> AsList<T>([NotNull, ValidatedNotNull] this IEnumerable<T> source) => 
        source as IList<T> ?? source.ToList();

    /// <summary>
    /// Tries to cast the specified enumerable to an <see cref="IList{T}" />, or
    /// creates a new collection containing the enumerable items by calling the specified delegate.
    /// </summary>
    /// <typeparam name="T">The item type of the collection.</typeparam>
    /// <param name="source">The enumerable that will be converted to <see cref="IList{T}" />.</param>
    /// <param name="createCollection">The delegate that creates the collection containing the specified items.</param>
    /// <returns>The cast enumerable, or a new collection containing the enumerable items.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> or <paramref name="createCollection" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("source:null => halt; source:notnull => notnull; createCollection:null => halt")]
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static IList<T> AsList<T>([NotNull, ValidatedNotNull] this IEnumerable<T> source, Func<IEnumerable<T>, IList<T>> createCollection) => 
        source as IList<T> ?? createCollection.MustNotBeNull(nameof(createCollection))(source.MustNotBeNull(nameof(source)));

    /// <summary>
    /// Tries to downcast the specified enumerable to an array, or creates a new array with the specified items.
    /// </summary>
    /// <typeparam name="T">The item type of the collection.</typeparam>
    /// <param name="source">The enumerable that will be converted to an array.</param>
    /// <returns>The cast array, or a new array containing the enumerable items.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("source:null => halt; source:notnull => notnull")]
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static T[] AsArray<T>([NotNull, ValidatedNotNull] this IEnumerable<T> source) => source as T[] ?? source.ToArray();

    /// <summary>
    /// Performs the action on each item of the specified enumerable. If the enumerable contains items that are null, this
    /// method can either throw an exception or ignore the value (your delegate will not be called in this case).
    /// </summary>
    /// <typeparam name="T">The item type of the enumerable.</typeparam>
    /// <param name="enumerable">The collection containing the items that will be passed to the action.</param>
    /// <param name="action">The action that executes for each item of the collection.</param>
    /// <param name="throwWhenItemIsNull">The value indicating whether this method should throw a <see cref="CollectionException" /> when any of the items is null (optional). Defaults to true.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="action" /> is null.</exception>
    /// <exception cref="CollectionException">Thrown when <paramref name="enumerable" /> contains a value that is null and <paramref name="throwWhenItemIsNull" /> is set to true.</exception>
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static IEnumerable<T> ForEach<T>([NotNull, ValidatedNotNull] this IEnumerable<T> enumerable, Action<T> action, bool throwWhenItemIsNull = true)
    {
        // ReSharper disable PossibleMultipleEnumeration
        action.MustNotBeNull(nameof(action));

        var i = 0;
        if (enumerable is IList<T> list)
        {
            for (; i < list.Count; i++)
            {
                var item = list[i];
                if (item is null)
                {
                    if (throwWhenItemIsNull) throw new CollectionException(nameof(enumerable), $"The collection contains null at index {i}.");
                    continue;
                }

                action(item);
            }
        }
        else
        {
            foreach (var item in enumerable.MustNotBeNull(nameof(enumerable)))
            {
                if (item is null)
                {
                    if (throwWhenItemIsNull) throw new CollectionException(nameof(enumerable), $"The collection contains null at index {i}.");
                    ++i;
                    continue;
                }

                action(item);
                ++i;
            }
        }

        return enumerable;
        // ReSharper restore PossibleMultipleEnumeration
    }

    /// <summary>
    /// Tries to cast the specified enumerable as an <see cref="IReadOnlyList{T}" />, or
    /// creates a new <see cref="List{T}" /> containing the enumerable items.
    /// </summary>
    /// <typeparam name="T">The item type of the enumerable.</typeparam>
    /// <param name="source">The enumerable to be transformed.</param>
    /// <returns>The list containing the items of the enumerable.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("source:null => halt; source:notnull => notnull")]
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static IReadOnlyList<T> AsReadOnlyList<T>([NotNull, ValidatedNotNull] this IEnumerable<T> source) =>
        source as IReadOnlyList<T> ?? source.ToList();

    /// <summary>
    /// Tries to cast the specified enumerable as an <see cref="IReadOnlyList{T}" />, or
    /// creates a new collection containing the enumerable items by calling the specified delegate.
    /// </summary>
    /// <typeparam name="T">The item type of the collection.</typeparam>
    /// <param name="source">The enumerable that will be converted to <see cref="IReadOnlyList{T}" />.</param>
    /// <param name="createCollection">The delegate that creates the collection containing the specified items.</param>
    /// <returns>The cast enumerable, or a new collection containing the enumerable items.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source" /> or <paramref name="createCollection" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("source:null => halt; source:notnull => notnull; createCollection:null => halt")]
    // ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static IReadOnlyList<T> AsReadOnlyList<T>([NotNull, ValidatedNotNull] this IEnumerable<T> source, [NotNull, ValidatedNotNull] Func<IEnumerable<T>, IReadOnlyList<T>> createCollection) =>
        source as IReadOnlyList<T> ?? createCollection.MustNotBeNull(nameof(createCollection))(source.MustNotBeNull(nameof(source)));
    // ReSharper restore RedundantNullableFlowAttribute


    /// <summary>
    /// Gets the count of the specified enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable whose count should be determined.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("enumerable:null => halt")]
    // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
    public static int Count([NotNull, ValidatedNotNull] this IEnumerable enumerable)
    {
        if (enumerable is ICollection collection)
            return collection.Count;
        if (enumerable is string @string)
            return @string.Length;

        return DetermineCountViaEnumerating(enumerable);
    }

    /// <summary>
    /// Gets the count of the specified enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable whose count should be determined.</param>
    /// <param name="parameterName">The name of the parameter that is passed to the <see cref="ArgumentNullException"/> (optional).</param>
    /// <param name="message">The message that is passed to the <see cref="ArgumentNullException"/> (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("enumerable:null => halt")]
    public static int Count([NotNull, ValidatedNotNull] this IEnumerable? enumerable, string? parameterName, string? message)
    {
        if (enumerable is ICollection collection)
            return collection.Count;
        if (enumerable is string @string)
            return @string.Length;

        return DetermineCountViaEnumerating(enumerable, parameterName, message);
    }
    
    /// <summary>
    /// Gets the count of the specified enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable whose count should be determined.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("enumerable:null => halt")]
    public static int GetCount<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is ICollection collection)
            return collection.Count;
        if (enumerable is string @string)
            return @string.Length;
        if (TryGetCollectionOfTCount(enumerable, out var count))
            return count;

        return DetermineCountViaEnumerating(enumerable);
    }

    /// <summary>
    /// Gets the count of the specified enumerable.
    /// </summary>
    /// <param name="enumerable">The enumerable whose count should be determined.</param>
    /// <param name="parameterName">The name of the parameter that is passed to the <see cref="ArgumentNullException"/> (optional).</param>
    /// <param name="message">The message that is passed to the <see cref="ArgumentNullException"/> (optional).</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable"/> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("enumerable:null => halt")]
    public static int GetCount<T>(this IEnumerable<T> enumerable, string? parameterName, string? message = null)
    {
        if (enumerable is ICollection collection)
            return collection.Count;
        if (enumerable is string @string)
            return @string.Length;
        if (TryGetCollectionOfTCount(enumerable, out var count))
            return count;

        return DetermineCountViaEnumerating(enumerable, parameterName, message);
    }

    private static bool TryGetCollectionOfTCount<T>([NoEnumeration] this IEnumerable<T> enumerable, out int count)
    {
        if (enumerable is ICollection<T> collectionOfT)
        {
            count = collectionOfT.Count;
            return true;
        }
        if (enumerable is IReadOnlyCollection<T> readOnlyCollection)
        {
            count = readOnlyCollection.Count;
            return true;
        }

        count = 0;
        return false;
    }

    private static int DetermineCountViaEnumerating(IEnumerable? enumerable)
    {
        var count = 0;
        var enumerator = enumerable.MustNotBeNull(nameof(enumerable)).GetEnumerator();
        while (enumerator.MoveNext())
            count++;
        if (enumerator is IDisposable disposable)
        {
            disposable.Dispose();
        }
        return count;
    }

    private static int DetermineCountViaEnumerating([NotNull] IEnumerable? enumerable, string? parameterName, string? message)
    {
        var count = 0;
        var enumerator = enumerable.MustNotBeNull(parameterName, message).GetEnumerator();
        while (enumerator.MoveNext())
            count++;
        if (enumerator is IDisposable disposable)
        {
            disposable.Dispose();
        }
        return count;
    }

    internal static bool ContainsViaForeach<TItem>(this IEnumerable<TItem> items, TItem item)
    {
        var equalityComparer = EqualityComparer<TItem>.Default;
        foreach (var i in items)
        {
            if (equalityComparer.Equals(i, item))
                return true;
        }

        return false;
    }
}