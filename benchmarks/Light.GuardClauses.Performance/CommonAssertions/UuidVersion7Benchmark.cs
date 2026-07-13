using System;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions;

[MemoryDiagnoser]
public class UuidVersion7Benchmark
{
    private readonly Guid _invalidUuid = Guid.NewGuid();
    private readonly Guid _validUuid = Guid.CreateVersion7();

    [Benchmark(Baseline = true)]
    public bool DirectValidStructuralCheck() => DirectCheck(_validUuid);

    [Benchmark]
    public bool IsUuidVersion7Valid() => _validUuid.IsUuidVersion7();

    [Benchmark]
    public Guid MustBeUuidVersion7Valid() => _validUuid.MustBeUuidVersion7();

    [Benchmark]
    public bool DirectInvalidStructuralCheck() => DirectCheck(_invalidUuid);

    [Benchmark]
    public bool IsUuidVersion7Invalid() => _invalidUuid.IsUuidVersion7();

    private static bool DirectCheck(Guid value)
    {
        var layout = new BenchmarkGuidLayout(value);
        return (layout.TimeHighAndVersion & 0xF000) == 0x7000 &&
               (layout.ClockSequenceHighAndReserved & 0xC0) == 0x80;
    }

    [StructLayout(LayoutKind.Explicit, Size = 16)]
    private readonly struct BenchmarkGuidLayout
    {
        [FieldOffset(0)]
        private readonly Guid _value;

        [FieldOffset(6)]
        public readonly ushort TimeHighAndVersion;

        [FieldOffset(8)]
        public readonly byte ClockSequenceHighAndReserved;

        public BenchmarkGuidLayout(Guid value) => _value = value;
    }
}
