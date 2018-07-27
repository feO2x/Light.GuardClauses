using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    public static partial class Check
    {
        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not have the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>(this TCollection parameter, int count, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) != count)
                Throw.InvalidCollectionCount(parameter, count, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified number of items, or when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>(this TCollection parameter, int count, Func<TCollection, int, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() != count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter;
        }

        /// <summary>
        /// Checks if the specified collection is null or empty.
        /// </summary>
        /// <param name="collection">The collection to be checked.</param>
        /// <returns>True if the collection is null or empty, else false.</returns>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("collection:null => false")]
        public static bool IsNullOrEmpty(this IEnumerable collection) =>
            collection == null || collection.Count() == 0;

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws an <see cref="EmptyCollectionException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="parameter"/> has no items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>(this TCollection parameter, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) == 0)
                Throw.EmptyCollection(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Thrown when <paramref name="parameter"/> has no items, or when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>(this TCollection parameter, Func<Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() == 0)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws a <see cref="MissingItemException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must be part of the collection.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="MissingItemException">Thrown when <paramref name="parameter" /> does not contain <paramref name="item"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>(this TCollection parameter, TItem item, string parameterName = null, string message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                    Throw.MissingItem(parameter, item, parameterName, message);
                return parameter;
            }

            if (!parameter.MustNotBeNull(parameterName, message).Contains(item))
                Throw.MissingItem(parameter, item, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must be part of the collection.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="item"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="item"/>, or when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>(this TCollection parameter, TItem item, Func<TCollection, TItem, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                    Throw.CustomException(exceptionFactory, parameter, item);
                return parameter;
            }

            if (parameter == null || !parameter.Contains(item))
                Throw.CustomException(exceptionFactory, parameter, item);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws an <see cref="ExistingItemException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must not be part of the collection.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ExistingItemException">Thrown when <paramref name="parameter" /> contains <paramref name="item"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>(this TCollection parameter, TItem item, string parameterName = null, string message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                    Throw.ExistingItem(parameter, item, parameterName, message);
                return parameter;
            }

            if (parameter.MustNotBeNull(parameterName, message).Contains(item))
                Throw.ExistingItem(parameter, item, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must not be part of the collection.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="item"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="item"/>.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>(this TCollection parameter, TItem item, Func<TCollection, TItem, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                    Throw.CustomException(exceptionFactory, parameter, item);
                return parameter;
            }

            if (parameter == null || parameter.Contains(item))
                Throw.CustomException(exceptionFactory, parameter, item);
            return parameter;
        }

        /// <summary>
        /// Checks if the given <paramref name="item" /> is one of the specified <paramref name="items" />.
        /// </summary>
        /// <param name="item">The item to be checked.</param>
        /// <param name="items">The collection that might contain the <paramref name="item" />.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static bool IsOneOf<TItem, TCollection>(this TItem item, TCollection items) where TCollection : class, IEnumerable<TItem>
        {
            if (items is ICollection<TItem> collection)
                return collection.Contains(item);

            if (items is string @string && item is char character)
                return @string.IndexOf(character) != -1;

            return items.MustNotBeNull(nameof(items)).Contains(item);
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws a <see cref="ValueIsNotOneOfException"/>.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that should contain the value.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValueIsNotOneOfException">Thrown when <paramref name="parameter"/> is not equal to one of the specified <paramref name="items"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, string parameterName = null, string message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (!parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
                Throw.ValueNotOneOf(parameter, items, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that should contain the value.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="items"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is not equal to one of the specified <paramref name="items"/>, or when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, Func<TItem, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (items == null || !parameter.IsOneOf(items))
                Throw.CustomException(exceptionFactory, parameter, items);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws a <see cref="ValueIsOneOfException"/>.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that must not contain the value.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="ValueIsOneOfException">Thrown when <paramref name="parameter"/> is equal to one of the specified <paramref name="items"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, string parameterName = null, string message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
                Throw.ValueIsOneOf(parameter, items, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that must not contain the value.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="items"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is equal to one of the specified <paramref name="items"/>, or when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, Func<TItem, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (items == null || parameter.IsOneOf(items))
                Throw.CustomException(exceptionFactory, parameter, items);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at least.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not contain at least the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>(this TCollection parameter, int count, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) < count)
                Throw.InvalidMinimumCollectionCount(parameter, count, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at least.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not contain at least the specified number of items, or when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>(this TCollection parameter, int count, Func<TCollection, int, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() < count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at most the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at most.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not contain at most the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMaximumCount<TCollection>(this TCollection parameter, int count, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) > count)
                Throw.InvalidMaximumCollectionCount(parameter, count, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at most the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at most.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not contain at most the specified number of items, or when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMaximumCount<TCollection>(this TCollection parameter, int count, Func<TCollection, int, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() > count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter;
        }
    }
}
