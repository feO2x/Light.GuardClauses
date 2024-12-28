using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the string is either "\n" or "\r\n". This is done independently of the current value of <see cref="Environment.NewLine" />.
    /// </summary>
    /// <param name="parameter">The string to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("=> false, parameter:canbenull; => true, parameter:notnull")]
    public static bool IsNewLine([NotNullWhen(true)] this string? parameter) =>
        parameter == "\n" || parameter == "\r\n";
}
