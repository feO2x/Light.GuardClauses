using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}