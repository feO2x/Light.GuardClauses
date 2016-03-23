using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    ///     The StringBuilderExtensions class contains an extension method that encapsulates the adding of items from a
    ///     collection or dictionary to a string builder.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        ///     Gets the default NewLineSeparator for <see cref="AppendItems{T}" /> and <see cref="AppendKeyValuePairs{TKey,TValue}" />. This value is ",{Environment.NewLine}".
        /// </summary>
        public static readonly string DefaultNewLineSeparator = ',' + Environment.NewLine;

        private static Func<object, string> _useSurroundingCharactersForItem = UseQuotationMarksForNonPrimitiveTypes;

        /// <summary>
        ///     Gets or sets the delegate that determines which items are surrounded by special characters when <see cref="AppendItems{T}" /> or <see cref="AppendKeyValuePairs{TKey,TValue}" /> appends content to a string builder.
        ///     This delegate point to the <see cref="UseQuotationMarksForNonPrimitiveTypes" /> method by default.
        /// </summary>
        public static Func<object, string> UseSurroundingCharactersForItem
        {
            get { return _useSurroundingCharactersForItem; }
            set
            {
                value.MustNotBeNull();
                _useSurroundingCharactersForItem = value;
            }
        }

        /// <summary>
        ///     Appends the string representations of the specified items to the string builder.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="items" />.</typeparam>
        /// <param name="stringBuilder">The string builder where the items will be appended to.</param>
        /// <param name="items">The items to be appended.</param>
        /// <param name="itemSeparator">
        ///     The characters used to separate the items. Defaults to ", " and is not appended after the
        ///     last item.
        /// </param>
        /// <param name="emptyCollectionText">
        ///     The text that is appended to the string builder when <paramref name="items" /> is
        ///     empty. Defaults to "empty collection".
        /// </param>
        /// <returns>The string builder to enable method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters but <paramref name="emptyCollectionText" /> is null.</exception>
        public static StringBuilder AppendItems<T>(this StringBuilder stringBuilder, IEnumerable<T> items, string itemSeparator = ", ", string emptyCollectionText = "empty collection")
        {
            // ReSharper disable PossibleMultipleEnumeration
            stringBuilder.MustNotBeNull(nameof(stringBuilder));

            items.MustNotBeNull(nameof(items));
            itemSeparator.MustNotBeNull(nameof(itemSeparator));

            var itemsCount = items.Count();
            if (itemsCount == 0)
                return stringBuilder.Append(emptyCollectionText);

            var currentIndex = 0;
            foreach (var itemToAppend in items)
            {
                var surroundingCharacters = UseSurroundingCharactersForItem(itemToAppend);
                stringBuilder.Append(surroundingCharacters);
                stringBuilder.Append(itemToAppend.ToStringOrNull());
                stringBuilder.Append(surroundingCharacters);
                if (currentIndex < itemsCount - 1)
                    stringBuilder.Append(itemSeparator);
                else
                    break;

                currentIndex++;
            }

            return stringBuilder;
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        ///     Appends the string representation of the specified items to the string builder, using <see cref="DefaultNewLineSeparator" /> after each item but the last one, and the default empty collection text.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="items" />.</typeparam>
        /// <param name="stringBuilder">The string builder where the items will be appended to.</param>
        /// <param name="items">The items to be appended.</param>
        /// <returns>The string builder to enable method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        public static StringBuilder AppendItemsWithNewLine<T>(this StringBuilder stringBuilder, IEnumerable<T> items)
        {
            return stringBuilder.AppendItems(items, DefaultNewLineSeparator);
        }

        /// <summary>
        ///     Appends the string reprensentations of the keys and values of the specified dictionary to the string builder.
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
        /// <param name="stringBuilder">The string builder where the pairs will be appended to.</param>
        /// <param name="dictionary">The dictionary whose keys and values will be appended.</param>
        /// <param name="pairSeparator">
        ///     The characters used to separate the entries. Defaults to ", " and is not appended after the last key-value-pair.
        /// </param>
        /// <param name="emptyDictionaryText">
        ///     The text that is appended to the string builder when <paramref name="dictionary" /> is empty. Defaults to "empty dictionary".
        /// </param>
        /// <returns>The string builder to enable method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters but <paramref name="emptyDictionaryText" /> is null.</exception>
        public static StringBuilder AppendKeyValuePairs<TKey, TValue>(this StringBuilder stringBuilder, IDictionary<TKey, TValue> dictionary, string pairSeparator = ", ", string emptyDictionaryText = "empty dictionary")
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            dictionary.MustNotBeNull(nameof(dictionary));
            pairSeparator.MustNotBeNull(nameof(pairSeparator));

            if (dictionary.Count == 0)
                return stringBuilder.Append(emptyDictionaryText);

            var currentIndex = 0;
            foreach (var keyValuePair in dictionary)
            {
                stringBuilder.Append('[');
                var keySurroundingCharacters = UseSurroundingCharactersForItem(keyValuePair.Key);
                stringBuilder.Append(keySurroundingCharacters);
                stringBuilder.Append(keyValuePair.Key);
                stringBuilder.Append(keySurroundingCharacters);
                stringBuilder.Append("] = ");

                var valueSurroundingCharacters = UseSurroundingCharactersForItem(keyValuePair.Value);
                stringBuilder.Append(valueSurroundingCharacters);
                stringBuilder.Append(keyValuePair.Value.ToStringOrNull());
                stringBuilder.Append(valueSurroundingCharacters);

                if (currentIndex < dictionary.Count - 1)
                    stringBuilder.Append(pairSeparator);
                else
                    break;

                currentIndex++;
            }

            return stringBuilder;
        }

        /// <summary>
        ///     Appends the string representation of the keys and values of the specified dictionary to the string builder, using <see cref="DefaultNewLineSeparator" /> after each pair but the last one, and the default empty dictionary text.
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
        /// <param name="stringBuilder">The string builder where the pairs will be appended to.</param>
        /// <param name="dictionary">The dictionary whose key-value-pairs will be appended.</param>
        /// <returns>The string builder to enable method chaining.</returns>
        /// <exception cref="ArgumentNullException">Occurs when any of the parameters is null.</exception>
        public static StringBuilder AppendKeyValuePairsWithNewLine<TKey, TValue>(this StringBuilder stringBuilder, IDictionary<TKey, TValue> dictionary)
        {
            return stringBuilder.AppendKeyValuePairs(dictionary, DefaultNewLineSeparator);
        }

        /// <summary>
        ///     Returns the string reprensentation of item or nullText if item is null.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item whose string representation should be returned.</param>
        /// <param name="nullText">The text that is returned when item is null (defaults to "null").</param>
        /// <returns>The string representation of item, or nullText.</returns>
        public static string ToStringOrNull<T>(this T item, string nullText = "null")
        {
            return item == null ? nullText : item.ToString();
        }

        /// <summary>
        ///     This method returns quotation marks for all types that are not primitive numerics (like e.g. int, long, short, uint, etc.) or types regarding dates and times (DateTime, DateTimeOffset, TimeSpan).
        ///     For the latter types, an empty string is returned. This method is not supposed to be called by you directly, but is the default value for <see cref="UseSurroundingCharactersForItem" />.
        /// </summary>
        public static string UseQuotationMarksForNonPrimitiveTypes(object value)
        {
            if (value == null)
                return string.Empty;

            var type = value.GetType();
            if (type == typeof (int) ||
                type == typeof (long) ||
                type == typeof (short) ||
                type == typeof (sbyte) ||
                type == typeof (uint) ||
                type == typeof (ulong) ||
                type == typeof (ushort) ||
                type == typeof (byte) ||
                type == typeof (bool) ||
                type == typeof (double) ||
                type == typeof (decimal) ||
                type == typeof (float) ||
                type == typeof (DateTime) ||
                type == typeof (DateTimeOffset) ||
                type == typeof (TimeSpan))
                return string.Empty;

            return "\"";
        }
    }
}