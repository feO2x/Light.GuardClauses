using System;
using System.Runtime.CompilerServices;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the value is within the specified range.
    /// </summary>
    /// <param name="parameter">The comparable to be checked.</param>
    /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
    /// <returns>True if the parameter is within the specified range, else false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIn<T>([NotNull] [ValidatedNotNull] this T parameter, Range<T> range)
        where T : IComparable<T> =>
        range.IsValueWithinRange(parameter);
}
