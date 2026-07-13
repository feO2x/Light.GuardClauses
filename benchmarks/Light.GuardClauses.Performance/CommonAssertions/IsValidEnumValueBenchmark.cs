using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class IsValidEnumValueBenchmark
    {
        public AttributeTargets AttributeTargets = AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface;
        public StringComparison ComparisonType = StringComparison.Ordinal;

        [Benchmark(Baseline = true)]
        public bool Imperative() => Enum.IsDefined(typeof(StringComparison), ComparisonType);

        [Benchmark]
        public bool LightGuardClauses() => ComparisonType.IsValidEnumValue();

        [Benchmark]
        public bool LightGuardClausesFlags() => AttributeTargets.IsValidEnumValue();
    }
}