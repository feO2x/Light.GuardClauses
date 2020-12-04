using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>([ValidatedNotNull] this TCollection? parameter, int count, string? parameterName = null, string? message = null) where TCollection : class, IEnumerable
        {
            if (parameter!.Count(parameterName, message) != count)
                Throw.InvalidCollectionCount(parameter!, count, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection must have.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified number of items, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>([ValidatedNotNull] this TCollection? parameter, int count, Func<TCollection?, int, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() != count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter!;
        }

        /// <summary>
        /// Checks if the specified collection is null or empty.
        /// </summary>
        /// <param name="collection">The collection to be checked.</param>
        /// <returns>True if the collection is null or empty, else false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("=> true, collection:canbenull; => false, collection:notnull")]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IEnumerable? collection) =>
            collection == null || collection.Count() == 0;

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws an <see cref="EmptyCollectionException"/>.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="parameter"/> has no items.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>([ValidatedNotNull] this TCollection? parameter, string? parameterName = null, string? message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) == 0)
                Throw.EmptyCollection(parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref="Exception">Thrown when <paramref name="parameter"/> has no items, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>([ValidatedNotNull] this TCollection? parameter, Func<TCollection?, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() == 0)
                Throw.CustomException(exceptionFactory, parameter);
            return parameter!;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>([ValidatedNotNull] this TCollection? parameter, TItem item, string? parameterName = null, string? message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                    Throw.MissingItem(parameter, item, parameterName, message);
                return parameter;
            }

            if (!parameter.MustNotBeNull(parameterName, message).Contains(item))
                Throw.MissingItem(parameter!, item, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must be part of the collection.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="item"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> does not contain <paramref name="item"/>, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>([ValidatedNotNull] this TCollection? parameter, TItem item, Func<TCollection?, TItem, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                    Throw.CustomException(exceptionFactory, parameter, item);
                return parameter;
            }

            if (parameter == null || !parameter.Contains(item))
                Throw.CustomException(exceptionFactory, parameter, item);
            return parameter!;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>([ValidatedNotNull] this TCollection? parameter, TItem item, string? parameterName = null, string? message = null) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                    Throw.ExistingItem(parameter, item, parameterName, message);
                return parameter;
            }

            if (parameter.MustNotBeNull(parameterName, message).Contains(item))
                Throw.ExistingItem(parameter!, item, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="item">The item that must not be part of the collection.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="item"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> contains <paramref name="item"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>([ValidatedNotNull] this TCollection? parameter, TItem item, Func<TCollection?, TItem, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                    Throw.CustomException(exceptionFactory, parameter, item);
                return parameter;
            }

            if (parameter == null || parameter.Contains(item))
                Throw.CustomException(exceptionFactory, parameter, item);
            return parameter!;
        }

        /// <summary>
        /// Checks if the given <paramref name="item" /> is one of the specified <paramref name="items" />.
        /// </summary>
        /// <param name="item">The item to be checked.</param>
        /// <param name="items">The collection that might contain the <paramref name="item" />.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static bool IsOneOf<TItem>(this TItem item, [ValidatedNotNull] IEnumerable<TItem> items)
        {
            if (items is ICollection<TItem> collection)
                return collection.Contains(item);

            if (items is string @string && item is char character)
                return @string.IndexOf(character) != -1;

            return items.MustNotBeNull(nameof(items)).ContainsViaForeach(item);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem>(this TItem parameter, [ValidatedNotNull] IEnumerable<TItem> items, string? parameterName = null, string? message = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (!parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
                Throw.ValueNotOneOf(parameter, items, parameterName, message);
            return parameter;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that should contain the value.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="items"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is not equal to one of the specified <paramref name="items"/>, or when <paramref name="items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem, TCollection>(this TItem parameter, [ValidatedNotNull] TCollection items, Func<TItem, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
            if (items is null || !parameter.IsOneOf(items))
                Throw.CustomException(exceptionFactory, parameter, items!);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem>(this TItem parameter, [ValidatedNotNull] IEnumerable<TItem> items, string? parameterName = null, string? message = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
                Throw.ValueIsOneOf(parameter, items, parameterName, message);
            return parameter;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The value to be checked.</param>
        /// <param name="items">The items that must not contain the value.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="items"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is equal to one of the specified <paramref name="items"/>, or when <paramref name="items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem, TCollection>(this TItem parameter, [ValidatedNotNull] TCollection items, Func<TItem, TCollection, Exception> exceptionFactory) where TCollection : class, IEnumerable<TItem>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse - caller might have NRTs turned off
            if (items is null || parameter.IsOneOf(items))
                Throw.CustomException(exceptionFactory, parameter, items!);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>([ValidatedNotNull] this TCollection? parameter, int count, string? parameterName = null, string? message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) < count)
                Throw.InvalidMinimumCollectionCount(parameter!, count, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at least.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not contain at least the specified number of items, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>([ValidatedNotNull] this TCollection? parameter, int count, Func<TCollection?, int, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() < count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter!;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMaximumCount<TCollection>([ValidatedNotNull] this TCollection? parameter, int count, string? parameterName = null, string? message = null) where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) > count)
                Throw.InvalidMaximumCollectionCount(parameter!, count, parameterName, message);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the collection has at most the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The collection to be checked.</param>
        /// <param name="count">The number of items the collection should have at most.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="count"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not contain at most the specified number of items, or when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMaximumCount<TCollection>([ValidatedNotNull] this TCollection? parameter, int count, Func<TCollection?, int, Exception> exceptionFactory) where TCollection : class, IEnumerable
        {
            if (parameter == null || parameter.Count() > count)
                Throw.CustomException(exceptionFactory, parameter, count);
            return parameter!;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length that the span must have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustHaveLength<T>(this Span<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length != length)
                Throw.InvalidSpanLength(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length that the span must have.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustHaveLength<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length != length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length that the span must have.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustHaveLength<T>(this ReadOnlySpan<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length != length)
                Throw.InvalidSpanLength(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length that the span must have.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustHaveLength<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length != length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The value that the span must be longer than.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is shorter than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThan<T>(this Span<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length <= length)
                Throw.SpanMustBeLongerThan(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be longer than.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is shorter than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThan<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length <= length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The value that the span must be longer than.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is shorter than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThan<T>(this ReadOnlySpan<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length <= length)
                Throw.SpanMustBeLongerThan(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be longer than.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is shorter than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThan<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length <= length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The value that the span must be longer than or equal to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is shorter than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThanOrEqualTo<T>(this Span<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length < length)
                Throw.SpanMustBeLongerThanOrEqualTo(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The value that the span must be longer than or equal to.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is shorter than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThanOrEqualTo<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length < length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The value that the span must be longer than or equal to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is shorter than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length < length)
                Throw.SpanMustBeLongerThanOrEqualTo(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The value that the span must be longer than or equal to.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is shorter than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length < length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThan<T>(this Span<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length >= length)
                Throw.SpanMustBeShorterThan(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThan<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length >= length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThan<T>(this ReadOnlySpan<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length >= length)
                Throw.SpanMustBeShorterThan(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than or equal to <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThan<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length >= length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than or equal to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThanOrEqualTo<T>(this Span<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length > length)
                Throw.SpanMustBeShorterThanOrEqualTo(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than or equal to.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThanOrEqualTo<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length > length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws an <see cref="InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than or equal to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref="InvalidCollectionCountException">Thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, string? parameterName = null, string? message = null)
        {
            if (parameter.Length > length)
                Throw.SpanMustBeShorterThanOrEqualTo(parameter, length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The span to be checked.</param>
        /// <param name="length">The length value that the span must be shorter than or equal to.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="length"/> are passed to it.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> is longer than <paramref name="length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length > length)
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            return parameter;
        }
    }
}
