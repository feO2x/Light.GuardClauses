using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The EnumerableAssertions class contains extension methods that apply assertions to <see cref="IEnumerable{T}" /> instances.
    /// </summary>
    public static class EnumerableAssertions
    {
        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is one of the specified <paramref name="items" />, or otherwise throws a <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="items">The items where <paramref name="parameter" /> must be part of.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is not part of <paramref name="items" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="parameter" /> is not part of <paramref name="items" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeOneOf<T>(this T parameter, IEnumerable<T> items, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            items.MustNotBeNull(nameof(items));

            if (items.Contains(parameter) == false)
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be one of the items{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(items)}{Environment.NewLine}but you specified {parameter}.");
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is not one of the specified <paramref name="items" />, or otherwise throws a <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="items">The items where <paramref name="parameter" /> must not be part of.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is part of <paramref name="items" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="parameter" /> is part of <paramref name="items" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeOneOf<T>(this T parameter, IEnumerable<T> items, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            items.MustNotBeNull(nameof(items));

            if (items.Contains(parameter))
                throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be none of the items{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(items)}{Environment.NewLine}but you specified {parameter}.");
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection is not null or empty, or otherwise throws an <see cref="EmptyCollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="EmptyCollectionException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> is empty (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        /// <exception cref="EmptyCollectionException">
        ///     Thrown when <paramref name="parameter" /> is empty and no <paramref name="exception" /> is specified.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotBeNullOrEmpty<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Any() == false)
                throw exception != null ? exception() : (message == null ? new EmptyCollectionException(parameterName) : new EmptyCollectionException(message, parameterName));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection has unique items, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> does not have unique items (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">
        ///     Thrown when <paramref name="parameter" /> has at least two equal items in it and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainDuplicates<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            var count = parameter.Count();
            if (count == 0)
                return;

            for (var i = 0; i < count; i++)
            {
                var itemToCompare = parameter.ElementAt(i);
                for (var j = i + 1; j < count; j++)
                {
                    if (itemToCompare.EqualsWithHashCode(parameter.ElementAt(j)) == false)
                        continue;

                    throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The value"} must be a collection with unique items, but there is a duplicate at index {j}.{Environment.NewLine}Actual content of the collection:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}.", parameterName);
                }
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection does not contain any item that is null, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection. This must be a Reference Type.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="CollectionException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when the specified <paramref name="parameter" /> has at least one item that is null (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">
        ///     Thrown when <paramref name="parameter" />contains at least one item that is null and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContainNull<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            var currentIndex = -1;
            foreach (var item in parameter)
            {
                currentIndex++;
                if (item != null)
                    continue;

                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The value"} must be a collection not containing null, but you specified null at index {currentIndex}.{Environment.NewLine}Actual content of the collection:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection contains the specified <paramref name="item" />, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that should be part of the collection's items.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="CollectionException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not contain <paramref name="item" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">
        ///     Thrown when <paramref name="parameter" /> does not contain the specified <paramref name="item" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContain<T>(this IEnumerable<T> parameter, T item, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(item) == false)
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must contain value \"{item.ToStringOrNull()}\", but does not.{Environment.NewLine}Actual content of the collection:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection does not contain the specified <paramref name="item" />, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that should not be part of the collection's items.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="CollectionException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does contain <paramref name="item" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">
        ///     Thrown when <paramref name="parameter" /> does contain the specified <paramref name="item" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContain<T>(this IEnumerable<T> parameter, T item, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(item))
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must not contain value \"{item.ToStringOrNull()}\", but it does.{Environment.NewLine}Actual content of the collection:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection is a subset of <paramref name="superset" />, or otherwise throws a <see cref="CollectionException" />. This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="superset">The collection that <paramref name="parameter" /> must be a subset of.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that is injected into the <see cref="CollectionException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not a subset of <paramref name="superset" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">
        ///     Thrown when <paramref name="parameter" /> is not part of <paramref name="superset" /> and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> or <paramref name="superset" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustBeSubsetOf<T>(this IEnumerable<T> parameter, IEnumerable<T> superset, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            superset.MustNotBeNull(nameof(superset));

            if (parameter.All(superset.Contains) == false)
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must be a subset of:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(superset)}{Environment.NewLine}{Environment.NewLine}The actual collection contains:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);

            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection contains the specified subset, or otherwise throws a <see cref="CollectionException" />.
        ///     This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="subset">The subset that must be part of the collection.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="CollectionException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not a superset of <paramref name="subset" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">
        ///     Thrown when there is any item of <paramref name="subset" /> that <paramref name="parameter" /> does not contain and no <paramref name="exception" /> is specified.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> or <paramref name="subset" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContain<T>(this IEnumerable<T> parameter, IEnumerable<T> subset, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            subset.MustNotBeNull(nameof(subset));

            if (subset.All(parameter.Contains) == false)
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must contain the following values{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(subset)}{Environment.NewLine}but does not.{Environment.NewLine}Actual content of the collection:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection contains the specified subset, or otherwise throws a <see cref="CollectionException" />. This method is not aware of duplicates and uses the default exception.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="subset">The subset that must be part of the collection.</param>
        /// <exception cref="CollectionException">
        ///     Thrown when there is any item of <paramref name="subset" /> that <paramref name="parameter" /> does not contain.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="parameter" /> or <paramref name="subset" /> is null.
        /// </exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustContain<T>(this IEnumerable<T> parameter, params T[] subset)
        {
            MustContain(parameter, (IEnumerable<T>) subset);
        }

        /// <summary>
        ///     Ensures that the collection does not contain the specified set of values, or otherwise throws a <see cref="CollectionException" />. This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The set that must not be part of the collection.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> contains any items of <paramref name="set" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> contains any items of <paramref name="set" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContain<T>(this IEnumerable<T> parameter, IEnumerable<T> set, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            set.MustNotBeNull(nameof(set));

            if (set.Any(parameter.Contains))
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must not contain any of the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(set)}{Environment.NewLine}{Environment.NewLine}The actual content of the collection is:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection does not contain the specified set of values, or otherwise throws a <see cref="CollectionException" />. This method is not aware of duplicates and uses the default exception.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The set that must not be part of the collection.</param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> contains any items of <paramref name="set" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustNotContain<T>(this IEnumerable<T> parameter, params T[] set)
        {
            MustNotContain(parameter, (IEnumerable<T>) set);
        }

        /// <summary>
        ///     Ensures that the collection has the specified count of items, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The count that the collection should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="CollectionException" />.</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not have the specified <paramref name="count" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not have the specified <paramref name="count" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count" /> is less than zero.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull();
            count.MustNotBeLessThan(0, nameof(count));

            if (parameter.Count() != count)
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must have count {count}, but you specified a collection with count {parameter.Count()}.{Environment.NewLine}Actual content of the collection:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(parameter)}", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection has at least the specified number of items, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should at least have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not at least have the specified number of items.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not have at least the specified number of items and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count" /> is less than zero.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveMinimumCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            count.MustNotBeLessThan(0);

            var collectionCount = parameter.Count();
            if (collectionCount < count)
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must have at least {count} {Items(count)}, but it actually has {collectionCount} {Items(collectionCount)}.", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection has no more than the specified number of items, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at most.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message to be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> contains more than the specified number of items.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> contains more than the specified number of items and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count" /> is less than zero.</exception>
        [Conditional(Check.CompileAssertionsSymbol)]
        public static void MustHaveMaximumCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            count.MustNotBeLessThan(0);

            var collectionCount = parameter.Count();
            if (collectionCount > count)
                throw exception != null ? exception() : new CollectionException(message ?? $"{parameterName ?? "The collection"} must have no more than {count} {Items(count)}, but it actually has {collectionCount} {Items(collectionCount)}.", parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        private static string Items(int count)
        {
            return count == 1 ? "item" : "items";
        }
    }
}