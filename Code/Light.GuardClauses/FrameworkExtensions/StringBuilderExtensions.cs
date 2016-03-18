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
        /// <param name="itemSeperators">
        ///     The characters used to seperate the items. Defaults to ", " and is not appended after the
        ///     last item.
        /// </param>
        /// <returns>The string builder to enable method chaining.</returns>
        public static StringBuilder AppendItems<T>(this StringBuilder stringBuilder, IReadOnlyList<T> items, string itemSeperators = ", ")
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            items.MustNotBeNullOrEmpty(nameof(items));

            for (var i = 0; i < items.Count; i++)
            {
                var itemToAppend = items[i];
                stringBuilder.Append(itemToAppend != null ? itemToAppend.ToString() : "null");
                if (i < items.Count - 1)
                    stringBuilder.Append(itemSeperators);
            }

            return stringBuilder;
        }
    }
}