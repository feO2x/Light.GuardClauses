using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is non-null and contains no Unicode uppercase character. Empty strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> is non-null and contains no Unicode uppercase character, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLowerCase(this string? parameter) => parameter is not null && parameter.AsSpan().IsLowerCase();

    /// <summary>
    /// Checks if the specified span contains no Unicode uppercase character. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode uppercase character, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLowerCase(this Span<char> parameter) => ((ReadOnlySpan<char>) parameter).IsLowerCase();

    /// <summary>
    /// Checks if the specified read-only span contains no Unicode uppercase character. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode uppercase character, otherwise false.</returns>
    public static bool IsLowerCase(this ReadOnlySpan<char> parameter)
    {
        foreach (var character in parameter)
        {
            if (char.IsUpper(character))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the specified memory contains no Unicode uppercase character. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode uppercase character, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLowerCase(this Memory<char> parameter) => parameter.Span.IsLowerCase();

    /// <summary>
    /// Checks if the specified read-only memory contains no Unicode uppercase character. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains no Unicode uppercase character, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLowerCase(this ReadOnlyMemory<char> parameter) => parameter.Span.IsLowerCase();
}
