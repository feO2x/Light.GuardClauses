using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string represents a valid file extension.
    /// </summary>
    /// <param name="value">
    /// The string to be checked. It must start with a period (.) and can only contain letters, digits,
    /// and additional periods.
    /// </param>
    /// <returns>True if the string is a valid file extension, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFileExtension([NotNullWhen(true)] this string? value) =>
        value != null && IsFileExtension(value.AsSpan());

    /// <summary>
    /// Checks if the specified character span represents a valid file extension.
    /// </summary>
    /// <param name="value">
    /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
    /// and additional periods.
    /// </param>
    /// <returns>True if the span is a valid file extension, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFileExtension(this Span<char> value) => IsFileExtension((ReadOnlySpan<char>) value);

    /// <summary>
    /// Checks if the specified character memory represents a valid file extension.
    /// </summary>
    /// <param name="value">
    /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
    /// and additional periods.
    /// </param>
    /// <returns>True if the span is a valid file extension, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFileExtension(this ReadOnlyMemory<char> value) => IsFileExtension(value.Span);

    /// <summary>
    /// Checks if the specified character memory represents a valid file extension.
    /// </summary>
    /// <param name="value">
    /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
    /// and additional periods.
    /// </param>
    /// <returns>True if the span is a valid file extension, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFileExtension(this Memory<char> value) => IsFileExtension(value.Span);

    /// <summary>
    /// Checks if the specified character span represents a valid file extension.
    /// </summary>
    /// <param name="value">
    /// The character span to be checked. It must start with a period (.) and can only contain letters, digits,
    /// and additional periods.
    /// </param>
    /// <returns>True if the span is a valid file extension, false otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFileExtension(this ReadOnlySpan<char> value)
    {
        // ReSharper disable once UseIndexFromEndExpression -- cannot use index from end expression in .NET Standard 2.0
        if (value.Length <= 1 || value[0] != '.' || value[value.Length - 1] == '.')
        {
            return false;
        }

        var hasAlphanumeric = false;
        for (var i = 1; i < value.Length; i++)
        {
            var character = value[i];
            if (character.IsLetterOrDigit())
            {
                hasAlphanumeric = true;
            }
            else if (character != '.')
            {
                return false;
            }
        }

        return hasAlphanumeric;
    }
}
