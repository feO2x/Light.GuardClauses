using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The <see cref="EnumerableAssertions" /> class contains extension methods that apply assertions to <see cref="IEnumerable{T}" /> instances.
    /// </summary>
    public static class EnumerableAssertions
    {
        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is one of the specified <paramref name="items" />, or otherwise throws a <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="items">The items where <paramref name="parameter" /> must be part of.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
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
        public static T MustBeOneOf<T>(this T parameter, IEnumerable<T> items, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            items.MustNotBeNull(nameof(items));
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (items.Contains(parameter, equalityComparer))
                return parameter;

            throw exception?.Invoke() ??
                  new ArgumentOutOfRangeException(parameterName,
                                                  parameter,
                                                  message ??
                                                  new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be one of the items")
                                                                     .AppendItemsWithNewLine(items).AppendLine()
                                                                     .AppendLine($"but you specified {parameter}.")
                                                                     .ToString());

            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that <paramref name="parameter" /> is not one of the specified <paramref name="items" />, or otherwise throws a <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="items">The items where <paramref name="parameter" /> must not be part of.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
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
        public static T MustNotBeOneOf<T>(this T parameter, IEnumerable<T> items, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            items.MustNotBeNull(nameof(items));
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (items.Contains(parameter, equalityComparer) == false)
                return parameter;

            throw exception?.Invoke() ??
                  new ArgumentOutOfRangeException(parameterName,
                                                  parameter,
                                                  message ??
                                                  new StringBuilder().AppendLine($"{parameterName ?? "The value"} must not be one of the items")
                                                                     .AppendItemsWithNewLine(items).AppendLine()
                                                                     .AppendLine($"but you specified {parameter}.")
                                                                     .ToString());
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
        public static IEnumerable<T> MustNotBeNullOrEmpty<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Any())
                return parameter;

            throw exception != null ? exception() : (message == null ? new EmptyCollectionException(parameterName) : new EmptyCollectionException(message, parameterName));
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Checks if the specified collection is null or empty.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <returns>True if the collection is null or empty, else false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> parameter)
        {
            if (parameter == null)
                return true;

            return !parameter.Any();
        }

        /// <summary>
        ///     Ensures that the specified collection has unique items, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
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
        public static IEnumerable<T> MustNotContainDuplicates<T>(this IEnumerable<T> parameter, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var collection = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (collection.Count == 0)
                return parameter;

            for (var i = 0; i < collection.Count; i++)
            {
                var itemToCompare = collection[i];
                for (var j = i + 1; j < collection.Count; j++)
                {
                    if (equalityComparer.EqualsWithHashCode(itemToCompare, collection[j]) == false)
                        continue;

                    throw exception?.Invoke() ??
                          new CollectionException(message ??
                                                  new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be a collection with unique items, but there is a duplicate at index {j}.")
                                                                     .AppendCollectionContent(collection)
                                                                     .ToString(),
                                                  parameterName);
                }
            }
            return parameter;
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
        public static IEnumerable<T> MustNotContainNull<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            // ReSharper disable PossibleMultipleEnumeration
            var collection = parameter.MustNotBeNull(parameterName).AsReadOnlyList();

            for (var i = 0; i < collection.Count; i++)
            {
                if (collection[i] != null)
                    continue;

                throw exception?.Invoke() ??
                      new CollectionException(message ??
                                              new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be a collection not containing null, but you specified null at index {i}.")
                                                                 .AppendCollectionContent(parameter)
                                                                 .ToString(),
                                              parameterName);
            }
            return parameter;
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
        public static IEnumerable<T> MustContain<T>(this IEnumerable<T> parameter, T item, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(item))
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must contain value \"{item.ToStringOrNull()}\", but does not.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
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
        public static IEnumerable<T> MustNotContain<T>(this IEnumerable<T> parameter, T item, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(item) == false)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder($"{parameterName ?? "The collection"} must not contain value \"{item.ToStringOrNull()}\", but it does.")
                                              .AppendCollectionContent(parameter)
                                              .ToString(),
                                          parameterName);
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
        public static IEnumerable<T> MustBeSubsetOf<T>(this IEnumerable<T> parameter, IEnumerable<T> superset, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            superset.MustNotBeNullOrEmpty(nameof(superset));

            if (parameter.All(superset.Contains))
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must be a subset of:")
                                                             .AppendItemsWithNewLine(superset).AppendLine()
                                                             .AppendLine("but it is not.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Checks if all items of <paramref name="enumerable" /> are part of <paramref name="superset" />. This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="enumerable">The collection to be checked.</param>
        /// <param name="superset">The collection that can be the superset of <paramref name="enumerable" />.</param>
        /// <returns>True when all items of <paramref name="enumerable" /> are also contained in <paramref name="superset" /> or when both collections are empty, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="superset" /> are null.</exception>
        public static bool IsSubsetOf<T>(this IEnumerable<T> enumerable, IEnumerable<T> superset)
        {
            // ReSharper disable PossibleMultipleEnumeration
            enumerable.MustNotBeNull(nameof(enumerable));
            superset.MustNotBeNull(nameof(superset));

            return enumerable.All(superset.Contains);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Checks if all items of <paramref name="enumerable" /> are part of <paramref name="superset" />. This method is not aware of duplicates.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="enumerable">The collection to be checked.</param>
        /// <param name="superset">The collection that can be the superset of <paramref name="enumerable" />.</param>
        /// <returns>True when all items of <paramref name="enumerable" /> are also contained in <paramref name="superset" /> or when both collections are empty, else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> or <paramref name="superset" /> are null.</exception>
        public static bool IsSubsetOf<T>(this IEnumerable<T> enumerable, params T[] superset)
        {
            return enumerable.IsSubsetOf((IEnumerable<T>) superset);
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
        public static IEnumerable<T> MustContain<T>(this IEnumerable<T> parameter, IEnumerable<T> subset, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            subset.MustNotBeNullOrEmpty(nameof(subset));

            if (subset.All(parameter.Contains))
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must contain the following values:")
                                                             .AppendItemsWithNewLine(subset).AppendLine()
                                                             .AppendLine("but it does not.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
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
        public static IEnumerable<T> MustContain<T>(this IEnumerable<T> parameter, params T[] subset)
        {
            return MustContain(parameter, (IEnumerable<T>) subset);
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
        public static IEnumerable<T> MustNotContain<T>(this IEnumerable<T> parameter, IEnumerable<T> set, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            set.MustNotBeNullOrEmpty(nameof(set));

            if (set.Any(parameter.Contains) == false)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not contain any of the following values:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
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
        public static IEnumerable<T> MustNotContain<T>(this IEnumerable<T> parameter, params T[] set)
        {
            return MustNotContain(parameter, (IEnumerable<T>) set);
        }

        /// <summary>
        ///     Ensures that the collection has the specified count of items, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The type of the items of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The count that the collection should have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not have the specified <paramref name="count" /> (optional).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not have the specified <paramref name="count" /> and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count" /> is less than zero.</exception>
        public static IEnumerable<T> MustHaveCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull();
            count.MustNotBeLessThan(0, nameof(count));

            if (parameter.Count() == count)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must have count {count}, but you specified a collection with count {parameter.Count()}.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
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
        public static IEnumerable<T> MustHaveMinimumCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            count.MustNotBeLessThan(0);

            var collectionCount = parameter.Count();
            if (collectionCount >= count)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must have at least {count} {Items(count)}, but it actually has {collectionCount} {Items(collectionCount)}.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
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
        public static IEnumerable<T> MustHaveMaximumCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            count.MustNotBeLessThan(0);

            var collectionCount = parameter.Count();
            if (collectionCount <= count)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must have no more than {count} {Items(count)}, but it actually has {collectionCount} {Items(collectionCount)}.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection contains only instances of different subtypes / subclasses, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection. This usually should be an interface / a base class type.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> contains at least two instances of the same subtype.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> contains two or more instances of the same subtype, or when <paramref name="parameter" /> contains an element that is null, and no <paramref name="exception" /> is specified.</exception>
        public static IEnumerable<T> MustContainInstancesOfDifferentTypes<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            // ReSharper disable PossibleMultipleEnumeration
            var list = parameter.AsReadOnlyList();

            var hashSet = new HashSet<Type>();
            for (var i = 0; i < list.Count; i++)
            {
                var currentItem = list[i];
                if (currentItem == null)
                    throw new CollectionException(new StringBuilder().AppendLine($"{parameterName ?? "The collection"} contains null at index {i}.")
                                                                     .AppendLine().AppendCollectionContent(list)
                                                                     .ToString(),
                                                  parameterName);
                var type = currentItem.GetType();
                if (hashSet.Add(type))
                    continue;

                throw exception?.Invoke() ??
                      new CollectionException(message ??
                                              new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must contain instances of different subtypes, but \"{type}\" occurs a second time at index \"{i}\".")
                                                                 .AppendLine().AppendCollectionContent(list)
                                                                 .ToString(),
                                              parameterName);
            }

            return parameter;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Checks if the specified collection contains instances of different types.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="enumerable">The collection to be checked.</param>
        /// <returns>True if all instances in the collection have a unique type (no items with same type), else false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="enumerable" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when any item in the collection is null.</exception>
        public static bool IsContainingInstancesOfDifferentTypes<T>(this IEnumerable<T> enumerable) where T : class
        {
            var list = enumerable.MustNotBeNull(nameof(enumerable)).AsReadOnlyList();

            var hashSet = new HashSet<Type>();
            for (var i = 0; i < list.Count; i++)
            {
                var currentItem = list[i];
                if (currentItem == null)
                    throw new CollectionException(new StringBuilder().AppendLine($"The collection contains null at index {i}.")
                                                                     .AppendLine().AppendCollectionContent(list)
                                                                     .ToString(),
                                                  nameof(enumerable));
                var type = currentItem.GetType();
                if (hashSet.Add(type))
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        ///     Ensures that the specified collection starts with the given set (same order), or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items that the collection must start with.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection does not contain the given set at the start of it.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not start with the items of <paramref name="set" /> (same order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> is an empty collection.</exception>
        public static IEnumerable<T> MustStartWith<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                goto ThrowException;

            for (var i = 0; i < second.Count; i++)
            {
                if (equalityComparer.EqualsWithHashCode(first[i], second[i]) == false)
                    goto ThrowException;
            }
            return parameter;

            ThrowException:
            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must start with the following items:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does not.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection ends with the given set (same order), or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items that the collection must end with.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection does not contain the given set at the end of it.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not start with the items of <paramref name="set" /> (same order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> is an empty collection.</exception>
        public static IEnumerable<T> MustEndWith<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                goto ThrowException;

            for (var i = 0; i < second.Count; i++)
            {
                var targetIndex = first.Count - second.Count + i;
                if (equalityComparer.EqualsWithHashCode(first[targetIndex], second[i]) == false)
                    goto ThrowException;
            }
            return parameter;

            ThrowException:
            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must end with the following items:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does not.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);

            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection does not start with the given set (same order), or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items the collection must not start with.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection contains the given set at the start of it.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does start with the items of <paramref name="set" /> (same order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> is empty.</exception>
        public static IEnumerable<T> MustNotStartWith<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                return parameter;

            for (var i = 0; i < second.Count; i++)
            {
                if (equalityComparer.EqualsWithHashCode(first[i], second[i]) == false)
                    return parameter;
            }

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not start with the following items:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the specified collection does not end with the given set (same order), or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items the collection must not end with.</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection contains the given set at the end of it.
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does contain the given set at the end of it, and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> is an empty collection.</exception>
        public static IEnumerable<T> MustNotEndWith<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                return parameter;

            for (var i = 0; i < second.Count; i++)
            {
                var targetIndex = first.Count - second.Count + i;
                if (equalityComparer.EqualsWithHashCode(first[targetIndex], second[i]) == false)
                    return parameter;
            }

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not end with the following items:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection starts with the specified items of <paramref name="set" /> in any order, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items the collection must start with (in any order).</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection does not start with the given items (in any order).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not start with the given items (in any order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> contains no items.</exception>
        public static IEnumerable<T> MustStartWithEquivalentOf<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                goto ThrowException;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < second.Count; i++)
            {
                if (ContainsAtStart(first, second.Count, second[i], equalityComparer) == false)
                    goto ThrowException;
            }
            return parameter;

            ThrowException:
            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must start with the following items in any order:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does not.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection does not start with the specified items of <paramref name="set" /> in any order, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items the collection must not start with (in any order).</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection does start with the given items (in any order).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not start with the given items (in any order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> contains no items.</exception>
        public static IEnumerable<T> MustNotStartWithEquivalentOf<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                return parameter;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < second.Count; i++)
            {
                if (ContainsAtStart(first, second.Count, second[i], equalityComparer) == false)
                    return parameter;
            }

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not start with the following items in any order:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        private static bool ContainsAtStart<T>(IReadOnlyList<T> collection, int upperIndex, T item, IEqualityComparer<T> equalityComparer)
        {
            for (var i = 0; i < upperIndex; i++)
            {
                if (equalityComparer.EqualsWithHashCode(collection[i], item))
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Ensures that the collection ends with the specified items of <paramref name="set" /> in any order, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items the collection must end with (in any order).</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection does not end with the given items (in any order).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not end with the given items (in any order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> contains no items.</exception>
        public static IEnumerable<T> MustEndWithEquivalentOf<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                goto ThrowException;

            var lowerIndex = first.Count - second.Count;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < second.Count; i++)
            {
                if (ContainsAtEnd(first, lowerIndex, second[i], equalityComparer) == false)
                    goto ThrowException;
            }
            return parameter;

            ThrowException:
            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must end with the following items in any order:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does not.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Ensures that the collection does not end with the specified items of <paramref name="set" /> in any order, or otherwise throws a <see cref="CollectionException" />.
        /// </summary>
        /// <typeparam name="T">The item type of the collection.</typeparam>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="set">The items the collection must not end with (in any order).</param>
        /// <param name="equalityComparer">The equality comparer that is used to compare items (optional). If null is specified, then <see cref="EqualityComparer{T}.Default" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="CollectionException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that will be thrown when the collection does end with the given items (in any order).
        ///     Please note that <paramref name="parameterName" /> and <paramref name="message" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="set" /> is null.</exception>
        /// <exception cref="CollectionException">Thrown when <paramref name="parameter" /> does not end with the given items (in any order), and no <paramref name="exception" /> is specified.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="set" /> contains no items.</exception>
        public static IEnumerable<T> MustNotEndWithEquivalentOf<T>(this IEnumerable<T> parameter, IEnumerable<T> set, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var first = parameter.MustNotBeNull(parameterName).AsReadOnlyList();
            var second = set.MustNotBeNullOrEmpty(nameof(set)).AsReadOnlyList();
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (first.Count < second.Count)
                return parameter;

            var lowerIndex = first.Count - second.Count;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < second.Count; i++)
            {
                if (ContainsAtEnd(first, lowerIndex, second[i], equalityComparer) == false)
                    return parameter;
            }
            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not end with the following items in any order:")
                                                             .AppendItemsWithNewLine(set).AppendLine()
                                                             .AppendLine("but it does.").AppendLine()
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }

        private static bool ContainsAtEnd<T>(IReadOnlyList<T> collection, int lowerIndex, T item, IEqualityComparer<T> equalityComparer)
        {
            for (var i = lowerIndex; i < collection.Count; i++)
            {
                if (equalityComparer.EqualsWithHashCode(collection[i], item))
                    return true;
            }
            return false;
        }

        private static string Items(int count)
        {
            return count == 1 ? "item" : "items";
        }
    }
}