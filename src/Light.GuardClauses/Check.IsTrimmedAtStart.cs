using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is trimmed at the start, i.e. it does not start with
    /// white space characters. Inputting an empty string will return true.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <param name="regardNullAsTrimmed">
    /// The value indicating whether true or false should be returned from this method when the
    /// <paramref name="parameter" /> is null. The default value is true.
    /// </param>
    /// <returns>
    /// True if the <paramref name="parameter" /> is trimmed at the start, else false.
    /// An empty string will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtStart(this string? parameter, bool regardNullAsTrimmed = true) =>
        parameter is null ? regardNullAsTrimmed : parameter.AsSpan().IsTrimmedAtStart();

    /// <summary>
    /// Checks if the specified character span is trimmed at the start, i.e. it does not start with
    /// white space characters. Inputting an empty span will return true.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if the <paramref name="parameter" /> is trimmed at the start, else false.
    /// An empty span will result in true.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrimmedAtStart(this ReadOnlySpan<char> parameter) =>
        parameter.Length == 0 || !parameter[0].IsWhiteSpace();
}
