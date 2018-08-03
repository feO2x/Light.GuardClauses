using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public abstract class DefaultBenchmark { }
}