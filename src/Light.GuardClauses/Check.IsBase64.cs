using System;
using System.Runtime.CompilerServices;
#if NET10_0_OR_GREATER
using System.Buffers.Text;
#endif

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the string is non-null and valid standard Base64. Space, tab, carriage return, and line feed are ignored.
    /// Empty and whitespace-only strings are valid.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> is non-null and contains valid standard Base64, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBase64(this string? parameter) => parameter is not null && parameter.AsSpan().IsBase64();

    /// <summary>
    /// Checks if the span is valid standard Base64. Space, tab, carriage return, and line feed are ignored.
    /// Empty and whitespace-only spans are valid.
    /// </summary>
    /// <param name="parameter">The character span to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains valid standard Base64, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBase64(this Span<char> parameter) => ((ReadOnlySpan<char>) parameter).IsBase64();

    /// <summary>
    /// Checks if the read-only span is valid standard Base64. Space, tab, carriage return, and line feed are ignored.
    /// Empty and whitespace-only spans are valid.
    /// </summary>
    /// <param name="parameter">The read-only character span to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains valid standard Base64, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBase64(this ReadOnlySpan<char> parameter)
    {
#if NET10_0_OR_GREATER
        return Base64.IsValid(parameter);
#else
        return IsBase64Portable(parameter);
#endif
    }

    /// <summary>
    /// Checks if the memory is valid standard Base64. Space, tab, carriage return, and line feed are ignored.
    /// Empty and whitespace-only memory is valid.
    /// </summary>
    /// <param name="parameter">The character memory to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains valid standard Base64, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBase64(this Memory<char> parameter) => parameter.Span.IsBase64();

    /// <summary>
    /// Checks if the read-only memory is valid standard Base64. Space, tab, carriage return, and line feed are ignored.
    /// Empty and whitespace-only memory is valid.
    /// </summary>
    /// <param name="parameter">The read-only character memory to be checked.</param>
    /// <returns>True if <paramref name="parameter" /> contains valid standard Base64, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsBase64(this ReadOnlyMemory<char> parameter) => parameter.Span.IsBase64();

    private static bool IsBase64Portable(ReadOnlySpan<char> parameter)
    {
        var nonWhiteSpaceCount = 0;
        var paddingCount = 0;
        var lastDataValue = 0;

        foreach (var character in parameter)
        {
            if (character is ' ' or '\t' or '\r' or '\n')
            {
                continue;
            }

            ++nonWhiteSpaceCount;

            if (character == '=')
            {
                if (++paddingCount > 2)
                {
                    return false;
                }

                continue;
            }

            if (paddingCount != 0 || !TryGetBase64Value(character, out lastDataValue))
            {
                return false;
            }
        }

        if ((nonWhiteSpaceCount & 3) != 0)
        {
            return false;
        }

        return paddingCount switch
        {
            0 => true,
            1 => nonWhiteSpaceCount >= 4 && (lastDataValue & 0b11) == 0,
            2 => nonWhiteSpaceCount >= 4 && (lastDataValue & 0b1111) == 0,
            _ => false,
        };
    }

    private static bool TryGetBase64Value(char character, out int value)
    {
        if (character is >= 'A' and <= 'Z')
        {
            value = character - 'A';
            return true;
        }

        if (character is >= 'a' and <= 'z')
        {
            value = character - 'a' + 26;
            return true;
        }

        if (character is >= '0' and <= '9')
        {
            value = character - '0' + 52;
            return true;
        }

        if (character == '+')
        {
            value = 62;
            return true;
        }

        if (character == '/')
        {
            value = 63;
            return true;
        }

        value = 0;
        return false;
    }
}
