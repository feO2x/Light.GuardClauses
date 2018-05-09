using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IEnumerable{T}" /> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Tries to cast the specified enumerable as an <see cref="IList{T}" />, or
        /// creates a new <see cref="List{T}" /> containing the enumerable's items.
        /// </summary>
        /// <typeparam name="T">The item type of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to be transformed.</param>
        /// <returns>The list containing the items of the enumerable.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> is null.</exception>
        public static IList<T> AsList<T>(this IEnumerable<T> enumerable) => 
            enumerable as IList<T> ?? enumerable.MustNotBeNull(nameof(enumerable)).ToList();

        /// <summary>
        /// Tries to cast the specified enumerable as an <see cref="IList{T}" />, or
        /// creates a new collection containing the enumerable's items by calling the specified delegate.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="enumerable">The enumerable that will be converted to <see cref="IList{T}" />.</param>
        /// <param name="createCollection">The delegate that creates the collection containing the specified items.</param>
        /// <returns>The casted enumerable, or a new collection containing the enumerable's items.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="createCollection" /> is null.</exception>
        public static IList<T> AsList<T>(this IEnumerable<T> enumerable, Func<IEnumerable<T>, IList<T>> createCollection) => 
            enumerable as IList<T> ?? createCollection.MustNotBeNull(nameof(createCollection))(enumerable.MustNotBeNull(nameof(enumerable)));

        /// <summary>
        /// Tries to downcast the specified enumerable as an array, or creates a new collection
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="enumerable">The enumerable that will be converted to an array.</param>
        /// <returns>The casted array, or a new array containing the enumerable's items.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> is null.</exception>
        public static T[] AsArray<T>(this IEnumerable<T> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            enumerable.MustNotBeNull(nameof(enumerable));

            if (enumerable is T[] array)
                return array;

            var i = 0;
            if (enumerable is IList<T> list)
            {
                array = new T[list.Count];
                for (; i < list.Count; i++) array[i] = list[i];
                return array;
            }

            array = new T[enumerable.Count()];
            foreach (var item in enumerable) array[i++] = item;
            return array;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Performs the action on each item of the specified enumerable.
        /// </summary>
        /// <typeparam name="T">The item type of the enumerable.</typeparam>
        /// <param name="enumerable">The collection containing the items that will be passed to the action.</param>
        /// <param name="action">The action that exectues for each item of the collection.</param>
        /// <param name="throwWhenItemIsNull">The value indicating whether this method should throw a <see cref="CollectionException" /> when any of the items is null (optional). Defaults to true.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="action" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="enumerable" /> contains a value that is null and <paramref name="throwWhenItemIsNull" /> is set to true.</exception>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action, bool throwWhenItemIsNull = true)
        {
            // ReSharper disable PossibleMultipleEnumeration
            enumerable.MustNotBeNull(nameof(enumerable));
            action.MustNotBeNull(nameof(action));

            var i = 0;
            if (enumerable is IList<T> list)
                for (; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item == null)
                    {
                        if (throwWhenItemIsNull) throw new CollectionException(nameof(enumerable), $"The collection contains null at index {i}.");
                        continue;
                    }

                    action(item);
                }
            else
                foreach (var item in enumerable)
                {
                    if (item == null)
                    {
                        if (throwWhenItemIsNull) throw new CollectionException(nameof(enumerable), $"The collection contains null at index {i}.");
                        ++i;
                        continue;
                    }

                    action(item);
                    ++i;
                }

            return enumerable;
            // ReSharper restore PossibleMultipleEnumeration
        }

#if NETSTANDARD2_0 || NETSTANDARD1_0 || NET45
        /// <summary>
        /// Tries to cast the specified enumerable as an <see cref="IReadOnlyList{T}" />, or
        /// creates a new <see cref="List{T}" /> containing the enumerable's items.
        /// </summary>
        /// <typeparam name="T">The item type of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to be transformed.</param>
        /// <returns>The list containing the items of the enumerable.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> is null.</exception>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable) =>
            enumerable as IReadOnlyList<T> ?? enumerable.MustNotBeNull(nameof(enumerable)).ToList();

        /// <summary>
        /// Tries to cast the specified enumerable as an <see cref="IReadOnlyList{T}" />, or
        /// creates a new collection containing the enumerable's items by calling the specified delegate.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="enumerable">The enumerable that will be converted to <see cref="IReadOnlyList{T}" />.</param>
        /// <param name="createCollection">The delegate that creates the collection containing the specified items.</param>
        /// <returns>The casted enumerable, or a new collection containing the enumerable's items.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="createCollection" /> is null.</exception>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable, Func<IEnumerable<T>, IReadOnlyList<T>> createCollection) =>
            enumerable as IReadOnlyList<T> ?? createCollection.MustNotBeNull(nameof(createCollection))(enumerable.MustNotBeNull(nameof(enumerable)));
#endif
    }
}