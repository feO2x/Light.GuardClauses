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
        /// <param name="itemsSeparator">
        ///     The characters used to separate the items. Defaults to ", " and is not appended after the
        ///     last item.
        /// </param>
        /// <returns>The string builder to enable method chaining.</returns>
        public static StringBuilder AppendItems<T>(this StringBuilder stringBuilder, IReadOnlyCollection<T> items, string itemsSeparator = ", ")
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            items.MustNotBeNullOrEmpty(nameof(items));
            itemsSeparator.MustNotBeNull(nameof(itemsSeparator));

            var currentIndex = 0;
            foreach (var itemToAppend in items)
            {
                stringBuilder.Append(itemToAppend != null ? itemToAppend.ToString() : "null");
                if (currentIndex < items.Count - 1)
                    stringBuilder.Append(itemsSeparator);
                else
                    break;

                currentIndex++;
            }

            return stringBuilder;
        }
    }
}