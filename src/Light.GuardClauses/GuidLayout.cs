using System;
using System.Runtime.InteropServices;

namespace Light.GuardClauses;

/// <summary>
/// Overlays the stable sequential GUID field layout so UUID structural fields can be inspected without allocations.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 16)]
internal readonly struct GuidLayout
{
    [FieldOffset(0)]
    private readonly Guid _value;

    [FieldOffset(6)]
    public readonly ushort TimeHighAndVersion;

    [FieldOffset(8)]
    public readonly byte ClockSequenceHighAndReserved;

    public GuidLayout(Guid value) => _value = value;
}
