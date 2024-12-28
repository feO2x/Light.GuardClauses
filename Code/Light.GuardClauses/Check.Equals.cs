using System;
using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified strings are equal, using the given comparison rules.
    /// </summary>
    /// <param name="string">The first string to compare.</param>
    /// <param name="value">The second string to compare.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
    /// <returns>True if the two strings are considered equal, else false.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="comparisonType" /> is no valid enum value.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(this string? @string, string? value, StringComparisonType comparisonType)
    {
        if ((int) comparisonType < 6)
        {
            return string.Equals(@string, value, (StringComparison) comparisonType);
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
}
