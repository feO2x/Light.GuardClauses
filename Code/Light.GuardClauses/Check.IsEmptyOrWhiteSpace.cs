using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified span is empty or contains only white space characters.
    /// </summary>
    /// <param name="span">The span to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmptyOrWhiteSpace(this Span<char> span) => ((ReadOnlySpan<char>) span).IsEmptyOrWhiteSpace();

    /// <summary>
    /// Checks if the specified span is empty or contains only white space characters.
    /// </summary>
    /// <param name="span">The span to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> span)
    {
        if (span.IsEmpty)
        {
            return true;
        }

        foreach (var character in span)
        {
            if (!character.IsWhiteSpace())
            {
                return false;
            }
        }

        return true;
    }
}
