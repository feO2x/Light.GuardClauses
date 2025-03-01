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
    public static bool IsFileExtension([NotNullWhen(true)] this string? value)
    {
        if (value is not { Length: > 1 } || value[0] != '.' || value[value.Length - 1] == '.')
        {
            return false;
        }

        var hasAlphanumeric = false;
        for (var i = 1; i < value.Length; i++)
        {
            var c = value[i];
            if (c.IsLetterOrDigit())
            {
                hasAlphanumeric = true;
            }
            else if (c != '.')
            {
                return false;
            }
        }

        return hasAlphanumeric;
    }
}
