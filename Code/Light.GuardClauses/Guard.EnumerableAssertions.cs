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
    public static partial class Guard
    {
        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidCollectionCountException" /> (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not have the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>(this TCollection parameter, int count, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName) != count)
                Throw.InvalidCollectionCount(parameter, count, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>(this TCollection parameter, int count, Func<TCollection, int, Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName) != count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter;
        }

        

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws an <see cref="EmptyCollectionException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="EmptyCollectionException" /> (optional).</param>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="parameter"/> has no items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>(this TCollection parameter, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName) == 0)
                Throw.EmptyCollection(parameter, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws an <see cref="EmptyCollectionException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="parameter"/> has no items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>(this TCollection parameter, Func<Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName) == 0)
                Throw.CustomException(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws a <see cref="MissingItemException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must be part of the collection.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="MissingItemException" /> (optional).</param>
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

            if (!parameter.MustNotBeNull(parameterName).Contains(item))
                Throw.MissingItem(parameter, item, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must be part of the collection.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="item"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="item"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>(this TCollection parameter, TItem item, Func<TCollection, TItem, Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                    Throw.CustomException(exceptionFactory, parameter, item);
                return parameter;
            }

            if (!parameter.MustNotBeNull(parameterName).Contains(item))
                Throw.CustomException(exceptionFactory, parameter, item);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws an <see cref="ExistingItemException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must not be part of the collection.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ExistingItemException" /> (optional).</param>
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

            if (parameter.MustNotBeNull(parameterName).Contains(item))
                Throw.ExistingItem(parameter, item, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must not be part of the collection.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="item"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="item"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>(this TCollection parameter, TItem item, Func<TCollection, TItem, Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                    Throw.CustomException(exceptionFactory, parameter, item);
                return parameter;
            }

            if (parameter.MustNotBeNull(parameterName).Contains(item))
                Throw.CustomException(exceptionFactory, parameter, item);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws a <see cref="ValueIsNotOneOfException"/>.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that should contain the value.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ValueIsNotOneOfException" /> (optional).</param>
        /// <exception cref="ValueIsNotOneOfException">Thrown when <paramref name="parameter"/> is not equal to one of the specified <paramref name="items"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, string parameterName = null, string message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (items is ICollection<TItem> collection)
            {
                if (!collection.Contains(parameter))
                    Throw.ValueNotOneOf(parameter, items, parameterName, message);
                return parameter;
            }

            if (!items.MustNotBeNull(nameof(items)).Contains(parameter))
                Throw.ValueNotOneOf(parameter, items, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that should contain the value.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="items"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is not equal to one of the specified <paramref name="items"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, Func<TItem, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (items is ICollection<TItem> collection)
            {
                if (!collection.Contains(parameter))
                    Throw.CustomException(exceptionFactory, parameter, items);
                return parameter;
            }

            if (!items.MustNotBeNull(nameof(items)).Contains(parameter))
                Throw.CustomException(exceptionFactory, parameter, items);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws a <see cref="ValueIsOneOfException"/>.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that must not contain the value.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ValueIsOneOfException" /> (optional).</param>
        /// <exception cref="ValueIsOneOfException">Thrown when <paramref name="parameter"/> is equal to one of the specified <paramref name="items"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, string parameterName = null, string message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (items is ICollection<TItem> collection)
            {
                if (collection.Contains(parameter))
                    Throw.ValueIsOneOf(parameter, items, parameterName, message);
                return parameter;
            }

            if (items.MustNotBeNull(nameof(items)).Contains(parameter))
                Throw.ValueIsOneOf(parameter, items, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that must not contain the value.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="items"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is equal to one of the specified <paramref name="items"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem, TCollection>(this TItem parameter, TCollection items, Func<TItem, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (items is ICollection<TItem> collection)
            {
                if (collection.Contains(parameter))
                    Throw.CustomException(exceptionFactory, parameter, items);
                return parameter;
            }

            if (items.MustNotBeNull(nameof(items)).Contains(parameter))
                Throw.CustomException(exceptionFactory, parameter, items);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at least.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="InvalidCollectionCountException" /> (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not contain at least the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>(this TCollection parameter, int count, string parameterName = null, string message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName) < count)
                Throw.InvalidMinimalCollectionCount(parameter, count, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at least.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not contain at least the specified number of items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>(this TCollection parameter, int count, Func<TCollection, int, Exception> exceptionFactory, string parameterName = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName) < count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter;
        }
    }
}
