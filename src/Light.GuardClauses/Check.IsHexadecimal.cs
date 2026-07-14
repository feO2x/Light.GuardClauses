using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is non-null and contains only ASCII hexadecimal characters. Empty strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> is non-null and contains only ASCII hexadecimal characters, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHexadecimal(this string? parameter) =>
        parameter is not null && parameter.AsSpan().IsHexadecimal();

    /// <summary>
    /// Checks if the specified span contains only ASCII hexadecimal characters. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> contains only ASCII hexadecimal characters, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHexadecimal(this Span<char> parameter) => ((ReadOnlySpan<char>) parameter).IsHexadecimal();

    /// <summary>
    /// Checks if the specified read-only span contains only ASCII hexadecimal characters. Empty spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> contains only ASCII hexadecimal characters, otherwise false.
    /// </returns>
    public static bool IsHexadecimal(this ReadOnlySpan<char> parameter)
    {
        foreach (var character in parameter)
        {
#if NET10_0_OR_GREATER
            if (!char.IsAsciiHexDigit(character))
#else
            if (!IsAsciiHexDigit(character))
#endif
            {
                return false;
            }
        }

        return true;
    }

#if !NET10_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAsciiHexDigit(char character) =>
        character is >= '0' and <= '9' or >= 'A' and <= 'F' or >= 'a' and <= 'f';
#endif

    /// <summary>
    /// Checks if the specified memory contains only ASCII hexadecimal characters. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> contains only ASCII hexadecimal characters, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHexadecimal(this Memory<char> parameter) => parameter.Span.IsHexadecimal();

    /// <summary>
    /// Checks if the specified read-only memory contains only ASCII hexadecimal characters. Empty memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <returns>
    /// True if <paramref name="parameter" /> contains only ASCII hexadecimal characters, otherwise false.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsHexadecimal(this ReadOnlyMemory<char> parameter) => parameter.Span.IsHexadecimal();
}
