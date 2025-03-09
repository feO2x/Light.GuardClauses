using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.FrameworkExtensions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the given <paramref name="item" /> is one of the specified <paramref name="items" />.
    /// </summary>
    /// <param name="item">The item to be checked.</param>
    /// <param name="items">The collection that might contain the <paramref name="item" />.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="items" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("items:null => halt")]
    // ReSharper disable once RedundantNullableFlowAttribute - the attribute has an effect, see Issue72NotNullAttribute tests
    public static bool IsOneOf<TItem>(this TItem item, [NotNull] [ValidatedNotNull] IEnumerable<TItem> items)
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
}
