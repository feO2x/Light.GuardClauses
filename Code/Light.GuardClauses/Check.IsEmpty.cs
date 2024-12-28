using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified GUID is an empty one.
    /// </summary>
    /// <param name="parameter">The GUID to be checked.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty(this Guid parameter) => parameter == Guid.Empty;
}
