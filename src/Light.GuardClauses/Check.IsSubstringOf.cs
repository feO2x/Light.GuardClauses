using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the string is a substring of the other string.
    /// </summary>
    /// <param name="value">The string to be checked.</param>
    /// <param name="other">The other string.</param>
    /// <returns>True if <paramref name="value" /> is a substring of <paramref name="other" />, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> or <paramref name="other" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("value:null => halt; other:null => halt")]
    // ReSharper disable RedundantNullableFlowAttribute
    public static bool IsSubstringOf(
        [NotNull] [ValidatedNotNull] this string value,
        [NotNull] [ValidatedNotNull] string other
    ) =>
        other.MustNotBeNull(nameof(other)).Contains(value);
    // ReSharper restore RedundantNullableFlowAttribute

    /// <summary>
    /// Checks if the string is a substring of the other string.
    /// </summary>
    /// <param name="value">The string to be checked.</param>
    /// <param name="other">The other string.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
    /// <returns>True if <paramref name="value" /> is a substring of <paramref name="other" />, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> or <paramref name="other" /> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is not a valid <see cref="StringComparison" /> value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("value:null => halt; other:null => halt")]
    // ReSharper disable RedundantNullableFlowAttribute
    public static bool IsSubstringOf(
        [NotNull] [ValidatedNotNull] this string value,
        [NotNull] [ValidatedNotNull] string other,
        StringComparison comparisonType
    ) =>
        other.MustNotBeNull(nameof(other)).IndexOf(value, comparisonType) != -1;
    // ReSharper disable RedundantNullableFlowAttribute
}
