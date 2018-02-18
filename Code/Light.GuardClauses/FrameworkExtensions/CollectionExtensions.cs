using System;
using System.Collections.Generic;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IReadOnlyList{T}" />.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Gets the index of the specified item, or -1 if the it cannot be found.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="collection">The collection that might contain the item.</param>
        /// <param name="item">The item whose index is searched for.</param>
        /// <param name="equalityComparer">
        /// The equality comparer used to compare the items (optional). By default, <see cref="EqualityComparer{T}.Default" /> is used.
        /// </param>
        /// <returns>The index of the specified item, or -1 when the item could not be found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is null.</exception>
        public static int IndexOf<T>(this IReadOnlyList<T> collection, T item, IEqualityComparer<T> equalityComparer = null)
        {
            collection.MustNotBeNull(nameof(collection));
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            for (var i = 0; i < collection.Count; i++)
                if (equalityComparer.EqualsWithHashCode(item, collection[i]))
                    return i;

            return -1;
        }

        /// <summary>
        /// Checks if the specified collection contains the given item.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="collection">The collection that might contain the specified item.</param>
        /// <param name="item">The item being checked.</param>
        /// <param name="equalityComparer">
        /// The equality comparer used to compare the items (optional). By default, <see cref="EqualityComparer{T}.Default" /> is used.
        /// </param>
        /// <returns>True if the item is contained in the collection, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection" /> is null.</exception>
        public static bool IsContaining<T>(this IReadOnlyList<T> collection, T item, IEqualityComparer<T> equalityComparer = null)
        {
            return collection.IndexOf(item, equalityComparer) != -1;
        }
    }
}