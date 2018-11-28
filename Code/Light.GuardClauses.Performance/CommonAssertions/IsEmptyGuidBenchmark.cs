using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class IsEmptyGuidBenchmark
    {
        public Guid Guid = Guid.NewGuid();

        [Benchmark(Baseline = true)]
        public bool Imperative() => Guid == Guid.Empty;

        [Benchmark]
        public bool LightGuardClauses() => Guid.IsEmpty();
    }
}