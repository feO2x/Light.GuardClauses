using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified string is null, empty, or contains only white space.
    /// </summary>
    /// <param name="string">The string to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("=> false, string:notnull; => true, string:canbenull")]
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? @string) =>
        string.IsNullOrWhiteSpace(@string);
}
