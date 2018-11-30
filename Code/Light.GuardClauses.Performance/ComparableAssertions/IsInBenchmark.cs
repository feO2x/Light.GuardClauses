using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class IsInBenchmark
    {
        private const long LowerBoundary = 8_000_000_000_000_000_000L;
        public Range<long> ExistingRange = Range.FromInclusive(LowerBoundary).ToExclusive(long.MaxValue);
        public long Value = long.MaxValue - 100L;

        [Benchmark(Baseline = true)]
        public bool Imperative() =>
            Value >= ExistingRange.From && Value < ExistingRange.To;

        [Benchmark]
        public bool LightGuardClausesNewRangeInstance() => Value.IsIn(Range.FromInclusive(LowerBoundary).ToExclusive(long.MaxValue));

        [Benchmark]
        public bool LightGuardClausesExistingRangeInstance() => Value.IsIn(ExistingRange);
    }
}