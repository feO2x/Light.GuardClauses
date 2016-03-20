using System;
using System.Collections.Generic;
using System.Text;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    ///     The StringBuilderExtensions class contains an extension method that encapsulates the adding of items from a
    ///     collection to a string builder.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        ///     Appends the string representations of the specified items to the string builder.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="items" />.</typeparam>
        /// <param name="stringBuilder">The string builder to append the items to.</param>
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
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
        public static StringBuilder AppendItems<T>(this StringBuilder stringBuilder, IReadOnlyCollection<T> items, string itemSeparator = ", ", string emptyCollectionText = "empty collection")
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            items.MustNotBeNull(nameof(items));
            itemSeparator.MustNotBeNull(nameof(itemSeparator));

            if (items.Count == 0)
                return stringBuilder.Append(emptyCollectionText);

            var currentIndex = 0;
            foreach (var itemToAppend in items)
            {
                stringBuilder.Append(itemToAppend != null ? itemToAppend.ToString() : "null");
                if (currentIndex < items.Count - 1)
                    stringBuilder.Append(itemSeparator);
                else
                    break;

                currentIndex++;
            }

            return stringBuilder;
        }

        /// <summary>
        ///     Appends the string reprensentations of the keys and values of the specified dictionary to the string builder.
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionary.</typeparam>
        /// <typeparam name="TValue">The value type of the dictionary.</typeparam>
        /// <param name="stringBuilder">The string builder to append the items to.</param>
        /// <param name="dictionary">The dictionary whose keys and values will be appended.</param>
        /// <param name="pairSeparator">
        ///     The characters used to separate the entries. Defaults to ", " and is not appended after the
        ///     last key-value-pair.
        /// </param>
        /// <param name="emptyDictionaryText">
        ///     The text that is appended to the string builder when <paramref name="dictionary" />
        ///     is empty. Defaults to "empty dictionary".
        /// </param>
        /// <returns>The string builder to enable method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is null.</exception>
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
                stringBuilder.Append(keyValuePair.Key);
                stringBuilder.Append("] = ");
                stringBuilder.Append(keyValuePair.Value);
                if (currentIndex < dictionary.Count - 1)
                    stringBuilder.Append(pairSeparator);
                else
                    break;

                currentIndex++;
            }

            return stringBuilder;
        }
    }
}