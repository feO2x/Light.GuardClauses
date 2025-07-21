/* ------------------------------
   Light.GuardClauses 13.1.0
   ------------------------------

License information for Light.GuardClauses

The MIT License (MIT)
Copyright (c) 2016, 2025 Kenny Pflug mailto:kenny.pflug@live.de

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.FrameworkExtensions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

#nullable enable annotations
namespace Light.GuardClauses
{
    /// <summary>
    /// The <see cref = "Check"/> class provides access to all assertions of Light.GuardClauses.
    /// </summary>
     // ReSharper disable once RedundantTypeDeclarationBody -- required for Source Code Transformation
    internal static class Check
    {
        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws a <see cref = "RelativeUriException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "RelativeUriException">Thrown when <paramref name = "parameter"/> is not an absolute URI.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeAbsoluteUri([NotNull][ValidatedNotNull] this Uri? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IsAbsoluteUri == false)
            {
                Throw.MustBeAbsoluteUri(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI is an absolute one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates the exception to be thrown. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not an absolute URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeAbsoluteUri([NotNull][ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.IsAbsoluteUri == false)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the given <paramref name = "item"/> is one of the specified <paramref name = "items"/>.
        /// </summary>
        /// <param name = "item">The item to be checked.</param>
        /// <param name = "items">The collection that might contain the <paramref name = "item"/>.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        // ReSharper disable once RedundantNullableFlowAttribute - the attribute has an effect, see Issue72NotNullAttribute tests
        public static bool IsOneOf<TItem>(this TItem item, [NotNull][ValidatedNotNull] IEnumerable<TItem> items)
        {
            if (items is ICollection<TItem> collection)
            {
                return collection.Contains(item);
            }

            if (items is string @string && item is char character)
            {
                return @string.IndexOf(character) != -1;
            }

            return items.MustNotBeNull(nameof(items)).ContainsViaForeach(item);
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws an <see cref = "ExistingItemException"/>.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "item">The item that must not be part of the collection.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ExistingItemException">Thrown when <paramref name = "parameter"/> contains <paramref name = "item"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>([NotNull][ValidatedNotNull] this TCollection? parameter, TItem item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                {
                    Throw.ExistingItem(parameter, item, parameterName, message);
                }

                return parameter;
            }

            if (parameter.MustNotBeNull(parameterName, message).Contains(item))
            {
                Throw.ExistingItem(parameter, item, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection does not contain the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "item">The item that must not be part of the collection.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "item"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> contains <paramref name = "item"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotContain<TCollection, TItem>([NotNull][ValidatedNotNull] this TCollection? parameter, TItem item, Func<TCollection?, TItem, Exception> exceptionFactory)
            where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (collection.Contains(item))
                {
                    Throw.CustomException(exceptionFactory, parameter, item);
                }

                return parameter;
            }

            if (parameter is null || parameter.Contains(item))
            {
                Throw.CustomException(exceptionFactory, parameter, item);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The string that must not be part of <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> contains <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain([NotNull][ValidatedNotNull] this string? parameter, string value, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Contains(value.MustNotBeNull(nameof(value), message)))
            {
                Throw.StringContains(parameter, value, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The string that must not be part of <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception (optional). <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> contains <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain([NotNull][ValidatedNotNull] this string? parameter, string value, Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || parameter.Contains(value))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The string that must not be part of <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> contains <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IndexOf(value.MustNotBeNull(nameof(value), message), comparisonType) >= 0)
            {
                Throw.StringContains(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not contain the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The string that must not be part of <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception (optional). <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> contains <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotContain([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || parameter.IndexOf(value, comparisonType) >= 0)
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/> does not contain the specified item, or otherwise throws an <see cref = "ExistingItemException"/>.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "item">The item that must not be part of the <see cref = "ImmutableArray{T}"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ExistingItemException">Thrown when <paramref name = "parameter"/> contains <paramref name = "item"/>.</exception>
        /// <remarks>
        /// The default instance of <see cref = "ImmutableArray{T}"/> cannot contain any items, so this method will not throw for default instances.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustNotContain<T>(this ImmutableArray<T> parameter, T item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsDefault && parameter.Contains(item))
            {
                Throw.ExistingItem(parameter, item, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/> does not contain the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "item">The item that must not be part of the <see cref = "ImmutableArray{T}"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "item"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> contains <paramref name = "item"/>.</exception>
        /// <remarks>
        /// The default instance of <see cref = "ImmutableArray{T}"/> cannot contain any items, so this method will not throw for default instances.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static ImmutableArray<T> MustNotContain<T>(this ImmutableArray<T> parameter, T item, Func<ImmutableArray<T>, T, Exception> exceptionFactory)
        {
            if (!parameter.IsDefault && parameter.Contains(item))
            {
                Throw.CustomException(exceptionFactory, parameter, item);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified string is trimmed at the end, i.e. it does not end with
        /// white space characters. Inputting an empty string will return true.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "regardNullAsTrimmed">
        /// The value indicating whether true or false should be returned from this method when the
        /// <paramref name = "parameter"/> is null. The default value is true.
        /// </param>
        /// <returns>
        /// True if the <paramref name = "parameter"/> is trimmed at the end, else false.
        /// An empty string will result in true.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrimmedAtEnd(this string? parameter, bool regardNullAsTrimmed = true) => parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtEnd();
        /// <summary>
        /// Checks if the specified character span is trimmed at the end, i.e. it does not end with
        /// white space characters. Inputting an empty span will return true.
        /// </summary>
        /// <param name = "parameter">The character span to be checked.</param>
        /// <returns>
        /// True if the <paramref name = "parameter"/> is trimmed at the end, else false.
        /// An empty span will result in true.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrimmedAtEnd(this ReadOnlySpan<char> parameter) => parameter.Length == 0 || !parameter[parameter.Length - 1].IsWhiteSpace();
        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "count">The number of items the collection must have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> does not have the specified number of items.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where TCollection : class, IEnumerable
        {
            if (parameter!.Count(parameterName, message) != count)
            {
                Throw.InvalidCollectionCount(parameter, count, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "count">The number of items the collection must have.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "count"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not have the specified number of items, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveCount<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, int count, Func<TCollection?, int, Exception> exceptionFactory)
            where TCollection : class, IEnumerable
        {
            if (parameter is null || parameter.Count() != count)
            {
                Throw.CustomException(exceptionFactory, parameter, count);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws an <see cref = "EmptyCollectionException"/>.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "EmptyCollectionException">Thrown when <paramref name = "parameter"/> has no items.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) == 0)
            {
                Throw.EmptyCollection(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection is not null or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "Exception">Thrown when <paramref name = "parameter"/> has no items, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustNotBeNullOrEmpty<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, Func<TCollection?, Exception> exceptionFactory)
            where TCollection : class, IEnumerable
        {
            if (parameter is null || parameter.Count() == 0)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified string is not null or empty, or otherwise throws an <see cref = "ArgumentNullException"/> or <see cref = "EmptyStringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "EmptyStringException">Thrown when <paramref name = "parameter"/> is an empty string.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotBeNullOrEmpty([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter is null)
            {
                Throw.ArgumentNull(parameterName, message);
            }

            if (parameter.Length == 0)
            {
                Throw.EmptyString(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified string is not null or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is an empty string or null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static string MustNotBeNullOrEmpty([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (parameter.IsNullOrEmpty())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the URI has one of the specified schemes, or otherwise throws an <see cref = "InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "schemes">One of these strings must be equal to the scheme of the URI.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidUriSchemeException">Thrown when the scheme <paramref name = "parameter"/> is not equal to one of the specified schemes.</exception>
        /// <exception cref = "RelativeUriException">Thrown when <paramref name = "parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "schemes"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; schemes:null => halt")]
        public static Uri MustHaveOneSchemeOf([NotNull][ValidatedNotNull] this Uri? parameter, IEnumerable<string> schemes, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustBeAbsoluteUri(parameterName, message);
            if (schemes is ICollection<string> collection)
            {
                if (!collection.Contains(parameter.Scheme))
                {
                    Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
                }

                return parameter;
            }

            if (!schemes.MustNotBeNull(nameof(schemes), message).Contains(parameter.Scheme))
            {
                Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            }

            return parameter;
        // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Ensures that the URI has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "schemes">One of these strings must be equal to the scheme of the URI.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "schemes"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the scheme <paramref name = "parameter"/> is not equal to one of the specified schemes, or when <paramref name = "parameter"/> is a relative URI, or when <paramref name = "parameter"/> is null.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "schemes"/> is null.</exception>
        /// <typeparam name = "TCollection">The type of the collection containing the schemes.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveOneSchemeOf<TCollection>([NotNull][ValidatedNotNull] this Uri? parameter, TCollection schemes, Func<Uri?, TCollection, Exception> exceptionFactory)
            where TCollection : class, IEnumerable<string>
        {
            if (parameter is null || !parameter.IsAbsoluteUri)
            {
                Throw.CustomException(exceptionFactory, parameter, schemes);
            }

            if (schemes is ICollection<string> collection)
            {
                if (!collection.Contains(parameter.Scheme))
                {
                    Throw.CustomException(exceptionFactory, parameter, schemes);
                }

                return parameter;
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (schemes is null || !schemes.Contains(parameter.Scheme))
            {
                Throw.CustomException(exceptionFactory, parameter, schemes!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not less than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is less than <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeGreaterThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) < 0)
            {
                Throw.MustBeGreaterThanOrEqualTo(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not less than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is less than <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeGreaterThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) < 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at most the specified number of items, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "count">The number of items the collection should have at most.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> does not contain at most the specified number of items.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMaximumCount<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) > count)
            {
                Throw.InvalidMaximumCollectionCount(parameter, count, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at most the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "count">The number of items the collection should have at most.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "count"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not contain at most the specified number of items, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMaximumCount<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, int count, Func<TCollection?, int, Exception> exceptionFactory)
            where TCollection : class, IEnumerable
        {
            if (parameter is null || parameter.Count() > count)
            {
                Throw.CustomException(exceptionFactory, parameter, count);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified character is a digit.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDigit(this char character) => char.IsDigit(character);
        /// <summary>
        /// Checks if the specified <paramref name = "condition"/> is true and throws an <see cref = "InvalidStateException"/> in this case.
        /// </summary>
        /// <param name = "condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name = "message">The message that will be passed to the <see cref = "InvalidStateException"/>.</param>
        /// <exception cref = "InvalidStateException">Thrown when <paramref name = "condition"/> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidState(bool condition, string? message = null)
        {
            if (condition)
            {
                Throw.InvalidState(message);
            }
        }

        /// <summary>
        /// Checks if the specified GUID is an empty one.
        /// </summary>
        /// <param name = "parameter">The GUID to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this Guid parameter) => parameter == Guid.Empty;
        /// <summary>
        /// Ensures that the string is a valid email address using the default email regular expression
        /// defined in <see cref = "RegularExpressions.EmailRegex"/>, or otherwise throws an <see cref = "InvalidEmailAddressException"/>.
        /// </summary>
        /// <param name = "parameter">The email address that will be validated.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidEmailAddressException">Thrown when <paramref name = "parameter"/> is no valid email address.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeEmailAddress([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsEmailAddress())
            {
                Throw.InvalidEmailAddress(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a valid email address using the default email regular expression
        /// defined in <see cref = "RegularExpressions.EmailRegex"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The email address that will be validated.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or no valid email address.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeEmailAddress([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (!parameter.IsEmailAddress())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a valid email address using the provided regular expression,
        /// or otherwise throws an <see cref = "InvalidEmailAddressException"/>.
        /// </summary>
        /// <param name = "parameter">The email address that will be validated.</param>
        /// <param name = "emailAddressPattern">The regular expression that determines if the input string is a valid email.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidEmailAddressException">Thrown when <paramref name = "parameter"/> is no valid email address.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; emailAddressPattern:null => halt")]
        public static string MustBeEmailAddress([NotNull][ValidatedNotNull] this string? parameter, Regex emailAddressPattern, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsEmailAddress(emailAddressPattern))
            {
                Throw.InvalidEmailAddress(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a valid email address using the provided regular expression,
        /// or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The email address that will be validated.</param>
        /// <param name = "emailAddressPattern">The regular expression that determines if the input string is a valid email.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "emailAddressPattern"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or no valid email address.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; emailAddressPattern:null => halt")]
        public static string MustBeEmailAddress([NotNull][ValidatedNotNull] this string? parameter, Regex emailAddressPattern, Func<string?, Regex, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (emailAddressPattern is null || !parameter.IsEmailAddress(emailAddressPattern))
            {
                Throw.CustomException(exceptionFactory, parameter, emailAddressPattern!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must not contain <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "value"/> contains <paramref name = "parameter"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (value.MustNotBeNull(nameof(value), message).Contains(parameter.MustNotBeNull(parameterName, message)))
            {
                Throw.Substring(parameter, value, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must not contain <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "value"/> contains <paramref name = "parameter"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || value.Contains(parameter))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must not contain <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "value"/> contains <paramref name = "parameter"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            value.MustNotBeNull(nameof(value), message);
            parameter.MustNotBeNull(parameterName, message);
            if (value.IndexOf(parameter, comparisonType) != -1)
            {
                Throw.Substring(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must not contain <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "value"/> contains <paramref name = "parameter"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustNotBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || value.IndexOf(parameter, comparisonType) != -1)
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is not
        /// less than 0.0001.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeApproximately(this double parameter, double other, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null) => parameter.MustBeApproximately(other, 0.0001, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is not
        /// less than 0.0001.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeApproximately(this double parameter, double other, Func<double, double, Exception> exceptionFactory)
        {
            if (!parameter.IsApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is not
        /// less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeApproximately(this double parameter, double other, double tolerance, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsApproximately(other, tolerance))
            {
                Throw.MustBeApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "other"/>, and <paramref name = "tolerance"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when the absolute difference between <paramref name = "parameter"/> and
        /// <paramref name = "other"/> is not less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeApproximately(this double parameter, double other, double tolerance, Func<double, double, double, Exception> exceptionFactory)
        {
            if (!parameter.IsApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001f, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// not less than 0.0001f.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeApproximately(this float parameter, float other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => parameter.MustBeApproximately(other, 0.0001f, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is not
        /// less than 0.0001.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeApproximately(this float parameter, float other, Func<float, float, Exception> exceptionFactory)
        {
            if (!parameter.IsApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is not
        /// less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeApproximately(this float parameter, float other, float tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsApproximately(other, tolerance))
            {
                Throw.MustBeApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "other"/>, and
        /// <paramref name = "tolerance"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when the absolute difference between <paramref name = "parameter"/> and
        /// <paramref name = "other"/> is not less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeApproximately(this float parameter, float other, float tolerance, Func<float, float, float, Exception> exceptionFactory)
        {
            if (!parameter.IsApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string ends with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must end with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref = "StringComparison.CurrentCulture"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> does not end with <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        public static string MustEndWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType = StringComparison.CurrentCulture, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).EndsWith(value, comparisonType))
            {
                Throw.StringDoesNotEndWith(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string ends with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must end with.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not end with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustEndWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || !parameter.EndsWith(value))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string ends with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must end with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not end with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustEndWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType, [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || !parameter.EndsWith(value, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the string contains the specified value using the given comparison type.
        /// </summary>
        /// <param name = "string">The string to be checked.</param>
        /// <param name = "value">The other string.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if <paramref name = "string"/> contains <paramref name = "value"/>, else false.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "string"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("string:null => halt; value:null => halt")]
        public static bool Contains(// ReSharper disable once RedundantNullableFlowAttribute -- Caller might have NRTs turned off
        [NotNull][ValidatedNotNull] this string @string, string value, StringComparison comparisonType) => @string.MustNotBeNull(nameof(@string)).IndexOf(value.MustNotBeNull(nameof(value)), comparisonType) >= 0;
        /// <summary>
        /// Ensures that <paramref name = "parameter"/> and <paramref name = "other"/> do not point to the same object instance, or otherwise
        /// throws a <see cref = "SameObjectReferenceException"/>.
        /// </summary>
        /// <param name = "parameter">The first reference to be checked.</param>
        /// <param name = "other">The second reference to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SameObjectReferenceException">Thrown when both <paramref name = "parameter"/> and <paramref name = "other"/> point to the same object.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotBeSameAs<T>([NoEnumeration] this T? parameter, [NoEnumeration] T? other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : class
        {
            if (ReferenceEquals(parameter, other))
            {
                Throw.SameObjectReference(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> and <paramref name = "other"/> do not point to the same object instance, or otherwise
        /// throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first reference to be checked.</param>
        /// <param name = "other">The second reference to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "SameObjectReferenceException">Thrown when both <paramref name = "parameter"/> and <paramref name = "other"/> point to the same object.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? MustNotBeSameAs<T>([NoEnumeration] this T? parameter, T? other, Func<T?, Exception> exceptionFactory)
            where T : class
        {
            if (ReferenceEquals(parameter, other))
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// less than 0.0001.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustNotBeApproximately(this double parameter, double other, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null) => parameter.MustNotBeApproximately(other, 0.0001, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// less than 0.0001.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustNotBeApproximately(this double parameter, double other, Func<double, double, Exception> exceptionFactory)
        {
            if (parameter.IsApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustNotBeApproximately(this double parameter, double other, double tolerance, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (parameter.IsApproximately(other, tolerance))
            {
                Throw.MustNotBeApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "other"/>, and <paramref name = "tolerance"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when the absolute difference between <paramref name = "parameter"/> and
        /// <paramref name = "other"/> is less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustNotBeApproximately(this double parameter, double other, double tolerance, Func<double, double, double, Exception> exceptionFactory)
        {
            if (parameter.IsApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001f, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// less than 0.0001f.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustNotBeApproximately(this float parameter, float other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => parameter.MustNotBeApproximately(other, 0.0001f, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// less than 0.0001.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustNotBeApproximately(this float parameter, float other, Func<float, float, Exception> exceptionFactory)
        {
            if (parameter.IsApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when the absolute difference between <paramref name = "parameter"/> and <paramref name = "other"/> is
        /// less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustNotBeApproximately(this float parameter, float other, float tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.IsApproximately(other, tolerance))
            {
                Throw.MustNotBeApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should not be approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "other"/>, and
        /// <paramref name = "tolerance"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when the absolute difference between <paramref name = "parameter"/> and
        /// <paramref name = "other"/> is less than <paramref name = "tolerance"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustNotBeApproximately(this float parameter, float other, float tolerance, Func<float, float, float, Exception> exceptionFactory)
        {
            if (parameter.IsApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified nullable has a value and returns it, or otherwise throws a <see cref = "NullableHasNoValueException"/>.
        /// </summary>
        /// <param name = "parameter">The nullable to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "NullableHasNoValueException">Thrown when <paramref name = "parameter"/> has no value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustHaveValue<T>([NotNull, NoEnumeration] this T? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : struct
        {
            if (!parameter.HasValue)
            {
                Throw.NullableHasNoValue(parameterName, message);
            }

            return parameter.Value;
        }

        /// <summary>
        /// Ensures that the specified nullable has a value and returns it, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The nullable to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "NullableHasNoValueException">Thrown when <paramref name = "parameter"/> has no value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static T MustHaveValue<T>([NotNull, NoEnumeration] this T? parameter, Func<Exception> exceptionFactory)
            where T : struct
        {
            if (!parameter.HasValue)
            {
                Throw.CustomException(exceptionFactory);
            }

            return parameter.Value;
        }

        /// <summary>
        /// Ensures that the string is longer than the specified length, or otherwise throws a <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be longer than.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringLengthException">Thrown when <paramref name = "parameter"/> has a length shorter than or equal to <paramref name = "length"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeLongerThan([NotNull][ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Length <= length)
            {
                Throw.StringNotLongerThan(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is longer than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be longer than.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or when it has a length shorter than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeLongerThan([NotNull][ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.Length <= length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The value that the span must be longer than.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is shorter than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThan<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<T>)parameter).MustBeLongerThan(length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be longer than.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is shorter than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThan<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length <= length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The value that the span must be longer than.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is shorter than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThan<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Length <= length)
            {
                Throw.SpanMustBeLongerThan(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be longer than.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is shorter than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThan<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length <= length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws an <see cref = "ArgumentNullException"/>.
        /// </summary>
        /// <param name = "parameter">The object reference to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNull<T>([NotNull, ValidatedNotNull, NoEnumeration] this T? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : class
        {
            if (parameter is null)
            {
                Throw.ArgumentNull(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified object reference is not null, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The reference to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeNull<T>([NotNull, ValidatedNotNull, NoEnumeration] this T? parameter, Func<Exception> exceptionFactory)
            where T : class
        {
            if (parameter is null)
            {
                Throw.CustomException(exceptionFactory);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified <paramref name = "condition"/> is true and throws an <see cref = "ArgumentException"/> in this case.
        /// </summary>
        /// <param name = "condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the <see cref = "ArgumentException"/> (optional).</param>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "condition"/> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidArgument(bool condition, string? parameterName = null, string? message = null)
        {
            if (condition)
            {
                Throw.Argument(parameterName, message);
            }
        }

        /// <summary>
        /// Checks if the specified <paramref name = "condition"/> is true and throws your custom exception in this case.
        /// </summary>
        /// <param name = "condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "condition"/> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static void InvalidArgument(bool condition, Func<Exception> exceptionFactory)
        {
            if (condition)
            {
                Throw.CustomException(exceptionFactory);
            }
        }

        /// <summary>
        /// Checks if the specified <paramref name = "condition"/> is true and throws your custom exception in this case.
        /// </summary>
        /// <param name = "condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name = "parameter">The value that is checked in the <paramref name = "condition"/>. This value is passed to the <paramref name = "exceptionFactory"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. The <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "condition"/> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static void InvalidArgument<T>(bool condition, T parameter, Func<T, Exception> exceptionFactory)
        {
            if (condition)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }
        }

        /// <summary>
        /// Checks if the specified character is a letter or digit.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLetterOrDigit(this char character) => char.IsLetterOrDigit(character);
        /// <summary>
        /// Checks if the given <paramref name = "type"/> is equal to the specified <paramref name = "otherType"/> or if it implements it. Internally, this
        /// method uses <see cref = "IsEquivalentTypeTo"/> so that constructed generic types and their corresponding generic type definitions are regarded as equal.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "otherType">The type that is equivalent to <paramref name = "type"/> or the interface type that <paramref name = "type"/> implements.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "otherType"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; otherType:null => halt")]
        public static bool IsOrImplements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type otherType) => type.IsEquivalentTypeTo(otherType.MustNotBeNull(nameof(otherType))) || type.Implements(otherType);
        /// <summary>
        /// Checks if the given <paramref name = "type"/> is equal to the specified <paramref name = "otherType"/> or if it implements it. This overload uses the specified <paramref name = "typeComparer"/>
        /// to compare the types.
        /// </summary>
        /// ,
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "otherType">The type that is equivalent to <paramref name = "type"/> or the interface type that <paramref name = "type"/> implements.</param>
        /// <param name = "typeComparer">The equality comparer used to compare the interface types.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "otherType"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; otherType:null => halt")]
        public static bool IsOrImplements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type otherType, [NotNull][ValidatedNotNull] IEqualityComparer<Type> typeComparer) => typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type.MustNotBeNull(nameof(type)), otherType.MustNotBeNull(nameof(otherType))) || type.Implements(otherType, typeComparer);
        /// <summary>
        /// Checks if the specified string is trimmed, i.e. it does not start or end with
        /// white space characters. Inputting an empty string will return true. When null is passed,
        /// you can control the return value with <paramref name = "regardNullAsTrimmed"/> which will
        /// return true by default.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "regardNullAsTrimmed">
        /// The value indicating whether true or false should be returned from this method when the
        /// <paramref name = "parameter"/> is null. The default value is true.
        /// </param>
        /// <returns>
        /// True if the <paramref name = "parameter"/> is trimmed, else false. An empty string will result in true.
        /// You can control the return value with <paramref name = "regardNullAsTrimmed"/> when the
        /// <paramref name = "parameter"/> is null.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrimmed(this string? parameter, bool regardNullAsTrimmed = true) => parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmed();
        /// <summary>
        /// Checks if the specified character span is trimmed, i.e. it does not start or end with
        /// white space characters. Inputting an empty span will return true.
        /// </summary>
        /// <param name = "parameter">The character span to be checked.</param>
        /// <returns>True if the <paramref name = "parameter"/> is trimmed, else false. An empty span will result in true.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrimmed(this ReadOnlySpan<char> parameter) => parameter.Length == 0 || !parameter[0].IsWhiteSpace() && !parameter[parameter.Length - 1].IsWhiteSpace();
        /// <summary>
        /// Ensures that the string is not null and trimmed, or otherwise throws a <see cref = "StringException"/>.
        /// Empty strings are regarded as trimmed.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringException">
        /// Thrown when <paramref name = "parameter"/> is not trimmed, i.e. they start or end with white space characters.
        /// Empty strings are regarded as trimmed.
        /// </exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeTrimmed([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsTrimmed())
            {
                Throw.NotTrimmed(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not null and trimmed, or otherwise throws your custom exception.
        /// Empty strings are regarded as trimmed.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or not trimmed. Empty strings are regarded as trimmed.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeTrimmed([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (parameter is null || !parameter.AsSpan().IsTrimmed())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not start with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that <paramref name = "parameter"/> must not start with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref = "StringComparison.CurrentCulture"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> starts with <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        public static string MustNotStartWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType = StringComparison.CurrentCulture, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).StartsWith(value, comparisonType))
            {
                Throw.StringStartsWith(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not start with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that <paramref name = "parameter"/> must not start with.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustNotStartWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || parameter.StartsWith(value))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not start with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that <paramref name = "parameter"/> must not start with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustNotStartWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType, [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || parameter.StartsWith(value, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span does not start with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "value">The other span that <paramref name = "parameter"/> must not start with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> starts with <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> MustNotStartWith(this ReadOnlySpan<char> parameter, ReadOnlySpan<char> value, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.StartsWith(value, comparisonType))
            {
                Throw.StringStartsWith(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span does not start with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "value">The other span that <paramref name = "parameter"/> must not start with.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustNotStartWith<T>(this ReadOnlySpan<T> parameter, ReadOnlySpan<T> value, ReadOnlySpansExceptionFactory<T> exceptionFactory)
            where T : IEquatable<T>
        {
            if (parameter.StartsWith(value))
            {
                Throw.CustomSpanException(exceptionFactory, parameter, value);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span does not start with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "value">The other span that <paramref name = "parameter"/> must not start with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">
        /// Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> MustNotStartWith(this ReadOnlySpan<char> parameter, ReadOnlySpan<char> value, StringComparison comparisonType, ReadOnlySpansExceptionFactory<char, StringComparison> exceptionFactory)
        {
            if (parameter.StartsWith(value, comparisonType))
            {
                Throw.CustomSpanException(exceptionFactory, parameter, value, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must contain <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "value"/> does not contain <paramref name = "parameter"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!value.MustNotBeNull(nameof(value), message).Contains(parameter.MustNotBeNull(parameterName, message)))
            {
                Throw.NotSubstring(parameter, value, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must contain <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "value"/> does not contain <paramref name = "parameter"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || !value.Contains(parameter))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must contain <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "value"/> does not contain <paramref name = "parameter"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            value.MustNotBeNull(nameof(value), message);
            parameter.MustNotBeNull(parameterName, message);
            if (value.IndexOf(parameter, comparisonType) == -1)
            {
                Throw.NotSubstring(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a substring of the specified other string, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string that must contain <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "value"/> does not contain <paramref name = "parameter"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustBeSubstringOf([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || value.IndexOf(parameter, comparisonType) == -1)
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws an <see cref = "InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidUriSchemeException">Thrown when <paramref name = "parameter"/> uses a different scheme than "http" or "https".</exception>
        /// <exception cref = "RelativeUriException">Thrown when <paramref name = "parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpOrHttpsUrl([NotNull][ValidatedNotNull] this Uri? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustBeAbsoluteUri(parameterName, message).Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
            {
                Throw.UriMustHaveOneSchemeOf(parameter, ["https", "http"], parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" or "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates the exception to be thrown. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> uses a different scheme than "http" or "https", or when <paramref name = "parameter"/> is a relative URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpOrHttpsUrl([NotNull][ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory)
        {
            if (parameter.MustBeAbsoluteUri(exceptionFactory).Scheme.Equals("https") == false && parameter.Scheme.Equals("http") == false)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is shorter than or equal to the specified length, or otherwise throws a <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be shorter than or equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringLengthException">Thrown when <paramref name = "parameter"/> has a length greater than <paramref name = "length"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeShorterThanOrEqualTo([NotNull][ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Length > length)
            {
                Throw.StringNotShorterThanOrEqualTo(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is shorter than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be shorter than or equal to.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or when it has a length greater than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeShorterThanOrEqualTo([NotNull][ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.Length > length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than or equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is longer than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThanOrEqualTo<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<T>)parameter).MustBeShorterThanOrEqualTo(length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than or equal to.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is longer than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThanOrEqualTo<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length > length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than or equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is longer than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Length > length)
            {
                Throw.SpanMustBeShorterThanOrEqualTo(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than or equal to.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is longer than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length > length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws an <see cref = "ArgumentNullException"/>, an <see cref = "EmptyStringException"/>, or a <see cref = "WhiteSpaceStringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "WhiteSpaceStringException">Thrown when <paramref name = "parameter"/> contains only white space.</exception>
        /// <exception cref = "EmptyStringException">Thrown when <paramref name = "parameter"/> is an empty string.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustNotBeNullOrWhiteSpace([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            parameter.MustNotBeNullOrEmpty(parameterName, message);
            foreach (var character in parameter)
            {
                if (!character.IsWhiteSpace())
                {
                    return parameter;
                }
            }

            Throw.WhiteSpaceString(parameter, parameterName, message);
            return null;
        }

        /// <summary>
        /// Ensures that the specified string is not null, empty, or contains only white space, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null, empty, or contains only white space.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory: null => halt")]
        public static string MustNotBeNullOrWhiteSpace([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (parameter.IsNullOrWhiteSpace())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the two specified types are equivalent. This is true when both types are equal or
        /// when one type is a constructed generic type and the other type is the corresponding generic type definition.
        /// </summary>
        /// <param name = "type">The first type to be checked.</param>
        /// <param name = "other">The other type to be checked.</param>
        /// <returns>
        /// True if both types are null, or if both are equal, or if one type
        /// is a constructed generic type and the other one is the corresponding generic type definition, else false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEquivalentTypeTo(this Type? type, Type? other) => ReferenceEquals(type, other) || (type is not null && other is not null && (type == other || (type.IsConstructedGenericType != other.IsConstructedGenericType && CheckTypeEquivalency(type, other))));
        private static bool CheckTypeEquivalency(Type type, Type other)
        {
            if (type.IsConstructedGenericType)
            {
                return type.GetGenericTypeDefinition() == other;
            }

            return other.GetGenericTypeDefinition() == type;
        }

        /// <summary>
        /// Checks if the value is within the specified range.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "range">The range where <paramref name = "parameter"/> must be in-between.</param>
        /// <returns>True if the parameter is within the specified range, else false.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsIn<T>([NotNull][ValidatedNotNull] this T parameter, Range<T> range)
            where T : IComparable<T> => range.IsValueWithinRange(parameter);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is less than or equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeLessThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) <= 0)
            {
                Throw.MustNotBeLessThanOrEqualTo(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is less than or equal to <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeLessThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) <= 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the given type derives from the specified base class or interface type. Internally, this method uses <see cref = "IsEquivalentTypeTo"/>
        /// so that constructed generic types and their corresponding generic type definitions are regarded as equal.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "baseClassOrInterfaceType">The type describing an interface or base class that <paramref name = "type"/> should derive from or implement.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "baseClassOrInterfaceType"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; baseClassOrInterfaceType:null => halt")]
        public static bool InheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type baseClassOrInterfaceType) => baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType)).IsInterface ? type.Implements(baseClassOrInterfaceType) : type.DerivesFrom(baseClassOrInterfaceType);
        /// <summary>
        /// Checks if the given type derives from the specified base class or interface type. This overload uses the specified <paramref name = "typeComparer"/>
        /// to compare the types.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "baseClassOrInterfaceType">The type describing an interface or base class that <paramref name = "type"/> should derive from or implement.</param>
        /// <param name = "typeComparer">The equality comparer used to compare the types.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/>, or <paramref name = "baseClassOrInterfaceType"/>, or <paramref name = "typeComparer"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; baseClassOrInterfaceType:null => halt; typeComparer:null => halt")]
        public static bool InheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type baseClassOrInterfaceType, [NotNull][ValidatedNotNull] IEqualityComparer<Type> typeComparer) => baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType)).IsInterface ? type.Implements(baseClassOrInterfaceType, typeComparer) : type.DerivesFrom(baseClassOrInterfaceType, typeComparer);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> uses <see cref = "DateTimeKind.Unspecified"/>, or otherwise throws an <see cref = "InvalidDateTimeException"/>.
        /// </summary>
        /// <param name = "parameter">The date time to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidDateTimeException">Thrown when <paramref name = "parameter"/> does not use <see cref = "DateTimeKind.Unspecified"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime MustBeUnspecified(this DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Kind != DateTimeKind.Unspecified)
            {
                Throw.MustBeUnspecifiedDateTime(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> uses <see cref = "DateTimeKind.Unspecified"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The date time to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not use <see cref = "DateTimeKind.Unspecified"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static DateTime MustBeUnspecified(this DateTime parameter, Func<DateTime, Exception> exceptionFactory)
        {
            if (parameter.Kind != DateTimeKind.Unspecified)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified string is null, empty, or contains only white space.
        /// </summary>
        /// <param name = "string">The string to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("=> false, string:notnull; => true, string:canbenull")]
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? @string) => string.IsNullOrWhiteSpace(@string);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> uses <see cref = "DateTimeKind.Local"/>, or otherwise throws an <see cref = "InvalidDateTimeException"/>.
        /// </summary>
        /// <param name = "parameter">The date time to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidDateTimeException">Thrown when <paramref name = "parameter"/> does not use <see cref = "DateTimeKind.Local"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime MustBeLocal(this DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Kind != DateTimeKind.Local)
            {
                Throw.MustBeLocalDateTime(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> uses <see cref = "DateTimeKind.Local"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The date time to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not use <see cref = "DateTimeKind.Local"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static DateTime MustBeLocal(this DateTime parameter, Func<DateTime, Exception> exceptionFactory)
        {
            if (parameter.Kind != DateTimeKind.Local)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws a <see cref = "ValueIsNotOneOfException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "items">The items that should contain the value.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValueIsNotOneOfException">Thrown when <paramref name = "parameter"/> is not equal to one of the specified <paramref name = "items"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem>(this TItem parameter, // ReSharper disable once RedundantNullableFlowAttribute - the attribute has an effect, see Issue72NotNullAttribute tests
        [NotNull][ValidatedNotNull] IEnumerable<TItem> items, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (!parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
            {
                Throw.ValueNotOneOf(parameter, items, parameterName, message);
            }

            return parameter;
        // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Ensures that the value is one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "items">The items that should contain the value.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "items"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not equal to one of the specified <paramref name = "items"/>, or when <paramref name = "items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustBeOneOf<TItem, TCollection>(this TItem parameter, [NotNull][ValidatedNotNull] TCollection items, Func<TItem, TCollection, Exception> exceptionFactory)
            where TCollection : class, IEnumerable<TItem>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (items is null || !parameter.IsOneOf(items))
            {
                Throw.CustomException(exceptionFactory, parameter, items!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is equal to <paramref name = "other"/> using the default equality comparer, or otherwise throws a <see cref = "ValuesNotEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesNotEqualException">Thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are not equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBe<T>(this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!EqualityComparer<T>.Default.Equals(parameter, other))
            {
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is equal to <paramref name = "other"/> using the default equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are not equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBe<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory)
        {
            if (!EqualityComparer<T>.Default.Equals(parameter, other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is equal to <paramref name = "other"/> using the specified equality comparer, or otherwise throws a <see cref = "ValuesNotEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesNotEqualException">Thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are not equal.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "equalityComparer"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
            {
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is equal to <paramref name = "other"/> using the specified equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "other"/>, and <paramref name = "equalityComparer"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are not equal, or when <paramref name = "equalityComparer"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (equalityComparer is null || !equalityComparer.Equals(parameter, other))
            {
                Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name = "comparisonType"/>, or otherwise throws a <see cref = "ValuesNotEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesNotEqualException">Thrown when <paramref name = "parameter"/> is not equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparison"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustBe(this string? parameter, string? other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!string.Equals(parameter, other, comparisonType))
            {
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name = "comparisonType"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparison"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustBe(this string? parameter, string? other, StringComparison comparisonType, Func<string?, string?, StringComparison, Exception> exceptionFactory)
        {
            if (!string.Equals(parameter, other, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, other, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name = "comparisonType"/>, or otherwise throws a <see cref = "ValuesNotEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesNotEqualException">Thrown when <paramref name = "parameter"/> is not equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparisonType"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustBe(this string? parameter, string? other, StringComparisonType comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.Equals(other, comparisonType))
            {
                Throw.ValuesNotEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are equal using the specified <paramref name = "comparisonType"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparisonType"/> enum.</exception>
        public static string? MustBe(this string? parameter, string? other, StringComparisonType comparisonType, Func<string?, string?, StringComparisonType, Exception> exceptionFactory)
        {
            if (!parameter.Equals(other, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, other, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified <paramref name = "condition"/> is true and throws an <see cref = "InvalidOperationException"/> in this case.
        /// </summary>
        /// <param name = "condition">The condition to be checked. The exception is thrown when it is true.</param>
        /// <param name = "message">The message that will be passed to the <see cref = "InvalidOperationException"/> (optional).</param>
        /// <exception cref = "InvalidOperationException">Thrown when <paramref name = "condition"/> is true.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidOperation(bool condition, string? message = null)
        {
            if (condition)
            {
                Throw.InvalidOperation(message);
            }
        }

        /// <summary>
        /// Ensures that the string matches the specified regular expression, or otherwise throws a <see cref = "StringDoesNotMatchException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "regex">The regular expression used for pattern matching.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringDoesNotMatchException">Thrown when <paramref name = "parameter"/> does not match the specified regular expression.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "regex"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; regex:null => halt")]
        public static string MustMatch([NotNull][ValidatedNotNull] this string? parameter, Regex regex, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!regex.MustNotBeNull(nameof(regex), message).IsMatch(parameter.MustNotBeNull(parameterName, message)))
            {
                Throw.StringDoesNotMatch(parameter, regex, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string matches the specified regular expression, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "regex">The regular expression used for pattern matching.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "regex"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not match the specified regular expression,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "regex"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string MustMatch([NotNull][ValidatedNotNull] this string? parameter, Regex regex, Func<string?, Regex, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || regex is null || !regex.IsMatch(parameter))
            {
                Throw.CustomException(exceptionFactory, parameter, regex!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> can be cast to <typeparamref name = "T"/> and returns the cast value, or otherwise throws a <see cref = "TypeCastException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be cast.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "TypeCastException">Thrown when <paramref name = "parameter"/> cannot be cast to <typeparamref name = "T"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeOfType<T>([NotNull, ValidatedNotNull, NoEnumeration] this object? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message)is T castValue)
                return castValue;
            Throw.InvalidTypeCast(parameter, typeof(T), parameterName, message);
            return default;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> can be cast to <typeparamref name = "T"/> and returns the cast value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be cast.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. The <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> cannot be cast to <typeparamref name = "T"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeOfType<T>([NotNull, ValidatedNotNull, NoEnumeration] this object? parameter, Func<object?, Exception> exceptionFactory)
        {
            if (parameter is T castValue)
                return castValue;
            Throw.CustomException(exceptionFactory, parameter);
            return default;
        }

        /// <summary>
        /// Checks if the specified value is a valid enum value of its type. This is true when the specified value
        /// is one of the constants defined in the enum, or a valid flags combination when the enum type is marked
        /// with the <see cref = "FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name = "T">The type of the enum.</typeparam>
        /// <param name = "parameter">The enum value to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidEnumValue<T>(this T parameter)
            where T : struct, Enum => EnumInfo<T>.IsValidEnumValue(parameter);
        /// <summary>
        /// Checks if <paramref name = "parameter"/> and <paramref name = "other"/> point to the same object.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable StringLiteralTypo
        [ContractAnnotation("parameter:notNull => true, other:notnull; parameter:notNull => false, other:canbenull; other:notnull => true, parameter:notnull; other:notnull => false, parameter:canbenull")]
        // ReSharper restore StringLiteralTypo
        public static bool IsSameAs<T>([NoEnumeration] this T? parameter, [NoEnumeration] T? other)
            where T : class => ReferenceEquals(parameter, other);
        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name = "value">The first value to be compared.</param>
        /// <param name = "other">The second value to be compared.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name = "value"/> and <paramref name = "other"/> are equal or if their absolute difference
        /// is smaller than the given <paramref name = "tolerance"/>, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this double value, double other, double tolerance) => Math.Abs(value - other) <= tolerance;
        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001.
        /// </summary>
        /// <param name = "value">The first value to be compared.</param>
        /// <param name = "other">The second value to be compared.</param>
        /// <returns>
        /// True if <paramref name = "value"/> and <paramref name = "other"/> are equal or if their absolute difference
        /// is smaller than 0.0001, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this double value, double other) => Math.Abs(value - other) <= 0.0001;
        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name = "value">The first value to be compared.</param>
        /// <param name = "other">The second value to be compared.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name = "value"/> and <paramref name = "other"/> are equal or if their absolute difference
        /// is smaller than the given <paramref name = "tolerance"/>, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this float value, float other, float tolerance) => Math.Abs(value - other) <= tolerance;
        /// <summary>
        /// Checks if the specified value is approximately the same as the other value, using the default tolerance of 0.0001f.
        /// </summary>
        /// <param name = "value">The first value to be compared.</param>
        /// <param name = "other">The second value to be compared.</param>
        /// <returns>
        /// True if <paramref name = "value"/> and <paramref name = "other"/> are equal or if their absolute difference
        /// is smaller than 0.0001f, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsApproximately(this float value, float other) => Math.Abs(value - other) <= 0.0001f;
        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is within the specified range, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <typeparam name = "T">The type of the parameter to be checked.</typeparam>
        /// <param name = "parameter">The parameter to be checked.</param>
        /// <param name = "range">The range where <paramref name = "parameter"/> must be in-between.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when <paramref name = "parameter"/> is not within <paramref name = "range"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeIn<T>([NotNull][ValidatedNotNull] this T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (!range.IsValueWithinRange(parameter.MustNotBeNullReference(parameterName, message)))
            {
                Throw.MustBeInRange(parameter, range, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is within the specified range, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The parameter to be checked.</param>
        /// <param name = "range">The range where <paramref name = "parameter"/> must be in-between.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "range"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not within <paramref name = "range"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeIn<T>([NotNull][ValidatedNotNull] this T parameter, Range<T> range, Func<T, Range<T>, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || !range.IsValueWithinRange(parameter))
            {
                Throw.CustomException(exceptionFactory, parameter!, range);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified string is trimmed at the start, i.e. it does not start with
        /// white space characters. Inputting an empty string will return true.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "regardNullAsTrimmed">
        /// The value indicating whether true or false should be returned from this method when the
        /// <paramref name = "parameter"/> is null. The default value is true.
        /// </param>
        /// <returns>
        /// True if the <paramref name = "parameter"/> is trimmed at the start, else false.
        /// An empty string will result in true.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrimmedAtStart(this string? parameter, bool regardNullAsTrimmed = true) => parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtStart();
        /// <summary>
        /// Checks if the specified character span is trimmed at the start, i.e. it does not start with
        /// white space characters. Inputting an empty span will return true.
        /// </summary>
        /// <param name = "parameter">The character span to be checked.</param>
        /// <returns>
        /// True if the <paramref name = "parameter"/> is trimmed at the start, else false.
        /// An empty span will result in true.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrimmedAtStart(this ReadOnlySpan<char> parameter) => parameter.Length == 0 || !parameter[0].IsWhiteSpace();
        /// <summary>
        /// Ensures that the <paramref name = "parameter"/> has the specified scheme, or otherwise throws an <see cref = "InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "scheme">The scheme that the URI should have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidUriSchemeException">Thrown when <paramref name = "parameter"/> uses a different scheme than the specified one.</exception>
        /// <exception cref = "RelativeUriException">Thrown when <paramref name = "parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme([NotNull][ValidatedNotNull] this Uri? parameter, string scheme, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(parameterName, message).Scheme, scheme) == false)
            {
                Throw.UriMustHaveScheme(parameter, scheme, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name = "parameter"/> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "scheme">The scheme that the URI should have.</param>
        /// <param name = "exceptionFactory">The delegate that creates the exception to be thrown. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> uses a different scheme than the specified one, or when <paramref name = "parameter"/> is a relative URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme([NotNull][ValidatedNotNull] this Uri? parameter, string scheme, Func<Uri?, Exception> exceptionFactory)
        {
            if (string.Equals(parameter.MustBeAbsoluteUri(exceptionFactory).Scheme, scheme) == false)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <paramref name = "parameter"/> has the specified scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "scheme">The scheme that the URI should have.</param>
        /// <param name = "exceptionFactory">The delegate that creates the exception to be thrown. <paramref name = "parameter"/> and <paramref name = "scheme"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> uses a different scheme than the specified one, or when <paramref name = "parameter"/> is a relative URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustHaveScheme([NotNull][ValidatedNotNull] this Uri? parameter, string scheme, Func<Uri?, string, Exception> exceptionFactory)
        {
            if (parameter is null || !parameter.IsAbsoluteUri || parameter.Scheme.Equals(scheme) == false)
            {
                Throw.CustomException(exceptionFactory, parameter, scheme);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified value is less than or approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is less than <paramref name = "other"/> or if their absolute difference
        /// is smaller than the given <paramref name = "tolerance"/>, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessThanOrApproximately(this double value, double other, double tolerance) => value < other || value.IsApproximately(other, tolerance);
        /// <summary>
        /// Checks if the specified value is less than or approximately the same as the other value, using the default tolerance of 0.0001.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is less than <paramref name = "other"/> or if their absolute difference
        /// is smaller than 0.0001, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessThanOrApproximately(this double value, double other) => value < other || value.IsApproximately(other);
        /// <summary>
        /// Checks if the specified value is less than or approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is less than <paramref name = "other"/> or if their absolute difference
        /// is smaller than the given <paramref name = "tolerance"/>, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessThanOrApproximately(this float value, float other, float tolerance) => value < other || value.IsApproximately(other, tolerance);
        /// <summary>
        /// Checks if the specified value is less than or approximately the same as the other value, using the default tolerance of 0.0001f.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is less than <paramref name = "other"/> or if their absolute difference
        /// is smaller than 0.0001f, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessThanOrApproximately(this float value, float other) => value < other || value.IsApproximately(other);
        /// <summary>
        /// Ensures that the specified <see cref = "ImmutableArray{T}"/> is not default or empty, or otherwise throws an <see cref = "Exceptions.EmptyCollectionException"/>.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "Exceptions.EmptyCollectionException">Thrown when <paramref name = "parameter"/> is default or empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustNotBeDefaultOrEmpty<T>(this ImmutableArray<T> parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.IsDefaultOrEmpty)
            {
                Throw.EmptyCollection(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <see cref = "ImmutableArray{T}"/> is not default or empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. The <see cref = "ImmutableArray{T}"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is default or empty.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static ImmutableArray<T> MustNotBeDefaultOrEmpty<T>(this ImmutableArray<T> parameter, Func<ImmutableArray<T>, Exception> exceptionFactory)
        {
            if (parameter.IsDefaultOrEmpty)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws an <see cref = "InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidUriSchemeException">Thrown when <paramref name = "parameter"/> uses a different scheme than "http".</exception>
        /// <exception cref = "RelativeUriException">Thrown when <paramref name = "parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpUrl([NotNull][ValidatedNotNull] this Uri? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => parameter.MustHaveScheme("http", parameterName, message);
        /// <summary>
        /// Ensures that the specified URI has the "http" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates the exception to be thrown. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> uses a different scheme than "http", or when <paramref name = "parameter"/> is a relative URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpUrl([NotNull][ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory) => parameter.MustHaveScheme("http", exceptionFactory);
        /// <summary>
        /// Ensures that the string starts with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must start with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref = "StringComparison.CurrentCulture"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt")]
        public static string MustStartWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType = StringComparison.CurrentCulture, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).StartsWith(value, comparisonType))
            {
                Throw.StringDoesNotStartWith(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string starts with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must start with.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustStartWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || !parameter.StartsWith(value))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string starts with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must start with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not start with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustStartWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType, [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || !comparisonType.IsValidEnumValue() || !parameter.StartsWith(value, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified value is greater than or approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is greater than <paramref name = "other"/> or if their absolute difference
        /// is smaller than the given <paramref name = "tolerance"/>, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterThanOrApproximately(this double value, double other, double tolerance) => value > other || value.IsApproximately(other, tolerance);
        /// <summary>
        /// Checks if the specified value is greater than or approximately the same as the other value, using the default tolerance of 0.0001.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is greater than <paramref name = "other"/> or if their absolute difference
        /// is smaller than 0.0001, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterThanOrApproximately(this double value, double other) => value > other || value.IsApproximately(other);
        /// <summary>
        /// Checks if the specified value is greater than or approximately the same as the other value, using the given tolerance.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is greater than <paramref name = "other"/> or if their absolute difference
        /// is smaller than the given <paramref name = "tolerance"/>, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterThanOrApproximately(this float value, float other, float tolerance) => value > other || value.IsApproximately(other, tolerance);
        /// <summary>
        /// Checks if the specified value is greater than or approximately the same as the other value, using the default tolerance of 0.0001f.
        /// </summary>
        /// <param name = "value">The first value to compare.</param>
        /// <param name = "other">The second value to compare.</param>
        /// <returns>
        /// True if <paramref name = "value"/> is greater than <paramref name = "other"/> or if their absolute difference
        /// is smaller than 0.0001, otherwise false.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterThanOrApproximately(this float value, float other) => value > other || value.IsApproximately(other);
        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is not within the specified range, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <typeparam name = "T">The type of the parameter to be checked.</typeparam>
        /// <param name = "parameter">The parameter to be checked.</param>
        /// <param name = "range">The range where <paramref name = "parameter"/> must not be in-between.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when <paramref name = "parameter"/> is within <paramref name = "range"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeIn<T>([NotNull][ValidatedNotNull] this T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (range.IsValueWithinRange(parameter.MustNotBeNullReference(parameterName, message)))
            {
                Throw.MustNotBeInRange(parameter, range, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is not within the specified range, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The parameter to be checked.</param>
        /// <param name = "range">The range where <paramref name = "parameter"/> must not be in-between.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "range"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is within <paramref name = "range"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeIn<T>([NotNull][ValidatedNotNull] this T parameter, Range<T> range, Func<T, Range<T>, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || range.IsValueWithinRange(parameter))
            {
                Throw.CustomException(exceptionFactory, parameter!, range);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is longer than or equal to the specified length, or otherwise throws a <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be longer than or equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringLengthException">Thrown when <paramref name = "parameter"/> has a length shorter than <paramref name = "length"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeLongerThanOrEqualTo([NotNull][ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Length < length)
            {
                Throw.StringNotLongerThanOrEqualTo(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is longer than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be longer than or equal to.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or when it has a length shorter than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeLongerThanOrEqualTo([NotNull][ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.Length < length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The value that the span must be longer than or equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is shorter than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThanOrEqualTo<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<T>)parameter).MustBeLongerThanOrEqualTo(length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The value that the span must be longer than or equal to.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is shorter than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeLongerThanOrEqualTo<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length < length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The value that the span must be longer than or equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is shorter than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (parameter.Length < length)
            {
                Throw.SpanMustBeLongerThanOrEqualTo(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is longer than or equal to the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The value that the span must be longer than or equal to.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is shorter than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeLongerThanOrEqualTo<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length < length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a valid file extension, or otherwise throws an <see cref = "StringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringException">Thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeFileExtension([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsFileExtension())
            {
                Throw.NotFileExtension(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is a valid file extension, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or not a valid file extension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeFileExtension([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (parameter is null || !parameter.IsFileExtension())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the character span is a valid file extension, or otherwise throws a <see cref = "StringException"/>.
        /// </summary>
        /// <param name = "parameter">The character span to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <returns>The original character span.</returns>
        /// <exception cref = "StringException">Thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<char> MustBeFileExtension(this Span<char> parameter, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<char>)parameter).MustBeFileExtension(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the character span is a valid file extension, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The character span to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <returns>The original character span.</returns>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        public static Span<char> MustBeFileExtension(this Span<char> parameter, ReadOnlySpanExceptionFactory<char> exceptionFactory)
        {
            ((ReadOnlySpan<char>)parameter).MustBeFileExtension(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the character memory is a valid file extension, or otherwise throws a <see cref = "StringException"/>.
        /// </summary>
        /// <param name = "parameter">The character memory to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <returns>The original character memory.</returns>
        /// <exception cref = "StringException">Thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<char> MustBeFileExtension(this Memory<char> parameter, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<char>)parameter.Span).MustBeFileExtension(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the character memory is a valid file extension, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The character memory to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <returns>The original character memory.</returns>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        public static Memory<char> MustBeFileExtension(this Memory<char> parameter, ReadOnlySpanExceptionFactory<char> exceptionFactory)
        {
            ((ReadOnlySpan<char>)parameter.Span).MustBeFileExtension(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the read-only character memory is a valid file extension, or otherwise throws a <see cref = "StringException"/>.
        /// </summary>
        /// <param name = "parameter">The read-only character memory to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <returns>The original read-only character memory.</returns>
        /// <exception cref = "StringException">Thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<char> MustBeFileExtension(this ReadOnlyMemory<char> parameter, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            parameter.Span.MustBeFileExtension(parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the read-only character memory is a valid file extension, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The read-only character memory to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <returns>The original read-only character memory.</returns>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        public static ReadOnlyMemory<char> MustBeFileExtension(this ReadOnlyMemory<char> parameter, ReadOnlySpanExceptionFactory<char> exceptionFactory)
        {
            parameter.Span.MustBeFileExtension(exceptionFactory);
            return parameter;
        }

        /// <summary>
        /// Ensures that the read-only character span is a valid file extension, or otherwise throws a <see cref = "StringException"/>.
        /// </summary>
        /// <param name = "parameter">The read-only character span to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <returns>The original read-only character span.</returns>
        /// <exception cref = "StringException">Thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> MustBeFileExtension(this ReadOnlySpan<char> parameter, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsFileExtension())
            {
                Throw.NotFileExtension(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the read-only character span is a valid file extension, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The read-only character span to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <returns>The original read-only character span.</returns>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not a valid file extension.</exception>
        public static ReadOnlySpan<char> MustBeFileExtension(this ReadOnlySpan<char> parameter, ReadOnlySpanExceptionFactory<char> exceptionFactory)
        {
            if (!parameter.IsFileExtension())
            {
                Throw.CustomSpanException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is not equal to <paramref name = "other"/> using the default equality comparer, or otherwise throws a <see cref = "ValuesEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesEqualException">Thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBe<T>(this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (EqualityComparer<T>.Default.Equals(parameter, other))
            {
                Throw.ValuesEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is not equal to <paramref name = "other"/> using the default equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are equal.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBe<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory)
        {
            if (EqualityComparer<T>.Default.Equals(parameter, other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is not equal to <paramref name = "other"/> using the specified equality comparer, or otherwise throws a <see cref = "ValuesEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesEqualException">Thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are equal.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "equalityComparer"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (equalityComparer.MustNotBeNull(nameof(equalityComparer), message).Equals(parameter, other))
            {
                Throw.ValuesEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name = "parameter"/> is not equal to <paramref name = "other"/> using the specified equality comparer, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first value to be compared.</param>
        /// <param name = "other">The other value to be compared.</param>
        /// <param name = "equalityComparer">The equality comparer used for comparing the two values.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "other"/>, and <paramref name = "equalityComparer"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> and <paramref name = "other"/> are equal, or when <paramref name = "equalityComparer"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("equalityComparer:null => halt")]
        public static T MustNotBe<T>(this T parameter, T other, IEqualityComparer<T> equalityComparer, Func<T, T, IEqualityComparer<T>, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (equalityComparer is null || equalityComparer.Equals(parameter, other))
            {
                Throw.CustomException(exceptionFactory, parameter, other, equalityComparer!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name = "comparisonType"/>, or otherwise throws a <see cref = "ValuesEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesEqualException">Thrown when <paramref name = "parameter"/> is equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparison"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustNotBe(this string? parameter, string? other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (string.Equals(parameter, other, comparisonType))
            {
                Throw.ValuesEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name = "comparisonType"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "other"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparison"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustNotBe(this string? parameter, string? other, StringComparison comparisonType, Func<string?, string?, Exception> exceptionFactory)
        {
            if (string.Equals(parameter, other, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name = "comparisonType"/>, or otherwise throws a <see cref = "ValuesEqualException"/>.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValuesEqualException">Thrown when <paramref name = "parameter"/> is equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparisonType"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustNotBe(this string? parameter, string? other, StringComparisonType comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Equals(other, comparisonType))
            {
                Throw.ValuesEqual(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the two strings are not equal using the specified <paramref name = "comparisonType"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The first string to be compared.</param>
        /// <param name = "other">The second string to be compared.</param>
        /// <param name = "comparisonType">The enum value specifying how the two strings should be compared.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "other"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid value from the <see cref = "StringComparisonType"/> enum.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? MustNotBe(this string? parameter, string? other, StringComparisonType comparisonType, Func<string?, string?, StringComparisonType, Exception> exceptionFactory)
        {
            if (parameter.Equals(other, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, other, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "count">The number of items the collection should have at least.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> does not contain at least the specified number of items.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where TCollection : class, IEnumerable
        {
            if (parameter.Count(parameterName, message) < count)
            {
                Throw.InvalidMinimumCollectionCount(parameter, count, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection has at least the specified number of items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "count">The number of items the collection should have at least.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "count"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not contain at least the specified number of items, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustHaveMinimumCount<TCollection>([NotNull][ValidatedNotNull] this TCollection? parameter, int count, Func<TCollection?, int, Exception> exceptionFactory)
            where TCollection : class, IEnumerable
        {
            if (parameter is null || parameter.Count() < count)
            {
                Throw.CustomException(exceptionFactory, parameter, count);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not greater than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is greater than <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeLessThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) > 0)
            {
                Throw.MustBeLessThanOrEqualTo(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not greater than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is greater than <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeLessThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) > 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is not less than <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeLessThan<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) >= 0)
            {
                Throw.MustBeLessThan(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is not less than <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeLessThan<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) >= 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws a <see cref = "ValueIsOneOfException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "items">The items that must not contain the value.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ValueIsOneOfException">Thrown when <paramref name = "parameter"/> is equal to one of the specified <paramref name = "items"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        // ReSharper disable once RedundantNullableFlowAttribute - the attribute has an effect, see Issue72NotNullAttribute tests
        public static TItem MustNotBeOneOf<TItem>(this TItem parameter, [NotNull][ValidatedNotNull] IEnumerable<TItem> items, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (parameter.IsOneOf(items.MustNotBeNull(nameof(items), message)))
            {
                Throw.ValueIsOneOf(parameter, items, parameterName, message);
            }

            return parameter;
        // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Ensures that the value is not one of the specified items, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "items">The items that must not contain the value.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "items"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is equal to one of the specified <paramref name = "items"/>, or when <paramref name = "items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("items:null => halt")]
        public static TItem MustNotBeOneOf<TItem, TCollection>(this TItem parameter, [NotNull][ValidatedNotNull] TCollection items, Func<TItem, TCollection, Exception> exceptionFactory)
            where TCollection : class, IEnumerable<TItem>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (items is null || parameter.IsOneOf(items))
            {
                Throw.CustomException(exceptionFactory, parameter, items!);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified span is empty or contains only white space characters.
        /// </summary>
        /// <param name = "span">The span to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmptyOrWhiteSpace(this Span<char> span) => ((ReadOnlySpan<char>)span).IsEmptyOrWhiteSpace();
        /// <summary>
        /// Checks if the specified span is empty or contains only white space characters.
        /// </summary>
        /// <param name = "span">The span to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> span)
        {
            if (span.IsEmpty)
            {
                return true;
            }

            foreach (var character in span)
            {
                if (!character.IsWhiteSpace())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the specified memory is empty or contains only white space characters.
        /// </summary>
        /// <param name = "memory">The memory to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmptyOrWhiteSpace(this Memory<char> memory) => memory.Span.IsEmptyOrWhiteSpace();
        /// <summary>
        /// Checks if the specified memory is empty or contains only white space characters.
        /// </summary>
        /// <param name = "memory">The memory to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmptyOrWhiteSpace(this ReadOnlyMemory<char> memory) => memory.Span.IsEmptyOrWhiteSpace();
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is not less than <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeGreaterThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) >= 0)
            {
                Throw.MustNotBeGreaterThanOrEqualTo(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is not less than <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeGreaterThanOrEqualTo<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) >= 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws an <see cref = "EnumValueNotDefinedException"/>. An enum value
        /// is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref = "FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name = "T">The type of the enum.</typeparam>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "EnumValueNotDefinedException">Thrown when <paramref name = "parameter"/> is no valid enum value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustBeValidEnumValue<T>(this T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : struct, Enum
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
            {
                Throw.EnumValueNotDefined(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified enum value is valid, or otherwise throws your custom exception. An enum value
        /// is valid when the specified value is one of the constants defined in the enum, or a valid flags combination when the enum type
        /// is marked with the <see cref = "FlagsAttribute"/>.
        /// </summary>
        /// <typeparam name = "T">The type of the enum.</typeparam>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. The <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is no valid enum value, or when <typeparamref name = "T"/> is no enum type.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static T MustBeValidEnumValue<T>(this T parameter, Func<T, Exception> exceptionFactory)
            where T : struct, Enum
        {
            if (!EnumInfo<T>.IsValidEnumValue(parameter))
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified GUID is not empty, or otherwise throws an <see cref = "EmptyGuidException"/>.
        /// </summary>
        /// <param name = "parameter">The GUID to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "EmptyGuidException">Thrown when <paramref name = "parameter"/> is an empty GUID.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid MustNotBeEmpty(this Guid parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter == Guid.Empty)
            {
                Throw.EmptyGuid(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified GUID is not empty, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The GUID to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is an empty GUID.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static Guid MustNotBeEmpty(this Guid parameter, Func<Exception> exceptionFactory)
        {
            if (parameter == Guid.Empty)
            {
                Throw.CustomException(exceptionFactory);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified strings are equal, using the given comparison rules.
        /// </summary>
        /// <param name = "string">The first string to compare.</param>
        /// <param name = "value">The second string to compare.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
        /// <returns>True if the two strings are considered equal, else false.</returns>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is no valid enum value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(this string? @string, string? value, StringComparisonType comparisonType)
        {
            if ((int)comparisonType < 6)
            {
                return string.Equals(@string, value, (StringComparison)comparisonType);
            }

            switch (comparisonType)
            {
                case StringComparisonType.OrdinalIgnoreWhiteSpace:
                    return @string.EqualsOrdinalIgnoreWhiteSpace(value);
                case StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace:
                    return @string.EqualsOrdinalIgnoreCaseIgnoreWhiteSpace(value);
                default:
                    Throw.EnumValueNotDefined(comparisonType, nameof(comparisonType));
                    return false;
            }
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not greater than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is greater than <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeGreaterThan<T>([NotNull, ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) > 0)
                Throw.MustNotBeGreaterThan(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not greater than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be greater than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is greater than <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeGreaterThan<T>([NotNull, ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) > 0)
                Throw.CustomException(exceptionFactory, parameter!, other);
            return parameter;
        }

        /// <summary>
        /// Checks if the given <paramref name = "type"/> is equal to the specified <paramref name = "otherType"/> or if it derives from it or implements it.
        /// Internally, this method uses <see cref = "IsEquivalentTypeTo"/> so that constructed generic types and their corresponding generic type definitions
        /// are regarded as equal.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "otherType">The type that is equivalent to <paramref name = "type"/> or the base class type where <paramref name = "type"/> derives from.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "otherType"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; otherType:null => halt")]
        public static bool IsOrInheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type otherType) => type.IsEquivalentTypeTo(otherType.MustNotBeNull(nameof(otherType))) || type.InheritsFrom(otherType);
        /// <summary>
        /// Checks if the given <paramref name = "type"/> is equal to the specified <paramref name = "otherType"/> or if it derives from it or implements it.
        /// This overload uses the specified <paramref name = "typeComparer"/> to compare the types.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "otherType">The type that is equivalent to <paramref name = "type"/> or the base class type where <paramref name = "type"/> derives from.</param>
        /// <param name = "typeComparer">The equality comparer used to compare the types.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "otherType"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; otherType:null => halt; typeComparer:null => halt")]
        public static bool IsOrInheritsFrom(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type otherType, [NotNull][ValidatedNotNull] IEqualityComparer<Type> typeComparer) => typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type, otherType.MustNotBeNull(nameof(otherType))) || type.InheritsFrom(otherType, typeComparer);
        /// <summary>
        /// Checks if the given <paramref name = "type"/> is equal to the specified <paramref name = "otherType"/> or if it derives from it. Internally, this
        /// method uses <see cref = "IsEquivalentTypeTo"/> so that constructed generic types and their corresponding generic type definitions are regarded as equal.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "otherType">The type that is equivalent to <paramref name = "type"/> or the base class type where <paramref name = "type"/> derives from.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "otherType"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; otherType:null => halt")]
        public static bool IsOrDerivesFrom([NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type otherType) => type.IsEquivalentTypeTo(otherType.MustNotBeNull(nameof(otherType))) || type.DerivesFrom(otherType);
        /// <summary>
        /// Checks if the given <paramref name = "type"/> is equal to the specified <paramref name = "otherType"/> or if it derives from it. This overload uses the specified <paramref name = "typeComparer"/>
        /// to compare the types.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "otherType">The type that is equivalent to <paramref name = "type"/> or the base class type where <paramref name = "type"/> derives from.</param>
        /// <param name = "typeComparer">The equality comparer used to compare the types.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/>, or <paramref name = "otherType"/>, or <paramref name = "typeComparer"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt; otherType:null => halt; typeComparer:null => halt")]
        public static bool IsOrDerivesFrom([NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type otherType, [NotNull][ValidatedNotNull] IEqualityComparer<Type> typeComparer) => typeComparer.MustNotBeNull(nameof(typeComparer)).Equals(type, otherType.MustNotBeNull(nameof(otherType))) || type.DerivesFrom(otherType, typeComparer);
        /// <summary>
        /// Ensures that the string is not null and trimmed at the start, or otherwise throws a <see cref = "StringException"/>.
        /// Empty strings are regarded as trimmed.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringException">
        /// Thrown when <paramref name = "parameter"/> is not trimmed at the start, i.e. they start with white space characters.
        /// Empty strings are regarded as trimmed.
        /// </exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeTrimmedAtStart([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtStart())
            {
                Throw.NotTrimmedAtStart(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not null and trimmed at the start, or otherwise throws your custom exception.
        /// Empty strings are regarded as trimmed.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or not trimmed at the start. Empty strings are regarded as trimmed.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeTrimmedAtStart([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (parameter is null || !parameter.AsSpan().IsTrimmedAtStart())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the type implements the specified interface type. Internally, this method uses <see cref = "IsEquivalentTypeTo"/>
        /// so that constructed generic types and their corresponding generic type definitions are regarded as equal.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "interfaceType">The interface type that <paramref name = "type"/> should implement.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "interfaceType"/> is null.</exception>
        [ContractAnnotation("type:null => halt; interfaceType:null => halt")]
        public static bool Implements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type interfaceType)
        {
            type.MustNotBeNull();
            interfaceType.MustNotBeNull();
            var implementedInterfaces = type.GetInterfaces();
            for (var i = 0; i < implementedInterfaces.Length; ++i)
            {
                if (interfaceType.IsEquivalentTypeTo(implementedInterfaces[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the type implements the specified interface type. This overload uses the specified <paramref name = "typeComparer"/>
        /// to compare the interface types.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <param name = "interfaceType">The interface type that <paramref name = "type"/> should implement.</param>
        /// <param name = "typeComparer">The equality comparer used to compare the interface types.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/>, or <paramref name = "interfaceType"/>, or <paramref name = "typeComparer"/> is null.</exception>
        [ContractAnnotation("type:null => halt; interfaceType:null => halt; typeComparer:null => halt")]
        public static bool Implements(
#if NET8_0
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
#endif
        [NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type interfaceType, [NotNull][ValidatedNotNull] IEqualityComparer<Type> typeComparer)
        {
            type.MustNotBeNull();
            interfaceType.MustNotBeNull();
            typeComparer.MustNotBeNull();
            var implementedInterfaces = type.GetInterfaces();
            for (var i = 0; i < implementedInterfaces.Length; ++i)
            {
                if (typeComparer.Equals(implementedInterfaces[i], interfaceType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Ensures that the specified URI is a relative one, or otherwise throws an <see cref = "AbsoluteUriException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "AbsoluteUriException">Thrown when <paramref name = "parameter"/> is an absolute URI.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeRelativeUri([NotNull][ValidatedNotNull] this Uri? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IsAbsoluteUri)
            {
                Throw.MustBeRelativeUri(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified URI is a relative one, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is an absolute URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeRelativeUri([NotNull][ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.IsAbsoluteUri)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> uses <see cref = "DateTimeKind.Utc"/>, or otherwise throws an <see cref = "InvalidDateTimeException"/>.
        /// </summary>
        /// <param name = "parameter">The date time to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidDateTimeException">Thrown when <paramref name = "parameter"/> does not use <see cref = "DateTimeKind.Utc"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime MustBeUtc(this DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Kind != DateTimeKind.Utc)
            {
                Throw.MustBeUtcDateTime(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> uses <see cref = "DateTimeKind.Utc"/>, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The date time to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not use <see cref = "DateTimeKind.Utc"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static DateTime MustBeUtc(this DateTime parameter, Func<DateTime, Exception> exceptionFactory)
        {
            if (parameter.Kind != DateTimeKind.Utc)
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified character is a white space character.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(this char character) => char.IsWhiteSpace(character);
        /// <summary>
        /// Ensures that the string is either "\n" or "\r\n", or otherwise throws a <see cref = "StringException"/>. This is done independently of the current value of <see cref = "Environment.NewLine"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringException">Thrown when <paramref name = "parameter"/> is not equal to "\n" or "\r\n".</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeNewLine([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsNewLine())
            {
                Throw.NotNewLine(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is either "\n" or "\r\n", or otherwise throws your custom exception. This is done independently of the current value of <see cref = "Environment.NewLine"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is not equal to "\n" or "\r\n".</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeNewLine([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (!parameter.IsNewLine())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified string represents a valid file extension.
        /// </summary>
        /// <param name = "value">
        /// The string to be checked. It must start with a period (.) and can only contain letters, digits,
        /// and additional periods.
        /// </param>
        /// <returns>True if the string is a valid file extension, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFileExtension([NotNullWhen(true)] this string? value) => value != null && IsFileExtension(value.AsSpan());
        /// <summary>
        /// Checks if the specified character span represents a valid file extension.
        /// </summary>
        /// <param name = "value">
        /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
        /// and additional periods.
        /// </param>
        /// <returns>True if the span is a valid file extension, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFileExtension(this Span<char> value) => IsFileExtension((ReadOnlySpan<char>)value);
        /// <summary>
        /// Checks if the specified character memory represents a valid file extension.
        /// </summary>
        /// <param name = "value">
        /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
        /// and additional periods.
        /// </param>
        /// <returns>True if the span is a valid file extension, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFileExtension(this ReadOnlyMemory<char> value) => IsFileExtension(value.Span);
        /// <summary>
        /// Checks if the specified character memory represents a valid file extension.
        /// </summary>
        /// <param name = "value">
        /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
        /// and additional periods.
        /// </param>
        /// <returns>True if the span is a valid file extension, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFileExtension(this Memory<char> value) => IsFileExtension(value.Span);
        /// <summary>
        /// Checks if the specified character span represents a valid file extension.
        /// </summary>
        /// <param name = "value">
        /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
        /// and additional periods.
        /// </param>
        /// <returns>True if the span is a valid file extension, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFileExtension(this ReadOnlySpan<char> value)
        {
            // ReSharper disable once UseIndexFromEndExpression -- cannot use index from end expression in .NET Standard 2.0
            if (value.Length <= 1 || value[0] != '.' || value[value.Length - 1] == '.')
            {
                return false;
            }

            var hasAlphanumeric = false;
            for (var i = 1; i < value.Length; i++)
            {
                var character = value[i];
                if (character.IsLetterOrDigit())
                {
                    hasAlphanumeric = true;
                }
                else if (character != '.')
                {
                    return false;
                }
            }

            return hasAlphanumeric;
        }

        /// <summary>
        /// Checks if the string is either "\n" or "\r\n". This is done independently of the current value of <see cref = "Environment.NewLine"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("=> false, parameter:canbenull; => true, parameter:notnull")]
        public static bool IsNewLine([NotNullWhen(true)] this string? parameter) => parameter == "\n" || parameter == "\r\n";
        /// <summary>
        /// Checks if the given <paramref name = "type"/> is a generic type that has open generic parameters,
        /// but is no generic type definition.
        /// </summary>
        /// <param name = "type">The type to be checked.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("type:null => halt")]
        // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static bool IsOpenConstructedGenericType([NotNull][ValidatedNotNull] this Type type) => type.MustNotBeNull(nameof(type)).IsGenericType && type.ContainsGenericParameters && type.IsGenericTypeDefinition == false;
        /// <summary>
        /// Checks if the specified string is an email address using the default email regular expression
        /// defined in <see cref = "RegularExpressions.EmailRegex"/>.
        /// </summary>
        /// <param name = "emailAddress">The string to be checked if it is an email address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("emailAddress:null => false")]
        public static bool IsEmailAddress([NotNullWhen(true)] this string? emailAddress) => emailAddress != null && RegularExpressions.EmailRegex.IsMatch(emailAddress);
        /// <summary>
        /// Checks if the specified string is an email address using the provided regular expression for validation.
        /// </summary>
        /// <param name = "emailAddress">The string to be checked.</param>
        /// <param name = "emailAddressPattern">The regular expression that determines whether the input string is an email address.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "emailAddressPattern"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("emailAddress:null => false; emailAddressPattern:null => halt")]
        public static bool IsEmailAddress([NotNullWhen(true)] this string? emailAddress, Regex emailAddressPattern) => emailAddress != null && emailAddressPattern.MustNotBeNull(nameof(emailAddressPattern)).IsMatch(emailAddress);
        /// <summary>
        /// Checks if the specified character is a letter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLetter(this char character) => char.IsLetter(character);
        /// <summary>
        /// Checks if the string is a substring of the other string.
        /// </summary>
        /// <param name = "value">The string to be checked.</param>
        /// <param name = "other">The other string.</param>
        /// <returns>True if <paramref name = "value"/> is a substring of <paramref name = "other"/>, else false.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "value"/> or <paramref name = "other"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("value:null => halt; other:null => halt")]
        // ReSharper disable RedundantNullableFlowAttribute
        public static bool IsSubstringOf([NotNull][ValidatedNotNull] this string value, [NotNull][ValidatedNotNull] string other) => other.MustNotBeNull(nameof(other)).Contains(value);
        // ReSharper restore RedundantNullableFlowAttribute
        /// <summary>
        /// Checks if the string is a substring of the other string.
        /// </summary>
        /// <param name = "value">The string to be checked.</param>
        /// <param name = "other">The other string.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>True if <paramref name = "value"/> is a substring of <paramref name = "other"/>, else false.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "value"/> or <paramref name = "other"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("value:null => halt; other:null => halt")]
        // ReSharper disable RedundantNullableFlowAttribute
        public static bool IsSubstringOf([NotNull][ValidatedNotNull] this string value, [NotNull][ValidatedNotNull] string other, StringComparison comparisonType) => other.MustNotBeNull(nameof(other)).IndexOf(value, comparisonType) != -1;
        /// <summary>
        /// Ensures that the string's length is within the specified range, or otherwise throws a <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "range">The range where the string's length must be in-between.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringLengthException">Thrown when the length of <paramref name = "parameter"/> is not with the specified <paramref name = "range"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustHaveLengthIn([NotNull][ValidatedNotNull] this string? parameter, Range<int> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!range.IsValueWithinRange(parameter.MustNotBeNull(parameterName, message).Length))
            {
                Throw.StringLengthNotInRange(parameter, range, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string's length is within the specified range, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "range">The range where the string's length must be in-between.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "range"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or its length is not within the specified range.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustHaveLengthIn([NotNull][ValidatedNotNull] this string? parameter, Range<int> range, Func<string?, Range<int>, Exception> exceptionFactory)
        {
            if (parameter is null || !range.IsValueWithinRange(parameter.Length))
            {
                Throw.CustomException(exceptionFactory, parameter, range);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/>'s length is within the specified range, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "range">The range where the <see cref = "ImmutableArray{T}"/>'s length must be in-between.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the length of <paramref name = "parameter"/> is not within the specified <paramref name = "range"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveLengthIn<T>(this ImmutableArray<T> parameter, Range<int> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            var length = parameter.IsDefault ? 0 : parameter.Length;
            if (!range.IsValueWithinRange(length))
            {
                Throw.ImmutableArrayLengthNotInRange(parameter, range, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/>'s length is within the specified range, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "range">The range where the <see cref = "ImmutableArray{T}"/>'s length must be in-between.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "range"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the length of <paramref name = "parameter"/> is not within the specified range.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("exceptionFactory:null => halt")]
        public static ImmutableArray<T> MustHaveLengthIn<T>(this ImmutableArray<T> parameter, Range<int> range, Func<ImmutableArray<T>, Range<int>, Exception> exceptionFactory)
        {
            var length = parameter.IsDefault ? 0 : parameter.Length;
            if (!range.IsValueWithinRange(length))
            {
                Throw.CustomException(exceptionFactory, parameter, range);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not the default value, or otherwise throws an <see cref = "ArgumentNullException"/>
        /// for reference types, or an <see cref = "ArgumentDefaultException"/> for value types.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is a reference type and null.</exception>
        /// <exception cref = "ArgumentDefaultException">Thrown when <paramref name = "parameter"/> is a value type and the default value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeDefault<T>([NotNull, ValidatedNotNull] this T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (default(T)is null)
            {
                if (parameter is null)
                {
                    Throw.ArgumentNull(parameterName, message);
                }

                return parameter;
            }

            if (EqualityComparer<T>.Default.Equals(parameter, default !))
            {
                Throw.ArgumentDefault(parameterName, message);
            }

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.

            return parameter;
#pragma warning restore CS8777
        }

        /// <summary>
        /// Ensures that the specified parameter is not the default value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is the default value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeDefault<T>([NotNull, ValidatedNotNull] this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T)is null)
            {
                if (parameter is null)
                {
                    Throw.CustomException(exceptionFactory);
                }

                return parameter;
            }

            if (EqualityComparer<T>.Default.Equals(parameter, default !))
            {
                Throw.CustomException(exceptionFactory);
            }

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.

            return parameter;
#pragma warning restore CS8777
        }

        /// <summary>
        /// Ensures that the string is shorter than the specified length, or otherwise throws a <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be shorter than.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringLengthException">Thrown when <paramref name = "parameter"/> has a length greater than or equal to <paramref name = "length"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeShorterThan([NotNull][ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Length >= length)
            {
                Throw.StringNotShorterThan(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is shorter than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The length that the string must be shorter than.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or when it has a length greater than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeShorterThan([NotNull][ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.Length >= length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is longer than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThan<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<T>)parameter).MustBeShorterThan(length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is longer than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustBeShorterThan<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length >= length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> is longer than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThan<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Length >= length)
            {
                Throw.SpanMustBeShorterThan(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span is shorter than the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length value that the span must be shorter than.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to it.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is longer than or equal to <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustBeShorterThan<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length >= length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/> has at most the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "length">The maximum length the <see cref = "ImmutableArray{T}"/> should have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> has more than the specified length.</exception>
        /// <remarks>The default instance of <see cref = "ImmutableArray{T}"/> will be treated as having length 0.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveMaximumLength<T>(this ImmutableArray<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            var parameterLength = parameter.IsDefault ? 0 : parameter.Length;
            if (parameterLength > length)
            {
                Throw.InvalidMaximumImmutableArrayLength(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/> has at most the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "length">The maximum length the <see cref = "ImmutableArray{T}"/> should have.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> has more than the specified length.</exception>
        /// <remarks>The default instance of <see cref = "ImmutableArray{T}"/> will be treated as having length 0.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveMaximumLength<T>(this ImmutableArray<T> parameter, int length, Func<ImmutableArray<T>, int, Exception> exceptionFactory)
        {
            var parameterLength = parameter.IsDefault ? 0 : parameter.Length;
            if (parameterLength > length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified collection is null or empty.
        /// </summary>
        /// <param name = "collection">The collection to be checked.</param>
        /// <returns>True if the collection is null or empty, else false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("=> true, collection:canbenull; => false, collection:notnull")]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IEnumerable? collection) => collection is null || collection.Count() == 0;
        /// <summary>
        /// Checks if the specified string is null or empty.
        /// </summary>
        /// <param name = "string">The string to be checked.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("=> false, string:notnull; => true, string:canbenull")]
        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? @string) => string.IsNullOrEmpty(@string);
        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws an <see cref = "InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidUriSchemeException">Thrown when <paramref name = "parameter"/> uses a different scheme than "https".</exception>
        /// <exception cref = "RelativeUriException">Thrown when <paramref name = "parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpsUrl([NotNull][ValidatedNotNull] this Uri? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => parameter.MustHaveScheme("https", parameterName, message);
        /// <summary>
        /// Ensures that the specified URI has the "https" scheme, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The URI to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates the exception to be thrown. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> uses a different scheme than "https", or when <paramref name = "parameter"/> is a relative URI, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static Uri MustBeHttpsUrl([NotNull][ValidatedNotNull] this Uri? parameter, Func<Uri?, Exception> exceptionFactory) => parameter.MustHaveScheme("https", exceptionFactory);
        /// <summary>
        /// Ensures that the string is not null and trimmed at the end, or otherwise throws a <see cref = "StringException"/>.
        /// Empty strings are regarded as trimmed.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringException">
        /// Thrown when <paramref name = "parameter"/> is not trimmed at the end, i.e. they end with white space characters.
        /// Empty strings are regarded as trimmed.
        /// </exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeTrimmedAtEnd([NotNull][ValidatedNotNull] this string? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).IsTrimmedAtEnd())
            {
                Throw.NotTrimmedAtEnd(parameter, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string is not null and trimmed at the end, or otherwise throws your custom exception.
        /// Empty strings are regarded as trimmed.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> is passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or not trimmed at the end. Empty strings are regarded as trimmed.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustBeTrimmedAtEnd([NotNull][ValidatedNotNull] this string? parameter, Func<string?, Exception> exceptionFactory)
        {
            if (parameter is null || !parameter.AsSpan().IsTrimmedAtEnd())
            {
                Throw.CustomException(exceptionFactory, parameter);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/> has at least the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "length">The minimum length the <see cref = "ImmutableArray{T}"/> should have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> has less than the specified length.</exception>
        /// <remarks>The default instance of <see cref = "ImmutableArray{T}"/> will be treated as having length 0.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveMinimumLength<T>(this ImmutableArray<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            var parameterLength = parameter.IsDefault ? 0 : parameter.Length;
            if (parameterLength < length)
            {
                Throw.InvalidMinimumImmutableArrayLength(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the <see cref = "ImmutableArray{T}"/> has at least the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The <see cref = "ImmutableArray{T}"/> to be checked.</param>
        /// <param name = "length">The minimum length the <see cref = "ImmutableArray{T}"/> should have.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> has less than the specified length.</exception>
        /// <remarks>The default instance of <see cref = "ImmutableArray{T}"/> will be treated as having length 0.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveMinimumLength<T>(this ImmutableArray<T> parameter, int length, Func<ImmutableArray<T>, int, Exception> exceptionFactory)
        {
            var parameterLength = parameter.IsDefault ? 0 : parameter.Length;
            if (parameterLength < length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not null when <typeparamref name = "T"/> is a reference type, or otherwise
        /// throws an <see cref = "ArgumentNullException"/>. PLEASE NOTICE: you should only use this assertion in generic contexts,
        /// use <see cref = "MustNotBeNull{T}(T, string, string)"/> by default.
        /// </summary>
        /// <param name = "parameter">The value to be checked for null.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentNullException">Thrown when <typeparamref name = "T"/> is a reference type and <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNullReference<T>([NotNull, ValidatedNotNull, NoEnumeration] this T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (default(T) != null)
            {
                // If we end up here, parameter cannot be null
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.

                return parameter;
#pragma warning restore CS8777
            }

            if (parameter is null)
            {
                Throw.ArgumentNull(parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified parameter is not null when <typeparamref name = "T"/> is a reference type, or otherwise
        /// throws your custom exception. PLEASE NOTICE: you should only use this assertion in generic contexts,
        /// use <see cref = "MustNotBeNull{T}(T, Func{Exception})"/> by default.
        /// </summary>
        /// <param name = "parameter">The value to be checked for null.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <typeparamref name = "T"/> is a reference type and <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeNullReference<T>([NotNull, ValidatedNotNull, NoEnumeration] this T parameter, Func<Exception> exceptionFactory)
        {
            if (default(T) != null)
            {
                // If we end up here, parameter cannot be null
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.

                return parameter;
#pragma warning restore CS8777
            }

            if (parameter is null)
            {
                Throw.CustomException(exceptionFactory);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not end with the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must not end with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search (optional). The default value is <see cref = "StringComparison.CurrentCulture"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> ends with <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        public static string MustNotEndWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType = StringComparison.CurrentCulture, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).EndsWith(value, comparisonType))
            {
                Throw.StringEndsWith(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not end with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must not end with.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> ends with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustNotEndWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, [NotNull, ValidatedNotNull] Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || parameter.EndsWith(value))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string does not end with the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The other string <paramref name = "parameter"/> must not end with.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> ends with <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; value:null => halt; exceptionFactory:null => halt")]
        public static string MustNotEndWith([NotNull, ValidatedNotNull] this string? parameter, [NotNull, ValidatedNotNull] string value, StringComparison comparisonType, [NotNull, ValidatedNotNull] Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract -- caller might have NRTs turned off
            if (parameter is null || value is null || parameter.EndsWith(value, comparisonType))
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string has the specified length, or otherwise throws a <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The asserted length of the string.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "StringLengthException">Thrown when <paramref name = "parameter"/> has a length other than <paramref name = "length"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustHaveLength([NotNull][ValidatedNotNull] this string? parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).Length != length)
            {
                Throw.StringLengthNotEqualTo(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "length">The asserted length of the string.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> is null or when it has a length other than <paramref name = "length"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustHaveLength([NotNull][ValidatedNotNull] this string? parameter, int length, Func<string?, int, Exception> exceptionFactory)
        {
            if (parameter is null || parameter.Length != length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length that the span must have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustHaveLength<T>(this Span<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            ((ReadOnlySpan<T>)parameter).MustHaveLength(length, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length that the span must have.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> MustHaveLength<T>(this Span<T> parameter, int length, SpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length != length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length that the span must have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustHaveLength<T>(this ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.Length != length)
            {
                Throw.InvalidSpanLength(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the span has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The span to be checked.</param>
        /// <param name = "length">The length that the span must have.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> MustHaveLength<T>(this ReadOnlySpan<T> parameter, int length, ReadOnlySpanExceptionFactory<T, int> exceptionFactory)
        {
            if (parameter.Length != length)
            {
                Throw.CustomSpanException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the immutable array has the specified length, or otherwise throws an <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameter">The immutable array to be checked.</param>
        /// <param name = "length">The length that the immutable array must have.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "InvalidCollectionCountException">Thrown when <paramref name = "parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveLength<T>(this ImmutableArray<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            var actualLength = parameter.IsDefault ? 0 : parameter.Length;
            if (actualLength != length)
            {
                Throw.InvalidImmutableArrayLength(parameter, length, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the immutable array has the specified length, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The immutable array to be checked.</param>
        /// <param name = "length">The length that the immutable array must have.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "length"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not have the specified length.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustHaveLength<T>(this ImmutableArray<T> parameter, int length, Func<ImmutableArray<T>, int, Exception> exceptionFactory)
        {
            var actualLength = parameter.IsDefault ? 0 : parameter.Length;
            if (actualLength != length)
            {
                Throw.CustomException(exceptionFactory, parameter, length);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the specified type derives from the other type. Internally, this method uses <see cref = "IsEquivalentTypeTo"/>
        /// by default so that constructed generic types and their corresponding generic type definitions are regarded as equal.
        /// </summary>
        /// <param name = "type">The type info to be checked.</param>
        /// <param name = "baseClass">The base class that <paramref name = "type"/> should derive from.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/> or <paramref name = "baseClass"/> is null.</exception>
        [ContractAnnotation("type:null => halt; baseClass:null => halt")]
        public static bool DerivesFrom([NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type baseClass)
        {
            baseClass.MustNotBeNull(nameof(baseClass));
            var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
            while (currentBaseType != null)
            {
                if (currentBaseType.IsEquivalentTypeTo(baseClass))
                {
                    return true;
                }

                currentBaseType = currentBaseType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified type derives from the other type. This overload uses the specified <paramref name = "typeComparer"/>
        /// to compare the types.
        /// </summary>
        /// <param name = "type">The type info to be checked.</param>
        /// <param name = "baseClass">The base class that <paramref name = "type"/> should derive from.</param>
        /// <param name = "typeComparer">The equality comparer used to compare the types.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "type"/>, or <paramref name = "baseClass"/>, or <paramref name = "typeComparer"/> is null.</exception>
        [ContractAnnotation("type:null => halt; baseClass:null => halt; typeComparer:null => halt")]
        public static bool DerivesFrom([NotNull][ValidatedNotNull] this Type type, [NotNull][ValidatedNotNull] Type baseClass, [NotNull][ValidatedNotNull] IEqualityComparer<Type> typeComparer)
        {
            baseClass.MustNotBeNull(nameof(baseClass));
            typeComparer.MustNotBeNull(nameof(typeComparer));
            var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
            while (currentBaseType != null)
            {
                if (typeComparer.Equals(currentBaseType, baseClass))
                {
                    return true;
                }

                currentBaseType = currentBaseType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeGreaterThanOrApproximately(this double parameter, double other, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null) => parameter.MustBeGreaterThanOrApproximately(other, 0.0001, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeGreaterThanOrApproximately(this double parameter, double other, Func<double, double, Exception> exceptionFactory)
        {
            if (!parameter.IsGreaterThanOrApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeGreaterThanOrApproximately(this double parameter, double other, double tolerance, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsGreaterThanOrApproximately(other, tolerance))
            {
                Throw.MustBeGreaterThanOrApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "other"/>, and <paramref name = "tolerance"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeGreaterThanOrApproximately(this double parameter, double other, double tolerance, Func<double, double, double, Exception> exceptionFactory)
        {
            if (!parameter.IsGreaterThanOrApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001f, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeGreaterThanOrApproximately(this float parameter, float other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => parameter.MustBeGreaterThanOrApproximately(other, 0.0001f, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeGreaterThanOrApproximately(this float parameter, float other, Func<float, float, Exception> exceptionFactory)
        {
            if (!parameter.IsGreaterThanOrApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeGreaterThanOrApproximately(this float parameter, float other, float tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsGreaterThanOrApproximately(other, tolerance))
            {
                Throw.MustBeGreaterThanOrApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be greater than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "other"/>, and <paramref name = "tolerance"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> is not greater than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeGreaterThanOrApproximately(this float parameter, float other, float tolerance, Func<float, float, float, Exception> exceptionFactory)
        {
            if (!parameter.IsGreaterThanOrApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is less than or equal to <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeGreaterThan<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) <= 0)
            {
                Throw.MustBeGreaterThan(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is greater than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is less than or equal to <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeGreaterThan<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) <= 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeLessThanOrApproximately(this double parameter, double other, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null) => parameter.MustBeLessThanOrApproximately(other, 0.0001, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeLessThanOrApproximately(this double parameter, double other, Func<double, double, Exception> exceptionFactory)
        {
            if (!parameter.IsLessThanOrApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeLessThanOrApproximately(this double parameter, double other, double tolerance, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsLessThanOrApproximately(other, tolerance))
            {
                Throw.MustBeLessThanOrApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "other"/>, and <paramref name = "tolerance"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MustBeLessThanOrApproximately(this double parameter, double other, double tolerance, Func<double, double, double, Exception> exceptionFactory)
        {
            if (!parameter.IsLessThanOrApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001f, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeLessThanOrApproximately(this float parameter, float other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => parameter.MustBeLessThanOrApproximately(other, 0.0001f, parameterName, message);
        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, using the default tolerance of 0.0001, or otherwise throws an
        /// <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/>
        /// are passed to this delegate.
        /// </param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeLessThanOrApproximately(this float parameter, float other, Func<float, float, Exception> exceptionFactory)
        {
            if (!parameter.IsLessThanOrApproximately(other))
            {
                Throw.CustomException(exceptionFactory, parameter, other);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">
        /// Thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeLessThanOrApproximately(this float parameter, float other, float tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.IsLessThanOrApproximately(other, tolerance))
            {
                Throw.MustBeLessThanOrApproximately(parameter, other, tolerance, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is less than or approximately equal to the given
        /// <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The value to be checked.</param>
        /// <param name = "other">The value that <paramref name = "parameter"/> should be less than or approximately equal to.</param>
        /// <param name = "tolerance">The tolerance indicating how much the two values may differ from each other.</param>
        /// <param name = "exceptionFactory">
        /// The delegate that creates your custom exception. <paramref name = "parameter"/>,
        /// <paramref name = "other"/>, and <paramref name = "tolerance"/> are passed to this delegate.
        /// </param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> is not less than or approximately equal to <paramref name = "other"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MustBeLessThanOrApproximately(this float parameter, float other, float tolerance, Func<float, float, float, Exception> exceptionFactory)
        {
            if (!parameter.IsLessThanOrApproximately(other, tolerance))
            {
                Throw.CustomException(exceptionFactory, parameter, other, tolerance);
            }

            return parameter;
        }

        /// <summary>
        /// Checks if the value is not within the specified range.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "range">The range where <paramref name = "parameter"/> must not be in-between.</param>
        /// <returns>True if the parameter is not within the specified range, else false.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotIn<T>([NotNull][ValidatedNotNull] this T parameter, Range<T> range)
            where T : IComparable<T> => !range.IsValueWithinRange(parameter);
        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws a <see cref = "MissingItemException"/>.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "item">The item that must be part of the collection.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "MissingItemException">Thrown when <paramref name = "parameter"/> does not contain <paramref name = "item"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>([NotNull][ValidatedNotNull] this TCollection? parameter, TItem item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                {
                    Throw.MissingItem(parameter, item, parameterName, message);
                }

                return parameter;
            }

            if (!parameter.MustNotBeNull(parameterName, message).Contains(item))
            {
                Throw.MissingItem(parameter, item, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the collection contains the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The collection to be checked.</param>
        /// <param name = "item">The item that must be part of the collection.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "item"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not contain <paramref name = "item"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static TCollection MustContain<TCollection, TItem>([NotNull][ValidatedNotNull] this TCollection? parameter, TItem item, Func<TCollection?, TItem, Exception> exceptionFactory)
            where TCollection : class, IEnumerable<TItem>
        {
            if (parameter is ICollection<TItem> collection)
            {
                if (!collection.Contains(item))
                {
                    Throw.CustomException(exceptionFactory, parameter, item);
                }

                return parameter;
            }

            if (parameter is null || !parameter.Contains(item))
            {
                Throw.CustomException(exceptionFactory, parameter, item);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified substring, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The substring that must be part of <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> does not contain <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain([NotNull][ValidatedNotNull] this string? parameter, string? value, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.MustNotBeNull(parameterName, message).Contains(value.MustNotBeNull(nameof(value), message)))
            {
                Throw.StringDoesNotContain(parameter, value, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The substring that must be part of <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates you custom exception. <paramref name = "parameter"/> and <paramref name = "value"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not contain <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain([NotNull][ValidatedNotNull] this string? parameter, string value, Func<string?, string, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || !parameter.Contains(value))
            {
                Throw.CustomException(exceptionFactory, parameter, value!);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified value, or otherwise throws a <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The substring that must be part of <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "SubstringException">Thrown when <paramref name = "parameter"/> does not contain <paramref name = "value"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> or <paramref name = "value"/> is null.</exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (parameter.MustNotBeNull(parameterName, message).IndexOf(value.MustNotBeNull(nameof(value), message), comparisonType) < 0)
            {
                Throw.StringDoesNotContain(parameter, value, comparisonType, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the string contains the specified value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The string to be checked.</param>
        /// <param name = "value">The substring that must be part of <paramref name = "parameter"/>.</param>
        /// <param name = "comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <param name = "exceptionFactory">The delegate that creates you custom exception. <paramref name = "parameter"/>, <paramref name = "value"/>, and <paramref name = "comparisonType"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">
        /// Your custom exception thrown when <paramref name = "parameter"/> does not contain <paramref name = "value"/>,
        /// or when <paramref name = "parameter"/> is null,
        /// or when <paramref name = "value"/> is null.
        /// </exception>
        /// <exception cref = "ArgumentException">Thrown when <paramref name = "comparisonType"/> is not a valid <see cref = "StringComparison"/> value.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static string MustContain([NotNull][ValidatedNotNull] this string? parameter, string value, StringComparison comparisonType, Func<string?, string, StringComparison, Exception> exceptionFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || value is null || parameter.IndexOf(value, comparisonType) < 0)
            {
                Throw.CustomException(exceptionFactory, parameter, value!, comparisonType);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the immutable array contains the specified item, or otherwise throws a <see cref = "MissingItemException"/>.
        /// </summary>
        /// <param name = "parameter">The immutable array to be checked.</param>
        /// <param name = "item">The item that must be part of the immutable array.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "MissingItemException">Thrown when <paramref name = "parameter"/> does not contain <paramref name = "item"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustContain<T>(this ImmutableArray<T> parameter, T item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            if (!parameter.Contains(item))
            {
                Throw.MissingItem(parameter, item, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the immutable array contains the specified item, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The immutable array to be checked.</param>
        /// <param name = "item">The item that must be part of the immutable array.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "item"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when <paramref name = "parameter"/> does not contain <paramref name = "item"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImmutableArray<T> MustContain<T>(this ImmutableArray<T> parameter, T item, Func<ImmutableArray<T>, T, Exception> exceptionFactory)
        {
            if (!parameter.Contains(item))
            {
                Throw.CustomException(exceptionFactory, parameter, item);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not less than the given <paramref name = "other"/> value, or otherwise throws an <see cref = "ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message that will be passed to the resulting exception (optional).</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when the specified <paramref name = "parameter"/> is less than <paramref name = "other"/>.</exception>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeLessThan<T>([NotNull][ValidatedNotNull] this T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) < 0)
            {
                Throw.MustNotBeLessThan(parameter, other, parameterName, message);
            }

            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name = "parameter"/> is not less than the given <paramref name = "other"/> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name = "parameter">The comparable to be checked.</param>
        /// <param name = "other">The boundary value that must be less than or equal to <paramref name = "parameter"/>.</param>
        /// <param name = "exceptionFactory">The delegate that creates your custom exception. <paramref name = "parameter"/> and <paramref name = "other"/> are passed to this delegate.</param>
        /// <exception cref = "Exception">Your custom exception thrown when the specified <paramref name = "parameter"/> is less than <paramref name = "other"/>, or when <paramref name = "parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeLessThan<T>([NotNull][ValidatedNotNull] this T parameter, T other, Func<T, T, Exception> exceptionFactory)
            where T : IComparable<T>
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract - caller might have NRTs turned off
            if (parameter is null || parameter.CompareTo(other) < 0)
            {
                Throw.CustomException(exceptionFactory, parameter!, other);
            }

            return parameter;
        }
    }

    /// <summary>
    /// Represents an <see cref = "IEqualityComparer{T}"/> that uses <see cref = "Check.IsEquivalentTypeTo"/>
    /// to compare types. This check works like the normal type equality comparison, but when two
    /// generic types are compared, they are regarded as equal when one of them is a constructed generic type
    /// and the other one is the corresponding generic type definition.
    /// </summary>
    internal sealed class EquivalentTypeComparer : IEqualityComparer<Type>
    {
        /// <summary>
        /// Gets a singleton instance of the equality comparer.
        /// </summary>
        public static readonly EquivalentTypeComparer Instance = new();
        /// <summary>
        /// Checks if the two types are equivalent (using <see cref = "Check.IsEquivalentTypeTo"/>).
        /// This check works like the normal type equality comparison, but when two
        /// generic types are compared, they are regarded as equal when one of them is a constructed generic type
        /// and the other one is the corresponding generic type definition.
        /// </summary>
        /// <param name = "x">The first type.</param>
        /// <param name = "y">The second type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Type? x, Type? y) => x.IsEquivalentTypeTo(y);
        /// <summary>
        /// Returns the hash code of the given type. When the specified type is a constructed generic type,
        /// the hash code of the generic type definition is returned instead.
        /// </summary>
        /// <param name = "type">The type whose hash code is requested.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(Type type) => // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        type is null ? 0 : type.IsConstructedGenericType ? type.GetGenericTypeDefinition().GetHashCode() : type.GetHashCode();
    }

    /// <summary>
    /// Defines a range that can be used to check if a specified <see cref = "IComparable{T}"/> is in between it or not.
    /// </summary>
    /// <typeparam name = "T">The type that the range should be applied to.</typeparam>
    internal readonly struct Range<T> : IEquatable<Range<T>> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the lower boundary of the range.
        /// </summary>
        public readonly T From;
        /// <summary>
        /// Gets the upper boundary of the range.
        /// </summary>
        public readonly T To;
        /// <summary>
        /// Gets the value indicating whether the From value is included in the range.
        /// </summary>
        public readonly bool IsFromInclusive;
        /// <summary>
        /// Gets the value indicating whether the To value is included in the range.
        /// </summary>
        public readonly bool IsToInclusive;
        private readonly int _expectedLowerBoundaryResult;
        private readonly int _expectedUpperBoundaryResult;
        /// <summary>
        /// Creates a new instance of <see cref = "Range{T}"/>.
        /// </summary>
        /// <param name = "from">The lower boundary of the range.</param>
        /// <param name = "to">The upper boundary of the range.</param>
        /// <param name = "isFromInclusive">The value indicating whether <paramref name = "from"/> is part of the range.</param>
        /// <param name = "isToInclusive">The value indicating whether <paramref name = "to"/> is part of the range.</param>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when <paramref name = "to"/> is less than <paramref name = "from"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Range(T from, T to, bool isFromInclusive = true, bool isToInclusive = true)
        {
            From = from.MustNotBeNullReference(nameof(from));
            To = to.MustNotBeLessThan(from, nameof(to));
            IsFromInclusive = isFromInclusive;
            IsToInclusive = isToInclusive;
            _expectedLowerBoundaryResult = isFromInclusive ? 0 : 1;
            _expectedUpperBoundaryResult = isToInclusive ? 0 : -1;
        }

        /// <summary>
        /// Checks if the specified <paramref name = "value"/> is within range.
        /// </summary>
        /// <param name = "value">The value to be checked.</param>
        /// <returns>True if value is within range, otherwise false.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsValueWithinRange(T value) => value.MustNotBeNullReference(nameof(value)).CompareTo(From) >= _expectedLowerBoundaryResult && value.CompareTo(To) <= _expectedUpperBoundaryResult;
        /// <summary>
        /// Use this method to create a range in a fluent style using method chaining.
        /// Defines the lower boundary as an inclusive value.
        /// </summary>
        /// <param name = "value">The value that indicates the inclusive lower boundary of the resulting range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFromInfo FromInclusive(T value) => new(value, true);
        /// <summary>
        /// Use this method to create a range in a fluent style using method chaining.
        /// Defines the lower boundary as an exclusive value.
        /// </summary>
        /// <param name = "value">The value that indicates the exclusive lower boundary of the resulting range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeFromInfo FromExclusive(T value) => new(value, false);
        /// <summary>
        /// The nested <see cref = "RangeFromInfo"/> can be used to fluently create a <see cref = "Range{T}"/>.
        /// </summary>
        public readonly struct RangeFromInfo
        {
            private readonly T _from;
            private readonly bool _isFromInclusive;
            /// <summary>
            /// Creates a new RangeFromInfo.
            /// </summary>
            /// <param name = "from">The lower boundary of the range.</param>
            /// <param name = "isFromInclusive">The value indicating whether <paramref name = "from"/> is part of the range.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public RangeFromInfo(T from, bool isFromInclusive)
            {
                _from = from;
                _isFromInclusive = isFromInclusive;
            }

            /// <summary>
            /// Use this method to create a range in a fluent style using method chaining.
            /// Defines the upper boundary as an exclusive value.
            /// </summary>
            /// <param name = "value">The value that indicates the exclusive upper boundary of the resulting range.</param>
            /// <returns>A new range with the specified upper and lower boundaries.</returns>
            /// <exception cref = "ArgumentOutOfRangeException">
            /// Thrown when <paramref name = "value"/> is less than the lower boundary value.
            /// </exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Range<T> ToExclusive(T value) => new(_from, value, _isFromInclusive, false);
            /// <summary>
            /// Use this method to create a range in a fluent style using method chaining.
            /// Defines the upper boundary as an inclusive value.
            /// </summary>
            /// <param name = "value">The value that indicates the inclusive upper boundary of the resulting range.</param>
            /// <returns>A new range with the specified upper and lower boundaries.</returns>
            /// <exception cref = "ArgumentOutOfRangeException">
            /// Thrown when <paramref name = "value"/> is less than the lower boundary value.
            /// </exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Range<T> ToInclusive(T value) => new(_from, value, _isFromInclusive);
        }

        /// <inheritdoc/>
        public override string ToString() => $"Range from {CreateRangeDescriptionText()}";
        /// <summary>
        /// Returns either "inclusive" or "exclusive", depending on whether <see cref = "IsFromInclusive"/> is true or false.
        /// </summary>
        public string LowerBoundaryText {[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBoundaryText(IsFromInclusive); }
        /// <summary>
        /// Returns either "inclusive" or "exclusive", depending on whether <see cref = "IsToInclusive"/> is true or false.
        /// </summary>
        public string UpperBoundaryText {[MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBoundaryText(IsToInclusive); }

        /// <summary>
        /// Returns a text description of this range with the following pattern: From (inclusive | exclusive) to To (inclusive | exclusive).
        /// </summary>
        public string CreateRangeDescriptionText(string fromToConnectionWord = "to") => From + " (" + LowerBoundaryText + ") " + fromToConnectionWord + ' ' + To + " (" + UpperBoundaryText + ")";
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetBoundaryText(bool isInclusive) => isInclusive ? "inclusive" : "exclusive";
        /// <inheritdoc/>
        public bool Equals(Range<T> other)
        {
            if (IsFromInclusive != other.IsFromInclusive || IsToInclusive != other.IsToInclusive)
            {
                return false;
            }

            var comparer = EqualityComparer<T>.Default;
            return comparer.Equals(From, other.From) && comparer.Equals(To, other.To);
        }

        /// <inheritdoc/>
        public override bool Equals(object? other)
        {
            if (other is null)
            {
                return false;
            }

            return other is Range<T> range && Equals(range);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => MultiplyAddHash.CreateHashCode(From, To, IsFromInclusive, IsToInclusive);
        /// <summary>
        /// Checks if two ranges are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Range<T> first, Range<T> second) => first.Equals(second);
        /// <summary>
        /// Checks if two ranges are not equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Range<T> first, Range<T> second) => first.Equals(second) == false;
    }

    /// <summary>
    /// Provides methods to simplify the creation of <see cref = "Range{T}"/> instances.
    /// </summary>
    internal static class Range
    {
        /// <summary>
        /// Use this method to create a range in a fluent style using method chaining.
        /// Defines the lower boundary as an inclusive value.
        /// </summary>
        /// <param name = "value">The value that indicates the inclusive lower boundary of the resulting range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<T>.RangeFromInfo FromInclusive<T>(T value)
            where T : IComparable<T> => new(value, true);
        /// <summary>
        /// Use this method to create a range in a fluent style using method chaining.
        /// Defines the lower boundary as an exclusive value.
        /// </summary>
        /// <param name = "value">The value that indicates the exclusive lower boundary of the resulting range.</param>
        /// <returns>A value you can use to fluently define the upper boundary of a new range.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<T>.RangeFromInfo FromExclusive<T>(T value)
            where T : IComparable<T> => new(value, false);
        /// <summary>
        /// Creates a range with both boundaries inclusive.
        /// </summary>
        /// <param name = "from">The lower boundary of the range.</param>
        /// <param name = "to">The upper boundary of the range.</param>
        /// <returns>A new range with both boundaries inclusive.</returns>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when <paramref name = "to"/> is less than <paramref name = "from"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<T> InclusiveBetween<T>(T from, T to)
            where T : IComparable<T> => new(from, to);
        /// <summary>
        /// Creates a range with both boundaries exclusive.
        /// </summary>
        /// <param name = "from">The lower boundary of the range.</param>
        /// <param name = "to">The upper boundary of the range.</param>
        /// <returns>A new range with both boundaries exclusive.</returns>
        /// <exception cref = "ArgumentOutOfRangeException">Thrown when <paramref name = "to"/> is less than <paramref name = "from"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<T> ExclusiveBetween<T>(T from, T to)
            where T : IComparable<T> => new(from, to, false, false);
        /// <summary>
        /// Creates a range for the specified enumerable that encompasses all valid indexes.
        /// </summary>
        /// <param name = "enumerable">
        /// The count of this enumerable will be used to create the index range. Please ensure that this enumerable
        /// is actually a collection, not a lazy enumerable.
        /// </param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<int> For(IEnumerable enumerable) => new(0, enumerable.Count(), true, false);
        /// <summary>
        /// Creates a range for the specified enumerable that encompasses all valid indexes.
        /// </summary>
        /// <param name = "enumerable">
        /// The count of this enumerable will be used to create the index range. Please ensure that this enumerable
        /// is actually a collection, not a lazy enumerable.
        /// </param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> is null.</exception>
        public static Range<int> For<T>(IEnumerable<T> enumerable) => new(0, enumerable.GetCount(), true, false);
        /// <summary>
        /// Creates a range for the specified span that encompasses all valid indexes.
        /// </summary>
        /// <param name = "span">
        /// The length of the span is used to create a valid index range.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<int> For<T>(ReadOnlySpan<T> span) => new(0, span.Length, true, false);
        /// <summary>
        /// Creates a range for the specified span that encompasses all valid indexes.
        /// </summary>
        /// <param name = "span">
        /// The length of the span is used to create a valid index range.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<int> For<T>(Span<T> span) => new(0, span.Length, true, false);
        /// <summary>
        /// Creates a range for the specified memory that encompasses all valid indexes.
        /// </summary>
        /// <param name = "memory">
        /// The length of the memory is used to create a valid index range.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<int> For<T>(Memory<T> memory) => new(0, memory.Length, true, false);
        /// <summary>
        /// Creates a range for the specified memory that encompasses all valid indexes.
        /// </summary>
        /// <param name = "memory">
        /// The length of the memory is used to create a valid index range.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<int> For<T>(ReadOnlyMemory<T> memory) => new(0, memory.Length, true, false);
        /// <summary>
        /// Creates a range for the specified memory that encompasses all valid indexes.
        /// </summary>
        /// <param name = "segment">
        /// The count of the segment is used to create a valid index range.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Range<int> For<T>(ArraySegment<T> segment) => new(0, segment.Count, true, false);
    }

    /// <summary>
    /// Provides regular expressions that are used in string assertions.
    /// </summary>
#if NET8_0
public static partial class RegularExpressions
#else
    internal static class RegularExpressions
#endif
    {
        /// <summary>
        /// Gets the string that represents the <see cref = "EmailRegex"/>.
        /// </summary>
         // This is an AI-generated regex. I don't have any clue and I find it way to complex to ever understand it.
        public const string EmailRegexText = @"^(?:(?:""(?:(?:[^""\\]|\\.)*)""|[\p{L}\p{N}!#$%&'*+\-/=?^_`{|}~-]+(?:\.[\p{L}\p{N}!#$%&'*+\-/=?^_`{|}~-]+)*)@(?:(?:[A-Za-z0-9](?:[A-Za-z0-9\-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}|(?:\[(?:IPv6:[0-9A-Fa-f:.]+)\])|(?:25[0-5]|2[0-4]\d|[01]?\d?\d)(?:\.(?:25[0-5]|2[0-4]\d|[01]?\d?\d)){3}))$";
        /// <summary>
        /// Gets the default regular expression for email validation.
        /// This pattern is based on https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/ and
        /// was modified to satisfy all tests of https://blogs.msdn.microsoft.com/testing123/2009/02/06/email-address-test-cases/.
        /// </summary>
        public static readonly Regex EmailRegex = 
#if NET8_0
        GenerateEmailRegex();

    [GeneratedRegex(EmailRegexText, RegexOptions.ECMAScript | RegexOptions.CultureInvariant)]
    private static partial Regex GenerateEmailRegex();
#else
        new(EmailRegexText, RegexOptions.ECMAScript | RegexOptions.CultureInvariant | RegexOptions.Compiled);
#endif
    }

    /// <summary>
    /// This class caches <see cref = "Type"/> instances to avoid use of the typeof operator.
    /// </summary>
    internal abstract class Types
    {
        /// <summary>
        /// Gets the <see cref = "FlagsAttribute"/> type.
        /// </summary>
        public static readonly Type FlagsAttributeType = typeof(FlagsAttribute);
    }

    /// <summary>
    /// Specifies the culture, case , and sort rules when comparing strings.
    /// </summary>
    /// <remarks>
    /// This enum is en extension of <see cref = "System.StringComparison"/>, adding
    /// capabilities to ignore white space when making string equality comparisons.
    /// See the <see cref = "Check.Equals(string, string, StringComparisonType)"/> when
    /// you want to compare in such a way.
    /// </remarks>
    internal enum StringComparisonType
    {
        /// <summary>
        /// Compare strings using culture-sensitive sort rules and the current culture.
        /// </summary>
        CurrentCulture = 0,
        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the current culture, and
        /// ignoring the case of the strings being compared.
        /// </summary>
        CurrentCultureIgnoreCase = 1,
        /// <summary>
        /// Compare strings using culture-sensitive sort rules and the invariant culture.
        /// </summary>
        InvariantCulture = 2,
        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the invariant culture, and
        /// ignoring the case of the strings being compared.
        /// </summary>
        InvariantCultureIgnoreCase = 3,
        /// <summary>
        /// Compare strings using ordinal sort rules.
        /// </summary>
        Ordinal = 4,
        /// <summary>
        /// Compare strings using ordinal sort rules and ignoring the case of the strings
        /// being compared.
        /// </summary>
        OrdinalIgnoreCase = 5,
        /// <summary>
        /// Compare strings using ordinal sort rules and ignoring the white space characters
        /// of the strings being compared.
        /// </summary>
        OrdinalIgnoreWhiteSpace = 6,
        /// <summary>
        /// Compare strings using ordinal sort rules, ignoring the case and ignoring the
        /// white space characters of the strings being compared.
        /// </summary>
        OrdinalIgnoreCaseIgnoreWhiteSpace = 7,
    }

    /// <summary>
    /// Represents an <see cref = "IEqualityComparer{T}"/> that compares strings using the
    /// ordinal sort rules, ignoring the case and the white space characters.
    /// </summary>
    internal sealed class OrdinalIgnoreCaseIgnoreWhiteSpaceComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Checks if the two strings are equal using ordinal sorting rules as well as ignoring the case and
        /// the white space of the provided strings.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "x"/> or <paramref name = "y"/> are null.</exception>
        public bool Equals(string? x, string? y)
        {
            x.MustNotBeNull(nameof(x));
            y.MustNotBeNull(nameof(y));
            return x.EqualsOrdinalIgnoreCaseIgnoreWhiteSpace(y);
        }

        /// <summary>
        /// Gets the hash code for the specified string. The hash code is created only from the non-white space characters
        /// which are interpreted as case-insensitive.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "string"/> is null.</exception>
        public int GetHashCode(string @string)
        {
            @string.MustNotBeNull(nameof(@string));
            var hashBuilder = MultiplyAddHashBuilder.Create();
            foreach (var character in @string)
            {
                if (!character.IsWhiteSpace())
                {
                    hashBuilder.CombineIntoHash(char.ToLowerInvariant(character));
                }
            }

            return hashBuilder.BuildHash();
        }
    }

    /// <summary>
    /// Provides meta-information about enum values and the flag bitmask if the enum is marked with the <see cref = "FlagsAttribute"/>.
    /// Can be used to validate that an enum value is valid.
    /// </summary>
    /// <typeparam name = "T">The type of the enum.</typeparam>
    internal static class EnumInfo<T>
        where T : struct, Enum
    {
        // ReSharper disable StaticMemberInGenericType
        /// <summary>
        /// Gets the value indicating whether the enum type is marked with the flags attribute.
        /// </summary>
        public static readonly bool IsFlagsEnum = typeof(T).GetCustomAttribute(Types.FlagsAttributeType) != null;
        /// <summary>
        /// Gets the flags pattern when <see cref = "IsFlagsEnum"/> is true. If the enum is not a flags enum, then 0UL is returned.
        /// </summary>
        public static readonly ulong FlagsPattern;
        private static readonly int EnumSize = Unsafe.SizeOf<T>();
        private static readonly T[] EnumConstantsArray;
        /// <summary>
        /// Gets the values of the enum as a read-only collection.
        /// </summary>
        public static ReadOnlyMemory<T> EnumConstants { get; }

        static EnumInfo()
        {
#if NET8_0
        EnumConstantsArray = Enum.GetValues<T>();
#else
            EnumConstantsArray = (T[])Enum.GetValues(typeof(T));
#endif
            EnumConstants = new ReadOnlyMemory<T>(EnumConstantsArray);
            if (!IsFlagsEnum)
            {
                return;
            }

            for (var i = 0; i < EnumConstantsArray.Length; ++i)
            {
                var convertedValue = ConvertToUInt64(EnumConstantsArray[i]);
                FlagsPattern |= convertedValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidFlagsValue(T enumValue)
        {
            var convertedValue = ConvertToUInt64(enumValue);
            return (FlagsPattern & convertedValue) == convertedValue;
        }

        private static bool IsValidValue(T parameter)
        {
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < EnumConstantsArray.Length; ++i)
            {
                if (comparer.Equals(EnumConstantsArray[i], parameter))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified enum value is valid. This is true if either the enum is a standard enum and the enum value corresponds
        /// to one of the enum constant values or if the enum type is marked with the <see cref = "FlagsAttribute"/> and the given value
        /// is a valid combination of bits for this type.
        /// </summary>
        /// <param name = "enumValue">The enum value to be checked.</param>
        /// <returns>True if either the enum value is </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsValidEnumValue(T enumValue) => IsFlagsEnum ? IsValidFlagsValue(enumValue) : IsValidValue(enumValue);
        private static ulong ConvertToUInt64(T value)
        {
            switch (EnumSize)
            {
                case 1:
                    return Unsafe.As<T, byte>(ref value);
                case 2:
                    return Unsafe.As<T, ushort>(ref value);
                case 4:
                    return Unsafe.As<T, uint>(ref value);
                case 8:
                    return Unsafe.As<T, ulong>(ref value);
                default:
                    ThrowUnknownEnumSize();
                    return 0UL;
            }
        }

        private static void ThrowUnknownEnumSize() => throw new InvalidOperationException($"The enum type \"{typeof(T)}\" has an unknown size of {EnumSize}. This means that the underlying enum type is not one of the supported ones.");
    }

    /// <summary>
    /// Represents an <see cref = "IEqualityComparer{T}"/> that compares strings using the
    /// ordinal sort rules and ignoring the white space characters.
    /// </summary>
    internal sealed class OrdinalIgnoreWhiteSpaceComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Checks if the two strings are equal using ordinal sorting rules as well as ignoring the white space
        /// of the provided strings.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "x"/> or <paramref name = "y"/> are null.</exception>
        public bool Equals(string? x, string? y)
        {
            x.MustNotBeNull(nameof(x));
            y.MustNotBeNull(nameof(y));
            return x.EqualsOrdinalIgnoreWhiteSpace(y);
        }

        /// <summary>
        /// Gets the hash code for the specified string. The hash code is created only from the non-white space characters.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "string"/> is null.</exception>
        public int GetHashCode(string @string)
        {
            @string.MustNotBeNull(nameof(@string));
            var hashCodeBuilder = MultiplyAddHashBuilder.Create();
            foreach (var character in @string)
            {
                if (!character.IsWhiteSpace())
                {
                    hashCodeBuilder.CombineIntoHash(character);
                }
            }

            return hashCodeBuilder.BuildHash();
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}

namespace Light.GuardClauses.Exceptions
{
    /// <summary>
    /// This exception indicates that a value is not defined in the corresponding enum type.
    /// </summary>
    [Serializable]
    internal class EnumValueNotDefinedException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "EnumValueNotDefinedException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter.</param>
        /// <param name = "message">The message of the exception.</param>
        public EnumValueNotDefinedException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected EnumValueNotDefinedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that configuration data is invalid.
    /// </summary>
    [Serializable]
    internal class InvalidConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref = "InvalidConfigurationException"/>.
        /// </summary>
        /// <param name = "message">The message of the exception (optional).</param>
        /// <param name = "innerException">The exception that is the cause of this one (optional).</param>
        public InvalidConfigurationException(string? message = null, Exception? innerException = null) : base(message, innerException)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an URI is relative instead of absolute.
    /// </summary>
    [Serializable]
    internal class RelativeUriException : UriException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "RelativeUriException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public RelativeUriException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected RelativeUriException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a collection contains an item that must not be part of it.
    /// </summary>
    [Serializable]
    internal class ExistingItemException : CollectionException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "ExistingItemException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public ExistingItemException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected ExistingItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an item is not present in a collection.
    /// </summary>
    [Serializable]
    internal class MissingItemException : CollectionException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "MissingItemException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public MissingItemException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected MissingItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an Email address is invalid.
    /// </summary>
    [Serializable]
    internal class InvalidEmailAddressException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "InvalidEmailAddressException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public InvalidEmailAddressException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected InvalidEmailAddressException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that two values are equal.
    /// </summary>
    [Serializable]
    internal class ValuesEqualException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "ValuesEqualException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public ValuesEqualException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected ValuesEqualException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a string has an invalid length.
    /// </summary>
    [Serializable]
    internal class StringLengthException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "StringLengthException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public StringLengthException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected StringLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a string is in an invalid state.
    /// </summary>
    [Serializable]
    internal class SubstringException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "SubstringException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public SubstringException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected SubstringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a collection has no items.
    /// </summary>
    [Serializable]
    internal class EmptyCollectionException : InvalidCollectionCountException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "EmptyCollectionException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public EmptyCollectionException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected EmptyCollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a value of a value type is the default value.
    /// </summary>
    [Serializable]
    internal class ArgumentDefaultException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "ArgumentDefaultException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public ArgumentDefaultException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected ArgumentDefaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a collection has an invalid number of items.
    /// </summary>
    [Serializable]
    internal class InvalidCollectionCountException : CollectionException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "InvalidCollectionCountException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public InvalidCollectionCountException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected InvalidCollectionCountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that the state of a collection is invalid.
    /// </summary>
    [Serializable]
    internal class CollectionException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "CollectionException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public CollectionException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected CollectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a string is in an invalid state.
    /// </summary>
    [Serializable]
    internal class StringException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "StringException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public StringException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected StringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a <see cref = "Nullable{T}"/> has no value.
    /// </summary>
    [Serializable]
    internal class NullableHasNoValueException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "NullableHasNoValueException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public NullableHasNoValueException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected NullableHasNoValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that two references point to the same object.
    /// </summary>
    [Serializable]
    internal class SameObjectReferenceException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "SameObjectReferenceException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public SameObjectReferenceException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected SameObjectReferenceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an URI has an invalid scheme.
    /// </summary>
    [Serializable]
    internal class InvalidUriSchemeException : UriException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public InvalidUriSchemeException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected InvalidUriSchemeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an URI is absolute instead of relative.
    /// </summary>
    [Serializable]
    internal class AbsoluteUriException : UriException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "AbsoluteUriException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public AbsoluteUriException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected AbsoluteUriException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that that a GUID is empty.
    /// </summary>
    [Serializable]
    internal class EmptyGuidException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "EmptyGuidException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public EmptyGuidException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected EmptyGuidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an URI is invalid.
    /// </summary>
    [Serializable]
    internal class UriException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "UriException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public UriException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected UriException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that two values are not equal.
    /// </summary>
    [Serializable]
    internal class ValuesNotEqualException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "ValuesNotEqualException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public ValuesNotEqualException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected ValuesNotEqualException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a string contains only white space.
    /// </summary>
    [Serializable]
    internal class WhiteSpaceStringException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "WhiteSpaceStringException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public WhiteSpaceStringException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected WhiteSpaceStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an item is part of a collection.
    /// </summary>
    [Serializable]
    internal class ValueIsOneOfException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "ValueIsOneOfException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public ValueIsOneOfException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected ValueIsOneOfException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a string is not matching a regular expression.
    /// </summary>
    [Serializable]
    internal class StringDoesNotMatchException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "StringDoesNotMatchException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public StringDoesNotMatchException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected StringDoesNotMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a <see cref = "DateTime"/> value is invalid.
    /// </summary>
    [Serializable]
    internal class InvalidDateTimeException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "InvalidDateTimeException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public InvalidDateTimeException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected InvalidDateTimeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a string is empty.
    /// </summary>
    [Serializable]
    internal class EmptyStringException : StringException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "EmptyStringException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public EmptyStringException(string? parameterName = null, string? message = null) : base(parameterName, message)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected EmptyStringException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that an item is not part of a collection.
    /// </summary>
    [Serializable]
    internal class ValueIsNotOneOfException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "CollectionException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public ValueIsNotOneOfException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected ValueIsNotOneOfException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that a value cannot be cast to another type.
    /// </summary>
    [Serializable]
    internal class TypeCastException : ArgumentException
    {
        /// <summary>
        /// Creates a new instance of <see cref = "TypeCastException"/>.
        /// </summary>
        /// <param name = "parameterName">The name of the parameter (optional).</param>
        /// <param name = "message">The message of the exception (optional).</param>
        public TypeCastException(string? parameterName = null, string? message = null) : base(message, parameterName)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected TypeCastException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }

    /// <summary>
    /// This exception indicates that the data is in invalid state.
    /// </summary>
    [Serializable]
    internal class InvalidStateException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref = "InvalidStateException"/>.
        /// </summary>
        /// <param name = "message">The message of the exception (optional).</param>
        /// <param name = "innerException">The exception that is the cause of this one (optional).</param>
        public InvalidStateException(string? message = null, Exception? innerException = null) : base(message, innerException)
        {
        }

#if !NET8_0
        /// <inheritdoc/>
        protected InvalidStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}

namespace Light.GuardClauses.ExceptionFactory
{
    /// <summary>
    /// Provides static factory methods that throw default exceptions.
    /// </summary>
     // ReSharper disable once RedundantTypeDeclarationBody - requried for the Source Code Transformation
    internal static class Throw
    {
        /// <summary>
        /// Throws the default <see cref = "ArgumentNullException"/>, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ArgumentNull(string? parameterName = null, string? message = null) => throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        /// <summary>
        /// Throws the default <see cref = "EmptyStringException"/> indicating that a string is empty, using the optional
        /// parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void EmptyString(string? parameterName = null, string? message = null) => throw new EmptyStringException(parameterName, message ?? $"{parameterName ?? "The string"} must not be an empty string, but it actually is.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that an <see cref = "ImmutableArray{T}"/> has less than a
        /// minimum number of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidMinimumImmutableArrayLength<T>(ImmutableArray<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The immutable array"} must have at least a length of {length}, but it actually {(parameter.IsDefault ? "has no length because it is the default instance" : $"has a length of {parameter.Length}")}.");
        }

        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must not be
        /// less than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustNotBeLessThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than or equal to {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ExistingItemException"/> indicating that a collection contains the specified item
        /// that should not be part of it, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ExistingItem<TItem>(IEnumerable<TItem> parameter, TItem item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ExistingItemException(parameterName, message ?? new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must not contain {item.ToStringOrNull()}, but it actually does.").AppendCollectionContent(parameter).ToString());
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must be greater
        /// than the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeGreaterThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "StringException"/> indicating that a string is not trimmed at the end.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotTrimmedAtEnd(string? parameter, string? parameterName, string? message) => throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be trimmed at the end, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a value must be approximately
        /// equal to another value within a specified tolerance, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeApproximately<T>(T parameter, T other, T tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomException(Func<Exception> exceptionFactory) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))();
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "parameter"/> is
        /// passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomException<T>(Func<T, Exception> exceptionFactory, T parameter) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(parameter);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "first"/> and
        /// <paramref name = "second"/> are passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomException<T1, T2>(Func<T1, T2, Exception> exceptionFactory, T1 first, T2 second) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "first"/>,
        /// <paramref name = "second"/>, and <paramref name = "third"/> are passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomException<T1, T2, T3>(Func<T1, T2, T3, Exception> exceptionFactory, T1 first, T2 second, T3 third) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second, third);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "span"/> and
        /// <paramref name = "value"/> are passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomSpanException<TItem, T>(SpanExceptionFactory<TItem, T> exceptionFactory, Span<TItem> span, T value) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory)).Invoke(span, value);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "span"/> is
        /// passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomSpanException<TItem>(ReadOnlySpanExceptionFactory<TItem> exceptionFactory, ReadOnlySpan<TItem> span) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(span);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "span"/> and
        /// <paramref name = "value"/> are passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomSpanException<TItem, T>(ReadOnlySpanExceptionFactory<TItem, T> exceptionFactory, ReadOnlySpan<TItem> span, T value) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(span, value);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "first"/> and
        /// <paramref name = "second"/> are passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomSpanException<TItem>(ReadOnlySpansExceptionFactory<TItem> exceptionFactory, ReadOnlySpan<TItem> first, ReadOnlySpan<TItem> second) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second);
        /// <summary>
        /// Throws the exception that is returned by <paramref name = "exceptionFactory"/>. <paramref name = "first"/>,
        /// <paramref name = "second"/>, and <paramref name = "third"/> are passed to <paramref name = "exceptionFactory"/>.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void CustomSpanException<TItem, T>(ReadOnlySpansExceptionFactory<TItem, T> exceptionFactory, ReadOnlySpan<TItem> first, ReadOnlySpan<TItem> second, T third) => throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second, third);
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a value is not within a specified
        /// range, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeInRange<T>(T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be between {range.CreateRangeDescriptionText("and")}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a value is within a specified
        /// range, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustNotBeInRange<T>(T parameter, Range<T> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be between {range.CreateRangeDescriptionText("and")}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must not be
        /// less than the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustNotBeLessThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must be less
        /// than the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeLessThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must be greater
        /// than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeGreaterThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than or equal to {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidDateTimeException"/> indicating that a date time is not using
        /// <see cref = "DateTimeKind.Utc"/>, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeUtcDateTime(DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Utc}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidDateTimeException"/> indicating that a date time is not using
        /// <see cref = "DateTimeKind.Local"/>, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeLocalDateTime(DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Local}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidDateTimeException"/> indicating that a date time is not using
        /// <see cref = "DateTimeKind.Unspecified"/>, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeUnspecifiedDateTime(DateTime parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidDateTimeException(parameterName, message ?? $"{parameterName ?? "The date time"} must use kind \"{DateTimeKind.Unspecified}\", but it actually uses \"{parameter.Kind}\" and is \"{parameter:O}\".");
        /// <summary>
        /// Throws the default <see cref = "StringException"/> indicating that a string is not equal to "\n" or "\r\n".
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotNewLine(string? parameter, string? parameterName, string? message) => throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be either \"\\n\" or \"\\r\\n\", but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "StringLengthException"/> indicating that a string is not shorter than the given
        /// length, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringNotShorterThan(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be shorter than {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "StringLengthException"/> indicating that a string is not shorter or equal to the
        /// given length, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringNotShorterThanOrEqualTo(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be shorter or equal to {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "StringLengthException"/> indicating that a string has a different length than the
        /// specified one, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringLengthNotEqualTo(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must have length {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "StringLengthException"/> indicating that a string is not longer than the given
        /// length, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringNotLongerThan(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be longer than {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "StringLengthException"/> indicating that a string is not longer than or equal to
        /// the given length, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringNotLongerThanOrEqualTo(string parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must be longer than or equal to {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "StringLengthException"/> indicating that a string's length is not in within the
        /// given range, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringLengthNotInRange(string parameter, Range<int> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringLengthException(parameterName, message ?? $"{parameterName ?? "The string"} must have its length in between {range.CreateRangeDescriptionText("and")}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string is not a substring of another one,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotSubstring(string parameter, string other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must be a substring of \"{other}\", but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string is not a substring of another one,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotSubstring(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must be a substring of \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string is a substring of another one,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void Substring(string parameter, string other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not be a substring of \"{other}\", but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string is a substring of another one,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void Substring(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not be a substring of \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a span has an invalid length,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidSpanLength<T>(ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The read-only span"} must have length {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a span is not longer than the
        /// specified length.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void SpanMustBeLongerThan<T>(ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be longer than {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a span is not longer than and
        /// not equal to the specified length.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void SpanMustBeLongerThanOrEqualTo<T>(ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression(nameof(parameter))] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be longer than or equal to {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a span is not shorter than the
        /// specified length.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void SpanMustBeShorterThan<T>(ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be shorter than {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a span is not shorter than the
        /// specified length.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void SpanMustBeShorterThanOrEqualTo<T>(ReadOnlySpan<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The span"} must be shorter than or equal to {length}, but it actually has length {parameter.Length}.");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a collection has less than a
        /// minimum number of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidMinimumCollectionCount(IEnumerable parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The collection"} must have at least count {count}, but it actually has count {parameter.Count()}.");
        /// <summary>
        /// Throws the default <see cref = "WhiteSpaceStringException"/> indicating that a string contains only white space,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void WhiteSpaceString(string parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new WhiteSpaceStringException(parameterName, message ?? $"{parameterName ?? "The string"} must not contain only white space, but it actually is \"{parameter}\".");
        /// <summary>
        /// Throws an <see cref = "InvalidStateException"/> using the optional message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidState(string? message = null) => throw new InvalidStateException(message);
        /// <summary>
        /// Throws the default <see cref = "RelativeUriException"/> indicating that a URI is relative instead of absolute,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeAbsoluteUri(Uri parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new RelativeUriException(parameterName, message ?? $"{parameterName ?? "The URI"} must be an absolute URI, but it actually is \"{parameter}\".");
        /// <summary>
        /// Throws the default <see cref = "AbsoluteUriException"/> indicating that a URI is absolute instead of relative,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeRelativeUri(Uri parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new AbsoluteUriException(parameterName, message ?? $"{parameterName ?? "The URI"} must be a relative URI, but it actually is \"{parameter}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidUriSchemeException"/> indicating that a URI has an unexpected scheme,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void UriMustHaveScheme(Uri parameter, string scheme, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidUriSchemeException(parameterName, message ?? $"{parameterName ?? "The URI"} must use the scheme \"{scheme}\", but it actually is \"{parameter}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidUriSchemeException"/> indicating that a URI does not use one of a set of
        /// expected schemes, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void UriMustHaveOneSchemeOf(Uri parameter, IEnumerable<string> schemes, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidUriSchemeException(parameterName, message ?? new StringBuilder().AppendLine($"{parameterName ?? "The URI"} must use one of the following schemes").AppendItemsWithNewLine(schemes).AppendLine($"but it actually is \"{parameter}\".").ToString());
        /// <summary>
        /// Throws the default <see cref = "EmptyCollectionException"/> indicating that a collection has no items, using the
        /// optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void EmptyCollection(string? parameterName = null, string? message = null) => throw new EmptyCollectionException(parameterName, message ?? $"{parameterName ?? "The collection"} must not be an empty collection, but it actually is.");
        /// <summary>
        /// Throws the default <see cref = "StringException"/> indicating that a string is not trimmed at the start.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotTrimmedAtStart(string? parameter, string? parameterName, string? message) => throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be trimmed at the start, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws an <see cref = "InvalidOperationException"/> using the optional message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidOperation(string? message = null) => throw new InvalidOperationException(message);
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must be less
        /// than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeLessThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than or equal to {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ValuesNotEqualException"/> indicating that two values are not equal, using the
        /// optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ValuesNotEqual<T>(T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ValuesNotEqualException(parameterName, message ?? $"{parameterName ?? "The value"} must be equal to {other.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "ValuesEqualException"/> indicating that two values are equal, using the optional
        /// parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ValuesEqual<T>(T parameter, T other, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ValuesEqualException(parameterName, message ?? $"{parameterName ?? "The value"} must not be equal to {other.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "EnumValueNotDefinedException"/> indicating that a value is not one of the
        /// constants defined in an enum, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void EnumValueNotDefined<T>(T parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : Enum => throw new EnumValueNotDefinedException(parameterName, message ?? $"{parameterName ?? "The value"} \"{parameter}\" must be one of the defined constants of enum \"{parameter.GetType()}\", but it actually is not.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a value must be less than or approximately
        /// equal to another value within a specified tolerance, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeLessThanOrApproximately<T>(T parameter, T other, T tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be less than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentDefaultException"/> indicating that a value is the default value of its
        /// type, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ArgumentDefault(string? parameterName = null, string? message = null) => throw new ArgumentDefaultException(parameterName, message ?? $"{parameterName ?? "The value"} must not be the default value.");
        /// <summary>
        /// Throws an <see cref = "ArgumentException"/> using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void Argument(string? parameterName = null, string? message = null) => throw new ArgumentException(message ?? $"{parameterName ?? "The value"} is invalid.", parameterName);
        /// <summary>
        /// Throws the default <see cref = "MissingItemException"/> indicating that a collection is not containing the
        /// specified item, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MissingItem<TItem>(IEnumerable<TItem> parameter, TItem item, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new MissingItemException(parameterName, message ?? new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must contain {item.ToStringOrNull()}, but it actually does not.").AppendCollectionContent(parameter).ToString());
        /// <summary>
        /// Throws the default <see cref = "ValueIsOneOfException"/> indicating that a value is one of a specified collection
        /// of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ValueIsOneOf<TItem>(TItem parameter, IEnumerable<TItem> items, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ValueIsOneOfException(parameterName, message ?? new StringBuilder().AppendLine($"{parameterName ?? "The value"} must not be one of the following items").AppendItemsWithNewLine(items).AppendLine($"but it actually is {parameter.ToStringOrNull()}.").ToString());
        /// <summary>
        /// Throws the default <see cref = "SameObjectReferenceException"/> indicating that two references point to the same
        /// object, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void SameObjectReference<T>(T? parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : class => throw new SameObjectReferenceException(parameterName, message ?? $"{parameterName ?? "The reference"} must not point to object \"{parameter}\", but it actually does.");
        /// <summary>
        /// Throws the default <see cref = "EmptyGuidException"/> indicating that a GUID is empty, using the optional
        /// parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void EmptyGuid(string? parameterName = null, string? message = null) => throw new EmptyGuidException(parameterName, message ?? $"{parameterName ?? "The value"} must be a valid GUID, but it actually is an empty one.");
        /// <summary>
        /// Throws the default <see cref = "StringException"/> indicating that a string is not trimmed.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotTrimmed(string? parameter, string? parameterName, string? message) => throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be trimmed, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must not be
        /// greater than or equal to the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustNotBeGreaterThanOrEqualTo<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be greater than or equal to {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "NullableHasNoValueException"/> indicating that a <see cref = "Nullable{T}"/> has
        /// no value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NullableHasNoValue(string? parameterName = null, string? message = null) => throw new NullableHasNoValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have a value, but it actually is null.");
        /// <summary>
        /// Throws an <see cref = "InvalidEmailAddressException"/> using the optional message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidEmailAddress(string parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidEmailAddressException(parameterName, message ?? $"{parameterName ?? "The string"} must be a valid email address, but it actually is \"{parameter}\".");
        /// <summary>
        /// Throws an <see cref = "InvalidEmailAddressException"/> using the optional message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidEmailAddress(ReadOnlySpan<char> parameter, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidEmailAddressException(parameterName, message ?? $"{parameterName ?? "The string"} must be a valid email address, but it actually is \"{parameter.ToString()}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a collection has more than a
        /// maximum number of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidMaximumCollectionCount(IEnumerable parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The collection"} must have at most count {count}, but it actually has count {parameter.Count()}.");
        /// <summary>
        /// Throws the default <see cref = "StringException"/> indicating that a string is not a valid file extension.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotFileExtension(string? parameter, string? parameterName, string? message) => throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be a valid file extension, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "StringException"/> indicating that a string is not a valid file extension.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void NotFileExtension(ReadOnlySpan<char> parameter, string? parameterName, string? message) => throw new StringException(parameterName, message ?? $"{parameterName ?? "The string"} must be a valid file extension, but it actually is {parameter.ToString()}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a value must be greater than or approximately
        /// equal to another value within a specified tolerance, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustBeGreaterThanOrApproximately<T>(T parameter, T other, T tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must be greater than or approximately equal to {other} with a tolerance of {tolerance}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does not end with another one,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringDoesNotEndWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must end with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "TypeCastException"/> indicating that a reference cannot be downcast, using the
        /// optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidTypeCast(object? parameter, Type targetType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new TypeCastException(parameterName, message ?? $"{parameterName ?? "The value"} {parameter.ToStringOrNull()} cannot be cast to \"{targetType}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that a collection has an invalid
        /// number of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidCollectionCount(IEnumerable parameter, int count, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The collection"} must have count {count}, but it actually has count {parameter.Count()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string ends with another one, using the
        /// optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringEndsWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not end with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "StringDoesNotMatchException"/> indicating that a string does not match a regular
        /// expression, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringDoesNotMatch(string parameter, Regex regex, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new StringDoesNotMatchException(parameterName, message ?? $"{parameterName ?? "The string"} must match the regular expression \"{regex}\", but it actually is \"{parameter}\".");
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that an immutable array has an invalid length,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidImmutableArrayLength<T>(ImmutableArray<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            var actualLength = parameter.IsDefault ? 0 : parameter.Length;
            throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The immutable array"} must have length {length}, but it actually has length {actualLength}.");
        }

        /// <summary>
        /// Throws the default <see cref = "ValueIsNotOneOfException"/> indicating that a value is not one of a specified
        /// collection of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ValueNotOneOf<TItem>(TItem parameter, IEnumerable<TItem> items, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ValueIsNotOneOfException(parameterName, message ?? new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be one of the following items").AppendItemsWithNewLine(items).AppendLine($"but it actually is {parameter.ToStringOrNull()}.").ToString());
        /// <summary>
        /// Throws the default <see cref = "InvalidCollectionCountException"/> indicating that an <see cref = "ImmutableArray{T}"/> has more than a
        /// maximum number of items, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void InvalidMaximumImmutableArrayLength<T>(ImmutableArray<T> parameter, int length, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
        {
            throw new InvalidCollectionCountException(parameterName, message ?? $"{parameterName ?? "The immutable array"} must have at most a length of {length}, but it actually {(parameter.IsDefault ? "has no length because it is the default instance" : $"has a length of {parameter.Length}")}.");
        }

        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a value must not be approximately
        /// equal to another value within a specified tolerance, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustNotBeApproximately<T>(T parameter, T other, T tolerance, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be approximately equal to {other} with a tolerance of {tolerance}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does start with another one, using
        /// the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringStartsWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not start with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does start with another one, using
        /// the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringStartsWith(ReadOnlySpan<char> parameter, ReadOnlySpan<char> other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not start with \"{other.ToString()}\" ({comparisonType}), but it actually is {parameter.ToString()}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that a comparable value must not be
        /// greater than the given boundary value, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void MustNotBeGreaterThan<T>(T parameter, T boundary, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null)
            where T : IComparable<T> => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The value"} must not be greater than {boundary}, but it actually is {parameter}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does not contain another string as
        /// a substring, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringDoesNotContain(string parameter, string substring, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must contain {substring.ToStringOrNull()}, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does not contain another string as
        /// a substring, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringDoesNotContain(string parameter, string substring, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must contain {substring.ToStringOrNull()} ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does contain another string as a
        /// substring, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringContains(string parameter, string substring, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not contain {substring.ToStringOrNull()} as a substring, but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does contain another string as a
        /// substring, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringContains(string parameter, string substring, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must not contain {substring.ToStringOrNull()} as a substring ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
        /// <summary>
        /// Throws the default <see cref = "ArgumentOutOfRangeException"/> indicating that an <see cref = "ImmutableArray{T}"/>'s length is not within the
        /// given range, using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void ImmutableArrayLengthNotInRange<T>(ImmutableArray<T> parameter, Range<int> range, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new ArgumentOutOfRangeException(parameterName, message ?? $"{parameterName ?? "The immutable array"} must have its length in between {range.CreateRangeDescriptionText("and")}, but it actually {(parameter.IsDefault ? "has no length because it is the default instance" : $"has length {parameter.Length}")}.");
        /// <summary>
        /// Throws the default <see cref = "SubstringException"/> indicating that a string does not start with another one,
        /// using the optional parameter name and message.
        /// </summary>
        [ContractAnnotation("=> halt")]
        [DoesNotReturn]
        public static void StringDoesNotStartWith(string parameter, string other, StringComparison comparisonType, [CallerArgumentExpression("parameter")] string? parameterName = null, string? message = null) => throw new SubstringException(parameterName, message ?? $"{parameterName ?? "The string"} must start with \"{other}\" ({comparisonType}), but it actually is {parameter.ToStringOrNull()}.");
    }

    /// <summary>
    /// Represents a delegate that receives a span and a value as parameters and that produces an exception.
    /// </summary>
    internal delegate Exception SpanExceptionFactory<TItem, in T>(Span<TItem> span, T value);
    /// <summary>
    /// Represents a delegate that receives a read-only span and produces an exception.
    /// </summary>
    internal delegate Exception ReadOnlySpanExceptionFactory<TItem>(ReadOnlySpan<TItem> span);
    /// <summary>
    /// Represents a delegate that receives a read-only span and a value as parameters and that produces an exception.
    /// </summary>
    internal delegate Exception ReadOnlySpanExceptionFactory<TItem, in T>(ReadOnlySpan<TItem> span, T value);
    /// <summary>
    /// Represents a delegate that receives two spans and produces an exception.
    /// </summary>
    internal delegate Exception ReadOnlySpansExceptionFactory<TItem>(ReadOnlySpan<TItem> span1, ReadOnlySpan<TItem> span2);
    /// <summary>
    /// Represents a delegate that receives two spans and a value as parameters and that produces an exception.
    /// </summary>
    internal delegate Exception ReadOnlySpansExceptionFactory<TItem, in T>(ReadOnlySpan<TItem> span1, ReadOnlySpan<TItem> span2, T value);
}

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    /// Represents a builder for the <see cref = "MultiplyAddHash"/> algorithm that does not allocate.
    /// Should only be used in cases where the overload for sixteen values is not enough or a dedicated
    /// initial hash must be provided (e.g. for test reasons).
    /// Instantiate the builder with the <see cref = "Create"/> method. You have to instantiate a new builder
    /// for each hash code that you want to calculate.
    /// </summary>
    internal struct MultiplyAddHashBuilder
    {
        private int _hash;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MultiplyAddHashBuilder(int initialHash) => _hash = initialHash;
        /// <summary>
        /// Combines the given value into the hash using the <see cref = "MultiplyAddHash.CombineIntoHash{T}(ref int, T)"/> method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MultiplyAddHashBuilder CombineIntoHash<T>(T value)
        {
            MultiplyAddHash.CombineIntoHash(ref _hash, value);
            return this;
        }

        /// <summary>
        /// Returns the calculated hash code.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BuildHash() => _hash;
        /// <summary>
        /// Initializes a new instance of <see cref = "MultiplyAddHashBuilder"/> with the specified initial hash.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MultiplyAddHashBuilder Create(int initialHash = MultiplyAddHash.FirstPrime) => new(initialHash);
    }

    /// <summary>
    /// Provides extension methods for <see cref = "Expression"/> instances.
    /// </summary>
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Extracts the <see cref = "PropertyInfo"/> from an expression of the shape "object => object.Property".
        /// </summary>
        /// <typeparam name = "T">The object type.</typeparam>
        /// <typeparam name = "TProperty">The type of the property.</typeparam>
        /// <param name = "expression">The expression where the property info will be extracted from.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "expression"/> is null.</exception>
        /// <exception cref = "ArgumentException">
        /// Throw when the <paramref name = "expression"/> is not of the shape "object => object.Property".
        /// </exception>
        public static PropertyInfo ExtractProperty<T, TProperty>(// ReSharper disable once RedundantNullableFlowAttribute - NotNull is not redundant, see Issue72NotNullAttributeTests
        [NotNull][ValidatedNotNull] this Expression<Func<T, TProperty>> expression)
        {
            expression.MustNotBeNull(nameof(expression));
            var memberExpression = expression.Body as MemberExpression;
            if (!(memberExpression?.Member is PropertyInfo propertyInfo))
            {
                throw new ArgumentException("The specified expression is not valid. Please use an expression like the following one: o => o.Property", nameof(expression));
            }

            return propertyInfo;
        }

        /// <summary>
        /// Extracts the <see cref = "FieldInfo"/> from an expression of the shape "object => object.Field".
        /// </summary>
        /// <typeparam name = "T">The object type.</typeparam>
        /// <typeparam name = "TField">The type of the field.</typeparam>
        /// <param name = "expression">The expression where the field info will be extracted from.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "expression"/> is null.</exception>
        /// <exception cref = "ArgumentException">
        /// Throw when the <paramref name = "expression"/> is not of the shape "object => object.Field".
        /// </exception>
        public static FieldInfo ExtractField<T, TField>(// ReSharper disable once RedundantNullableFlowAttribute - NotNull is not redundant, see Issue72NotNullAttributeTests
        [NotNull][ValidatedNotNull] this Expression<Func<T, TField>> expression)
        {
            expression.MustNotBeNull(nameof(expression));
            var memberExpression = expression.Body as MemberExpression;
            if (!(memberExpression?.Member is FieldInfo fieldInfo))
            {
                throw new ArgumentException("The specified expression is not valid. Please use an expression like the following one: o => o.Field", nameof(expression));
            }

            return fieldInfo;
        }
    }

    /// <summary>
    /// The <see cref = "MultiplyAddHash"/> class represents a simple non-cryptographic hash function that uses a prime number
    /// as seed and then manipulates this value by constantly performing <c>hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);</c>
    /// for each given value. It is implemented according to the guidelines of Jon Skeet as stated in this Stack Overflow
    /// answer: http://stackoverflow.com/a/263416/1560623. IMPORTANT: do not persist any hash codes and rely on them
    /// to stay the same. Hash codes should only be used in memory within a single process session, usually for the use
    /// in dictionaries (hash tables) and sets. This algorithm, especially the prime numbers can change even during minor
    /// releases of Light.GuardClauses.
    /// </summary>
    internal static class MultiplyAddHash
    {
        /// <summary>
        /// This prime number is used as an initial (seed) value when calculating hash codes. Its value is 1322837333.
        /// </summary>
        public const int FirstPrime = 1322837333;
        /// <summary>
        /// The second prime number (397) used for hash code generation. It is applied using the following statement:
        /// <c>hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);</c>.
        /// It is the same value that ReSharper (2018.1) uses for hash code generation.
        /// </summary>
        public const int SecondPrime = 397;
        /// <summary>
        /// Creates a hash code from the two specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2>(T1 value1, T2 value2)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the three specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the four specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the five specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the six specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the seven specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the eight specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the nine specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the ten specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the eleven specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the eleven specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the thirteen specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the fourteen specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            CombineIntoHash(ref hash, value14);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the fifteen specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            CombineIntoHash(ref hash, value14);
            CombineIntoHash(ref hash, value15);
            return hash;
        }

        /// <summary>
        /// Creates a hash code from the sixteen specified values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8, T9 value9, T10 value10, T11 value11, T12 value12, T13 value13, T14 value14, T15 value15, T16 value16)
        {
            var hash = FirstPrime;
            CombineIntoHash(ref hash, value1);
            CombineIntoHash(ref hash, value2);
            CombineIntoHash(ref hash, value3);
            CombineIntoHash(ref hash, value4);
            CombineIntoHash(ref hash, value5);
            CombineIntoHash(ref hash, value6);
            CombineIntoHash(ref hash, value7);
            CombineIntoHash(ref hash, value8);
            CombineIntoHash(ref hash, value9);
            CombineIntoHash(ref hash, value10);
            CombineIntoHash(ref hash, value11);
            CombineIntoHash(ref hash, value12);
            CombineIntoHash(ref hash, value13);
            CombineIntoHash(ref hash, value14);
            CombineIntoHash(ref hash, value15);
            CombineIntoHash(ref hash, value16);
            return hash;
        }

        /// <summary>
        /// Mutates the given hash with the specified value using the following statement:
        /// <c>hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);</c>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CombineIntoHash<T>(ref int hash, T value) => hash = unchecked(hash * SecondPrime + value?.GetHashCode() ?? 0);
    }

    /// <summary>
    /// Provides extension methods for the <see cref = "IEnumerable{T}"/> interface.
    /// </summary>
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Tries to cast the specified enumerable to an <see cref = "IList{T}"/>, or
        /// creates a new <see cref = "List{T}"/> containing the enumerable items.
        /// </summary>
        /// <typeparam name = "T">The item type of the enumerable.</typeparam>
        /// <param name = "source">The enumerable to be transformed.</param>
        /// <returns>The list containing the items of the enumerable.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "source"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("source:null => halt; source:notnull => notnull")]
        // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static IList<T> AsList<T>([NotNull][ValidatedNotNull] this IEnumerable<T> source) => source as IList<T> ?? source.ToList();
        /// <summary>
        /// Tries to cast the specified enumerable to an <see cref = "IList{T}"/>, or
        /// creates a new collection containing the enumerable items by calling the specified delegate.
        /// </summary>
        /// <typeparam name = "T">The item type of the collection.</typeparam>
        /// <param name = "source">The enumerable that will be converted to <see cref = "IList{T}"/>.</param>
        /// <param name = "createCollection">The delegate that creates the collection containing the specified items.</param>
        /// <returns>The cast enumerable, or a new collection containing the enumerable items.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "source"/> or <paramref name = "createCollection"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("source:null => halt; source:notnull => notnull; createCollection:null => halt")]
        public static IList<T> AsList<T>(// ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        [NotNull][ValidatedNotNull] this IEnumerable<T> source, Func<IEnumerable<T>, IList<T>> createCollection) => source as IList<T> ?? createCollection.MustNotBeNull(nameof(createCollection))(source.MustNotBeNull(nameof(source)));
        /// <summary>
        /// Tries to downcast the specified enumerable to an array, or creates a new array with the specified items.
        /// </summary>
        /// <typeparam name = "T">The item type of the collection.</typeparam>
        /// <param name = "source">The enumerable that will be converted to an array.</param>
        /// <returns>The cast array, or a new array containing the enumerable items.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "source"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("source:null => halt; source:notnull => notnull")]
        // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static T[] AsArray<T>([NotNull][ValidatedNotNull] this IEnumerable<T> source) => source as T[] ?? source.ToArray();
        /// <summary>
        /// Performs the action on each item of the specified enumerable. If the enumerable contains items that are null, this
        /// method can either throw an exception or ignore the value (your delegate will not be called in this case).
        /// </summary>
        /// <typeparam name = "T">The item type of the enumerable.</typeparam>
        /// <param name = "enumerable">The collection containing the items that will be passed to the action.</param>
        /// <param name = "action">The action that executes for each item of the collection.</param>
        /// <param name = "throwWhenItemIsNull">The value indicating whether this method should throw a <see cref = "CollectionException"/> when any of the items is null (optional). Defaults to true.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> or <paramref name = "action"/> is null.</exception>
        /// <exception cref = "CollectionException">Thrown when <paramref name = "enumerable"/> contains a value that is null and <paramref name = "throwWhenItemIsNull"/> is set to true.</exception>
        public static IEnumerable<T> ForEach<T>(// ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        [NotNull][ValidatedNotNull] this IEnumerable<T> enumerable, Action<T> action, bool throwWhenItemIsNull = true)
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
                        if (throwWhenItemIsNull)
                        {
                            throw new CollectionException(nameof(enumerable), $"The collection contains null at index {i}.");
                        }

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
                        if (throwWhenItemIsNull)
                        {
                            throw new CollectionException(nameof(enumerable), $"The collection contains null at index {i}.");
                        }

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
        /// Tries to cast the specified enumerable as an <see cref = "IReadOnlyList{T}"/>, or
        /// creates a new <see cref = "List{T}"/> containing the enumerable items.
        /// </summary>
        /// <typeparam name = "T">The item type of the enumerable.</typeparam>
        /// <param name = "source">The enumerable to be transformed.</param>
        /// <returns>The list containing the items of the enumerable.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "source"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("source:null => halt; source:notnull => notnull")]
        // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static IReadOnlyList<T> AsReadOnlyList<T>([NotNull][ValidatedNotNull] this IEnumerable<T> source) => source as IReadOnlyList<T> ?? source.ToList();
        /// <summary>
        /// Tries to cast the specified enumerable as an <see cref = "IReadOnlyList{T}"/>, or
        /// creates a new collection containing the enumerable items by calling the specified delegate.
        /// </summary>
        /// <typeparam name = "T">The item type of the collection.</typeparam>
        /// <param name = "source">The enumerable that will be converted to <see cref = "IReadOnlyList{T}"/>.</param>
        /// <param name = "createCollection">The delegate that creates the collection containing the specified items.</param>
        /// <returns>The cast enumerable, or a new collection containing the enumerable items.</returns>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "source"/> or <paramref name = "createCollection"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("source:null => halt; source:notnull => notnull; createCollection:null => halt")]
        // ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static IReadOnlyList<T> AsReadOnlyList<T>([NotNull][ValidatedNotNull] this IEnumerable<T> source, [NotNull][ValidatedNotNull] Func<IEnumerable<T>, IReadOnlyList<T>> createCollection) => source as IReadOnlyList<T> ?? createCollection.MustNotBeNull(nameof(createCollection))(source.MustNotBeNull(nameof(source)));
        // ReSharper restore RedundantNullableFlowAttribute
        /// <summary>
        /// Gets the count of the specified enumerable.
        /// </summary>
        /// <param name = "enumerable">The enumerable whose count should be determined.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("enumerable:null => halt")]
        // ReSharper disable once RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static int Count([NotNull][ValidatedNotNull] this IEnumerable enumerable)
        {
            if (enumerable is ICollection collection)
            {
                return collection.Count;
            }

            if (enumerable is string @string)
            {
                return @string.Length;
            }

            return DetermineCountViaEnumerating(enumerable);
        }

        /// <summary>
        /// Gets the count of the specified enumerable.
        /// </summary>
        /// <param name = "enumerable">The enumerable whose count should be determined.</param>
        /// <param name = "parameterName">The name of the parameter that is passed to the <see cref = "ArgumentNullException"/> (optional).</param>
        /// <param name = "message">The message that is passed to the <see cref = "ArgumentNullException"/> (optional).</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("enumerable:null => halt")]
        public static int Count([NotNull][ValidatedNotNull] this IEnumerable? enumerable, string? parameterName, string? message)
        {
            if (enumerable is ICollection collection)
            {
                return collection.Count;
            }

            if (enumerable is string @string)
            {
                return @string.Length;
            }

            return DetermineCountViaEnumerating(enumerable, parameterName, message);
        }

        /// <summary>
        /// Gets the count of the specified enumerable.
        /// </summary>
        /// <param name = "enumerable">The enumerable whose count should be determined.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("enumerable:null => halt")]
        // ReSharper disable RedundantNullableFlowAttribute -- NotNull has an effect, see Issue72NotNullAttributeTests
        public static int GetCount<T>([NotNull][ValidatedNotNull] this IEnumerable<T> enumerable)
        {
            if (enumerable is ICollection collection)
            {
                return collection.Count;
            }

            if (enumerable is string @string)
            {
                return @string.Length;
            }

            if (TryGetCollectionOfTCount(enumerable, out var count))
            {
                return count;
            }

            return DetermineCountViaEnumerating(enumerable);
        }

        /// <summary>
        /// Gets the count of the specified enumerable.
        /// </summary>
        /// <param name = "enumerable">The enumerable whose count should be determined.</param>
        /// <param name = "parameterName">The name of the parameter that is passed to the <see cref = "ArgumentNullException"/> (optional).</param>
        /// <param name = "message">The message that is passed to the <see cref = "ArgumentNullException"/> (optional).</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "enumerable"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("enumerable:null => halt")]
        public static int GetCount<T>([NotNull][ValidatedNotNull] this IEnumerable<T> enumerable, string? parameterName, string? message = null)
        {
            if (enumerable is ICollection collection)
            {
                return collection.Count;
            }

            if (enumerable is string @string)
            {
                return @string.Length;
            }

            if (TryGetCollectionOfTCount(enumerable, out var count))
            {
                return count;
            }

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
            {
                count++;
            }

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
            {
                count++;
            }

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
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Provides extension methods for <see cref = "string "/> and <see cref = "StringBuilder"/> to easily assembly error messages.
    /// </summary>
    internal static class TextExtensions
    {
        /// <summary>
        /// Gets the default NewLineSeparator. This value is $",{Environment.NewLine}".
        /// </summary>
        public static readonly string DefaultNewLineSeparator = ',' + Environment.NewLine;
        /// <summary>
        /// Gets the list of types that will not be surrounded by quotation marks in error messages.
        /// </summary>
        public static readonly ReadOnlyCollection<Type> UnquotedTypes = new([typeof(int), typeof(long), typeof(short), typeof(sbyte), typeof(uint), typeof(ulong), typeof(ushort), typeof(byte), typeof(bool), typeof(double), typeof(decimal), typeof(float), ]);
        private static bool IsUnquotedType<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return true;
            }

            if (typeof(T) == typeof(long))
            {
                return true;
            }

            if (typeof(T) == typeof(short))
            {
                return true;
            }

            if (typeof(T) == typeof(sbyte))
            {
                return true;
            }

            if (typeof(T) == typeof(uint))
            {
                return true;
            }

            if (typeof(T) == typeof(ulong))
            {
                return true;
            }

            if (typeof(T) == typeof(ushort))
            {
                return true;
            }

            if (typeof(T) == typeof(byte))
            {
                return true;
            }

            if (typeof(T) == typeof(bool))
            {
                return true;
            }

            if (typeof(T) == typeof(double))
            {
                return true;
            }

            if (typeof(T) == typeof(decimal))
            {
                return true;
            }

            if (typeof(T) == typeof(float))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the string representation of <paramref name = "value"/>, or <paramref name = "nullText"/> if <paramref name = "value"/> is null.
        /// If the type of <paramref name = "value"/> is not one of <see cref = "UnquotedTypes"/>, then quotation marks will be put around the string representation.
        /// </summary>
        /// <param name = "value">The item whose string representation should be returned.</param>
        /// <param name = "nullText">The text that is returned when <paramref name = "value"/> is null (defaults to "null").</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("=> notnull")]
        public static string ToStringOrNull<T>(this T value, string nullText = "null") => value?.ToStringRepresentation() ?? nullText;
        /// <summary>
        /// Returns the string representation of <paramref name = "value"/>. This is done by calling <see cref = "object.ToString"/>. If the type of
        /// <paramref name = "value"/> is not one of <see cref = "UnquotedTypes"/>, then the resulting string will be wrapped in quotation marks.
        /// </summary>
        /// <param name = "value">The value whose string representation is requested.</param>
        [ContractAnnotation("value:null => halt; value:notnull => notnull")]
        public static string? ToStringRepresentation<T>([NotNull][ValidatedNotNull] this T value)
        {
            value.MustNotBeNullReference(nameof(value));
            var content = value.ToString();
            if (IsUnquotedType<T>() || content.IsNullOrEmpty())
            {
                return content;
            }

            // ReSharper disable UseIndexFromEndExpression -- not possible in netstandard2.0
            if (content.Length <= 126)
            {
                Span<char> span = stackalloc char[content.Length + 2];
                span[0] = span[span.Length - 1] = '"';
                content.AsSpan().CopyTo(span.Slice(1, content.Length));
                return span.ToString();
            }

            var contentWithQuotationMarks = new char[content.Length + 2];
            contentWithQuotationMarks[0] = contentWithQuotationMarks[contentWithQuotationMarks.Length - 1] = '"';
            // ReSharper restore UseIndexFromEndExpression
            content.CopyTo(0, contentWithQuotationMarks, 1, content.Length);
            return new string (contentWithQuotationMarks);
        }

        /// <summary>
        /// Appends the content of the collection with the specified header line to the string builder.
        /// Each item is on a new line.
        /// </summary>
        /// <typeparam name = "T">The item type of the collection.</typeparam>
        /// <param name = "stringBuilder">The string builder that the content is appended to.</param>
        /// <param name = "items">The collection whose items will be appended to the string builder.</param>
        /// <param name = "headerLine">The string that will be placed before the actual items as a header.</param>
        /// <param name = "finishWithNewLine">The value indicating if a new line is added after the last item. This value defaults to true.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "stringBuilder"/> or <paramref name = "items"/>is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("stringBuilder:null => halt; items:null => halt; stringBuilder:notnull => notnull")]
        // ReSharper disable RedundantNullableFlowAttribute
        public static StringBuilder AppendCollectionContent<T>([NotNull][ValidatedNotNull] this StringBuilder stringBuilder, [NotNull][ValidatedNotNull] IEnumerable<T> items, string headerLine = "Content of the collection:", bool finishWithNewLine = true) => stringBuilder.MustNotBeNull(nameof(stringBuilder)).AppendLine(headerLine).AppendItemsWithNewLine(items, finishWithNewLine: finishWithNewLine);
        // ReSharper restore RedundantNullableFlowAttribute
        /// <summary>
        /// Appends the string representations of the specified items to the string builder.
        /// </summary>
        /// <param name = "stringBuilder">The string builder where the items will be appended to.</param>
        /// <param name = "items">The items to be appended.</param>
        /// <param name = "itemSeparator">The characters used to separate the items. Defaults to ", " and is not appended after the last item.</param>
        /// <param name = "emptyCollectionText">The text that is appended to the string builder when <paramref name = "items"/> is empty. Defaults to "empty collection".</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "stringBuilder"/> or <paramref name = "items"/> is null.</exception>
        [ContractAnnotation("stringBuilder:null => halt; items:null => halt; stringBuilder:notnull => notnull")]
        // ReSharper disable RedundantNullableFlowAttribute
        public static StringBuilder AppendItems<T>([NotNull][ValidatedNotNull] this StringBuilder stringBuilder, [NotNull][ValidatedNotNull] IEnumerable<T> items, string itemSeparator = ", ", string emptyCollectionText = "empty collection")
        // ReSharper restore RedundantNullableFlowAttribute
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            var list = items.MustNotBeNull(nameof(items)).AsList();
            var currentIndex = 0;
            var itemsCount = list.Count;
            if (itemsCount == 0)
            {
                return stringBuilder.Append(emptyCollectionText);
            }

            while (true)
            {
                stringBuilder.Append(list[currentIndex].ToStringOrNull());
                if (currentIndex < itemsCount - 1)
                {
                    stringBuilder.Append(itemSeparator);
                }
                else
                {
                    return stringBuilder;
                }

                ++currentIndex;
            }
        }

        /// <summary>
        /// Appends the string representations of the specified items to the string builder. Each item is on its own line.
        /// </summary>
        /// <param name = "stringBuilder">The string builder where the items will be appended to.</param>
        /// <param name = "items">The items to be appended.</param>
        /// <param name = "emptyCollectionText">The text that is appended to the string builder when <paramref name = "items"/> is empty. Defaults to "empty collection".</param>
        /// <param name = "finishWithNewLine">The value indicating if a new line is added after the last item. This value defaults to true.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "stringBuilder"/> or <paramref name = "items"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("stringBuilder:null => halt; items:null => halt; stringBuilder:notnull => notnull")]
        // ReSharper disable RedundantNullableFlowAttribute
        public static StringBuilder AppendItemsWithNewLine<T>([NotNull][ValidatedNotNull] this StringBuilder stringBuilder, [NotNull][ValidatedNotNull] IEnumerable<T> items, string emptyCollectionText = "empty collection", bool finishWithNewLine = true) => stringBuilder.AppendItems(items, DefaultNewLineSeparator, emptyCollectionText).AppendLineIf(finishWithNewLine);
        // ReSharper restore RedundantNullableFlowAttribute
        /// <summary>
        /// Appends the value to the specified string builder if the condition is true.
        /// </summary>
        /// <param name = "stringBuilder">The string builder where <paramref name = "value"/> will be appended to.</param>
        /// <param name = "condition">The boolean value indicating whether the append operation will be performed or not.</param>
        /// <param name = "value">The value to be appended to the string builder.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "stringBuilder"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("stringBuilder:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendIf(// ReSharper disable once RedundantNullableFlowAttribute
        [NotNull][ValidatedNotNull] this StringBuilder stringBuilder, bool condition, string value)
        {
            if (condition)
            {
                stringBuilder.MustNotBeNull(nameof(stringBuilder)).Append(value);
            }

            return stringBuilder;
        }

        /// <summary>
        /// Appends the value followed by a new line separator to the specified string builder if the condition is true.
        /// </summary>
        /// <param name = "stringBuilder">The string builder where <paramref name = "value"/> will be appended to.</param>
        /// <param name = "condition">The boolean value indicating whether the append operation will be performed or not.</param>
        /// <param name = "value">The value to be appended to the string builder (optional). This value defaults to an empty string.</param>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "stringBuilder"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ContractAnnotation("stringBuilder:null => halt; stringBuilder:notnull => notnull")]
        public static StringBuilder AppendLineIf(// ReSharper disable once RedundantNullableFlowAttribute
        [NotNull][ValidatedNotNull] this StringBuilder stringBuilder, bool condition, string value = "")
        {
            if (condition)
            {
                stringBuilder.MustNotBeNull(nameof(stringBuilder)).AppendLine(value);
            }

            return stringBuilder;
        }

        /// <summary>
        /// Appends the messages of the <paramref name = "exception"/> and its nested exceptions to the
        /// specified <paramref name = "stringBuilder"/>.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown when any parameter is null.</exception>
         // ReSharper disable RedundantNullableFlowAttribute
        public static StringBuilder AppendExceptionMessages([NotNull][ValidatedNotNull] this StringBuilder stringBuilder, [NotNull][ValidatedNotNull] Exception exception)
        // ReSharper restore RedundantNullableFlowAttribute
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            exception.MustNotBeNull(nameof(exception));
            while (true)
            {
                // ReSharper disable once PossibleNullReferenceException
                stringBuilder.AppendLine(exception.Message);
                if (exception.InnerException is null)
                {
                    return stringBuilder;
                }

                stringBuilder.AppendLine();
                exception = exception.InnerException;
            }
        }

        /// <summary>
        /// Formats all messages of the <paramref name = "exception"/> and its nested exceptions into
        /// a single string.
        /// </summary>
        /// <exception cref = "ArgumentNullException">Thrown when <paramref name = "exception"/> is null.</exception>
         // ReSharper disable once RedundantNullableFlowAttribute
        public static string GetAllExceptionMessages([NotNull][ValidatedNotNull] this Exception exception) => new StringBuilder().AppendExceptionMessages(exception).ToString();
        /// <summary>
        /// Checks if the two strings are equal using ordinal sorting rules as well as ignoring the white space
        /// of the provided strings.
        /// </summary>
        public static bool EqualsOrdinalIgnoreWhiteSpace(this string? x, string? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (x.Length == 0)
            {
                return y.Length == 0;
            }

            var indexX = 0;
            var indexY = 0;
            bool wasXSuccessful;
            bool wasYSuccessful;
            // This condition of the while loop actually has to use the single '&' operator because
            // y.TryAdvanceToNextNonWhiteSpaceCharacter must be called even though it already returned
            // false on x. Otherwise, the 'wasXSuccessful == wasYSuccessful' comparison would not return
            // the desired result.
            while ((wasXSuccessful = x.TryAdvanceToNextNonWhiteSpaceCharacter(ref indexX)) & (wasYSuccessful = y.TryAdvanceToNextNonWhiteSpaceCharacter(ref indexY)))
            {
                if (x[indexX++] != y[indexY++])
                {
                    return false;
                }
            }

            return wasXSuccessful == wasYSuccessful;
        }

        /// <summary>
        /// Checks if the two strings are equal using ordinal sorting rules, ignoring the case of the letters
        /// as well as ignoring the white space of the provided strings.
        /// </summary>
        public static bool EqualsOrdinalIgnoreCaseIgnoreWhiteSpace(this string? x, string? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null || y is null)
            {
                return false;
            }

            if (x.Length == 0)
            {
                return y.Length == 0;
            }

            var indexX = 0;
            var indexY = 0;
            bool wasXSuccessful;
            bool wasYSuccessful;
            // This condition of the while loop actually has to use the single '&' operator because
            // y.TryAdvanceToNextNonWhiteSpaceCharacter must be called even though it already returned
            // false on x. Otherwise, the 'wasXSuccessful == wasYSuccessful' comparison would not return
            // the desired result.
            while ((wasXSuccessful = x.TryAdvanceToNextNonWhiteSpaceCharacter(ref indexX)) & (wasYSuccessful = y.TryAdvanceToNextNonWhiteSpaceCharacter(ref indexY)))
            {
                if (char.ToLowerInvariant(x[indexX++]) != char.ToLowerInvariant(y[indexY++]))
                {
                    return false;
                }
            }

            return wasXSuccessful == wasYSuccessful;
        }

        private static bool TryAdvanceToNextNonWhiteSpaceCharacter(this string @string, ref int currentIndex)
        {
            while (currentIndex < @string.Length)
            {
                if (!char.IsWhiteSpace(@string[currentIndex]))
                {
                    return true;
                }

                ++currentIndex;
            }

            return false;
        }
    }
}

/* 
License information for JetBrains.Annotations

MIT License
Copyright (c) 2016 JetBrains http://www.jetbrains.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */
namespace JetBrains.Annotations
{
    /// <summary>
    /// Indicates that the value of the marked element can never be <c>null</c>.
    /// </summary>
    /// <example><code>
    /// [NotNull] object Foo() {
    ///   return null; // Warning: Possible 'null' assignment
    /// }
    /// </code></example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    internal sealed class NotNullAttribute : Attribute
    {
    }

    /// <summary>
    /// Describes dependence between method input and output.
    /// </summary>
    /// <syntax>
    /// <p>Function Definition Table syntax:</p>
    /// <list>
    /// <item>FDT      ::= FDTRow [;FDTRow]*</item>
    /// <item>FDTRow   ::= Input =&gt; Output | Output &lt;= Input</item>
    /// <item>Input    ::= ParameterName: Value [, Input]*</item>
    /// <item>Output   ::= [ParameterName: Value]* {halt|stop|void|nothing|Value}</item>
    /// <item>Value    ::= true | false | null | notnull | canbenull</item>
    /// </list>
    /// If the method has a single input parameter, its name could be omitted.<br/>
    /// Using <c>halt</c> (or <c>void</c>/<c>nothing</c>, which is the same) for the method output
    /// means that the method doesn't return normally (throws or terminates the process).<br/>
    /// Value <c>canbenull</c> is only applicable for output parameters.<br/>
    /// You can use multiple <c>[ContractAnnotation]</c> for each FDT row, or use single attribute
    /// with rows separated by the semicolon. There is no notion of order rows, all rows are checked
    /// for applicability and applied per each program state tracked by the analysis engine.<br/>
    /// </syntax>
    /// <examples><list>
    /// <item><code>
    /// [ContractAnnotation("=&gt; halt")]
    /// public void TerminationMethod()
    /// </code></item>
    /// <item><code>
    /// [ContractAnnotation("null &lt;= param:null")] // reverse condition syntax
    /// public string GetName(string surname)
    /// </code></item>
    /// <item><code>
    /// [ContractAnnotation("s:null =&gt; true")]
    /// public bool IsNullOrEmpty(string s) // string.IsNullOrEmpty()
    /// </code></item>
    /// <item><code>
    /// // A method that returns null if the parameter is null,
    /// // and not null if the parameter is not null
    /// [ContractAnnotation("null =&gt; null; notnull =&gt; notnull")]
    /// public object Transform(object data)
    /// </code></item>
    /// <item><code>
    /// [ContractAnnotation("=&gt; true, result: notnull; =&gt; false, result: null")]
    /// public bool TryParse(string s, out Person result)
    /// </code></item>
    /// </list></examples>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class ContractAnnotationAttribute : Attribute
    {
        public ContractAnnotationAttribute([NotNull] string contract) : this(contract, false)
        {
        }

        public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
        {
            Contract = contract;
            ForceFullStates = forceFullStates;
        }

        [NotNull]
        public string Contract { get; }
        public bool ForceFullStates { get; }
    }

    /// <summary>
    /// Indicates that IEnumerable passed as a parameter is not enumerated.
    /// Use this annotation to suppress the 'Possible multiple enumeration of IEnumerable' inspection.
    /// </summary>
    /// <example><code>
    /// static void ThrowIfNull&lt;T&gt;([NoEnumeration] T v, string n) where T : class
    /// {
    ///   // custom check for null but no enumeration
    /// }
    ///
    /// void Foo(IEnumerable&lt;string&gt; values)
    /// {
    ///   ThrowIfNull(values, nameof(values));
    ///   var x = values.ToList(); // No warnings about multiple enumeration
    /// }
    /// </code></example>
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class NoEnumerationAttribute : Attribute
    {
    }
}

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that <see langword="null"/> is allowed as an input even if the
    /// corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class AllowNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "AllowNullAttribute"/> class.
        /// </summary>
        public AllowNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that <see langword="null"/> is disallowed as an input even if the
    /// corresponding type allows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false)]
    internal sealed class DisallowNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "DisallowNullAttribute"/> class.
        /// </summary>
        public DisallowNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that a method that will never return under any circumstance.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    internal sealed class DoesNotReturnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "DoesNotReturnAttribute"/> class.
        /// </summary>
        public DoesNotReturnAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that the method will not return if the associated <see cref = "Boolean"/>
    /// parameter is passed the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class DoesNotReturnIfAttribute : Attribute
    {
        /// <summary>
        /// Gets the condition parameter value.
        /// Code after the method is considered unreachable by diagnostics if the argument
        /// to the associated parameter matches this value.
        /// </summary>
        public bool ParameterValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DoesNotReturnIfAttribute"/>
        /// class with the specified parameter value.
        /// </summary>
        /// <param name = "parameterValue">
        /// The condition parameter value.
        /// Code after the method is considered unreachable by diagnostics if the argument
        /// to the associated parameter matches this value.
        /// </param>
        public DoesNotReturnIfAttribute(bool parameterValue)
        {
            ParameterValue = parameterValue;
        }
    }

    /// <summary>
    /// Specifies that an output may be <see langword="null"/> even if the
    /// corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class MaybeNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "MaybeNullAttribute"/> class.
        /// </summary>
        public MaybeNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that when a method returns <see cref = "ReturnValue"/>, 
    /// the parameter may be <see langword="null"/> even if the corresponding type disallows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class MaybeNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Gets the return value condition.
        /// If the method returns this value, the associated parameter may be <see langword="null"/>.
        /// </summary>
        public bool ReturnValue { get; }

        /// <summary>
        /// Initializes the attribute with the specified return value condition.
        /// </summary>
        /// <param name = "returnValue">
        /// The return value condition.
        /// If the method returns this value, the associated parameter may be <see langword="null"/>.
        /// </param>
        public MaybeNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }

    /// <summary>
    /// Specifies that an output is not <see langword="null"/> even if the
    /// corresponding type allows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false)]
    internal sealed class NotNullAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "NotNullAttribute"/> class.
        /// </summary>
        public NotNullAttribute()
        {
        }
    }

    /// <summary>
    /// Specifies that the output will be non-<see langword="null"/> if the
    /// named parameter is non-<see langword="null"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        /// <summary>
        /// Gets the associated parameter name.
        /// The output will be non-<see langword="null"/> if the argument to the
        /// parameter specified is non-<see langword="null"/>.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes the attribute with the associated parameter name.
        /// </summary>
        /// <param name = "parameterName">
        /// The associated parameter name.
        /// The output will be non-<see langword="null"/> if the argument to the
        /// parameter specified is non-<see langword="null"/>.
        /// </param>
        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }
    }

    /// <summary>
    /// Specifies that when a method returns <see cref = "ReturnValue"/>,
    /// the parameter will not be <see langword="null"/> even if the corresponding type allows it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class NotNullWhenAttribute : Attribute
    {
        /// <summary>
        /// Gets the return value condition.
        /// If the method returns this value, the associated parameter will not be <see langword="null"/>.
        /// </summary>
        public bool ReturnValue { get; }

        /// <summary>
        /// Initializes the attribute with the specified return value condition.
        /// </summary>
        /// <param name = "returnValue">
        /// The return value condition.
        /// If the method returns this value, the associated parameter will not be <see langword="null"/>.
        /// </param>
        public NotNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}