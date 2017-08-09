using System;
using System.Collections.Generic;
using System.Linq;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    ///     Provides extension methods for the <see cref="IEnumerable{T}" /> interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Tries to cast the specified enumerable as an <see cref="IList{T}" />, or
        ///     creates a new <see cref="List{T}" /> containing the enumerable's items.
        /// </summary>
        /// <typeparam name="T">The item type of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to be transformed.</param>
        /// <returns>The list containing the items of the enumerable.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> is null.</exception>
        public static IList<T> AsList<T>(this IEnumerable<T> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            enumerable.MustNotBeNull(nameof(enumerable));

            return enumerable as IList<T> ?? enumerable.ToList();
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Tries to cast the specified enumerable as an <see cref="IReadOnlyList{T}" />, or
        ///     creates a new <see cref="List{T}" /> containing the enumerable's items.
        /// </summary>
        /// <typeparam name="T">The item type of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to be transformed.</param>
        /// <returns>The list containing the items of the enumerable.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> is null.</exception>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable)
        {
            // ReSharper disable PossibleMultipleEnumeration
            enumerable.MustNotBeNull(nameof(enumerable));

            return enumerable as IReadOnlyList<T> ?? enumerable.ToList();
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Performs the action on each item of the specified enumerable.
        /// </summary>
        /// <typeparam name="T">The item type of the enumerable.</typeparam>
        /// <param name="enumerable">The collection containing the items that will be passed to the action.</param>
        /// <param name="action">The action that exectues for each item of the collection.</param>
        /// <param name="throwWhenItemIsNull">The value indicating whether this method should throw a <see cref="CollectionException" /> when any of the items is null (optional). Defaults to true.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="action" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="enumerable"/> contains a value that is null and <paramref name="throwWhenItemIsNull"/> is set to true.</exception>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action, bool throwWhenItemIsNull = true)
        {
            // ReSharper disable PossibleMultipleEnumeration
            enumerable.MustNotBeNull(nameof(enumerable));
            action.MustNotBeNull(nameof(action));

            var i = 0;
            if (enumerable is IReadOnlyList<T> readonlyList)
            {
                for (; i < readonlyList.Count; i++)
                {
                    var item = readonlyList[i];
                    if (item == null)
                    {
                        if (throwWhenItemIsNull) throw new CollectionException($"The collection contains null at index {i}.", nameof(enumerable));
                        continue;
                    }

                    action(item);
                }
            }
            else
            {
                foreach (var item in enumerable)
                {
                    if (item == null)
                    {
                        if (throwWhenItemIsNull) throw new CollectionException($"The collection contains null at index {i}.", nameof(enumerable));
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
    }
}