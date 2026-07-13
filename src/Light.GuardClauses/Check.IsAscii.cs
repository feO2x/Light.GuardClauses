using System;
using System.Runtime.CompilerServices;
#if NET8_0_OR_GREATER
using System.Text;
#endif

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>Checks if the character is an ASCII code point.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this char parameter) => parameter <= 0x7F;

    /// <summary>Checks if the byte is an ASCII value.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this byte parameter) => parameter <= 0x7F;

    /// <summary>Checks if the string is non-null and contains only ASCII characters.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this string? parameter) => parameter is not null && parameter.AsSpan().IsAscii();

    /// <summary>Checks if the character span contains only ASCII characters.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this Span<char> parameter) => ((ReadOnlySpan<char>) parameter).IsAscii();

    /// <summary>Checks if the read-only character span contains only ASCII characters.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlySpan<char> parameter)
    {
#if NET8_0_OR_GREATER
        return Ascii.IsValid(parameter);
#else
        foreach (var value in parameter)
        {
            if (value > 0x7F)
            {
                return false;
            }
        }

        return true;
#endif
    }

    /// <summary>Checks if the character memory contains only ASCII characters.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this Memory<char> parameter) => parameter.Span.IsAscii();

    /// <summary>Checks if the read-only character memory contains only ASCII characters.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlyMemory<char> parameter) => parameter.Span.IsAscii();

    /// <summary>Checks if the byte span contains only ASCII values.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this Span<byte> parameter) => ((ReadOnlySpan<byte>) parameter).IsAscii();

    /// <summary>Checks if the read-only byte span contains only ASCII values.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlySpan<byte> parameter)
    {
#if NET8_0_OR_GREATER
        return Ascii.IsValid(parameter);
#else
        foreach (var value in parameter)
        {
            if (value > 0x7F)
            {
                return false;
            }
        }

        return true;
#endif
    }

    /// <summary>Checks if the byte memory contains only ASCII values.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this Memory<byte> parameter) => parameter.Span.IsAscii();

    /// <summary>Checks if the read-only byte memory contains only ASCII values.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAscii(this ReadOnlyMemory<byte> parameter) => parameter.Span.IsAscii();
}
