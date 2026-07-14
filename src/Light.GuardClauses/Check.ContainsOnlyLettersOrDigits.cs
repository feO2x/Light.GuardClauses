using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is non-null and every character is a Unicode letter or decimal digit. Empty strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> is non-null and every character is a Unicode letter or decimal digit, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsOnlyLettersOrDigits(this string? parameter) =>
        parameter is not null && parameter.AsSpan().ContainsOnlyLettersOrDigits();

    /// <summary>
    /// Checks if every character in the specified span is a Unicode letter or decimal digit. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if every character in <paramref name="parameter" /> is a Unicode letter or decimal digit, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsOnlyLettersOrDigits(this Span<char> parameter) =>
        ((ReadOnlySpan<char>) parameter).ContainsOnlyLettersOrDigits();

    /// <summary>
    /// Checks if every character in the specified read-only span is a Unicode letter or decimal digit. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <returns>
    /// True if every character in <paramref name="parameter" /> is a Unicode letter or decimal digit, otherwise false.
    /// </returns>
    public static bool ContainsOnlyLettersOrDigits(this ReadOnlySpan<char> parameter)
    {
        foreach (var character in parameter)
        {
            if (!char.IsLetterOrDigit(character))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if every character in the specified memory is a Unicode letter or decimal digit. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <returns>
    /// True if every character in <paramref name="parameter" /> is a Unicode letter or decimal digit, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsOnlyLettersOrDigits(this Memory<char> parameter) =>
        parameter.Span.ContainsOnlyLettersOrDigits();

    /// <summary>
    /// Checks if every character in the specified read-only memory is a Unicode letter or decimal digit. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <returns>
    /// True if every character in <paramref name="parameter" /> is a Unicode letter or decimal digit, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsOnlyLettersOrDigits(this ReadOnlyMemory<char> parameter) =>
        parameter.Span.ContainsOnlyLettersOrDigits();
}
