using System;
using System.Runtime.CompilerServices;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Checks if the specified GUID structurally identifies an RFC/IETF UUID version 7.
    /// </summary>
    /// <param name="parameter">The GUID to be checked.</param>
    /// <returns>True when the UUID version is 7 and the variant is RFC/IETF, otherwise false.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUuidVersion7(this Guid parameter)
    {
        var layout = new GuidLayout(parameter);
        return (layout.TimeHighAndVersion & 0xF000) == 0x7000 &&
               (layout.ClockSequenceHighAndReserved & 0xC0) == 0x80;
    }
}
