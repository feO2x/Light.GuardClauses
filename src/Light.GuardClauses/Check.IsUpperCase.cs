using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is non-null and contains no Unicode lowercase character. Empty strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> is non-null and contains no Unicode lowercase character, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUpperCase(this string? parameter) => parameter is not null && parameter.AsSpan().IsUpperCase();

    /// <summary>
    /// Checks if the specified span contains no Unicode lowercase character. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode lowercase character, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUpperCase(this Span<char> parameter) => ((ReadOnlySpan<char>) parameter).IsUpperCase();

    /// <summary>
    /// Checks if the specified read-only span contains no Unicode lowercase character. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode lowercase character, otherwise false.</returns>
    public static bool IsUpperCase(this ReadOnlySpan<char> parameter)
    {
        foreach (var character in parameter)
        {
            if (char.IsLower(character))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the specified memory contains no Unicode lowercase character. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode lowercase character, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUpperCase(this Memory<char> parameter) => parameter.Span.IsUpperCase();

    /// <summary>
    /// Checks if the specified read-only memory contains no Unicode lowercase character. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode lowercase character, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUpperCase(this ReadOnlyMemory<char> parameter) => parameter.Span.IsUpperCase();
}
