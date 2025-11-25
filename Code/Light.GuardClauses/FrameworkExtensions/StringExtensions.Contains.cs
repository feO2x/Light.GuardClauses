using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the string contains the specified value using the given comparison type.
    /// </summary>
    /// <param name="string">The string to be checked.</param>
    /// <param name="value">The other string.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <returns>True if <paramref name="string" /> contains <paramref name="value" />, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="string" /> or <paramref name="value" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("string:null => halt; value:null => halt")]
    public static bool Contains(
        // ReSharper disable once RedundantNullableFlowAttribute -- Caller might have NRTs turned off
        [NotNull] [ValidatedNotNull] this string @string,
        string value,
        StringComparison comparisonType
    ) =>
        @string.MustNotBeNull(nameof(@string)).IndexOf(value.MustNotBeNull(nameof(value)), comparisonType) >= 0;
}
