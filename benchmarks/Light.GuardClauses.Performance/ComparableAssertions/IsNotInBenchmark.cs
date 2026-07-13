using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class IsNotInBenchmark
    {
        public const char LowerBoundary = 'A';
        public const char UpperBoundary = 'G';
        public Range<char> ExistingRange = Range.FromInclusive(LowerBoundary).ToInclusive(UpperBoundary);
        public char Value = 'I';

        [Benchmark(Baseline = true)]
        public bool Imperative() => Value < ExistingRange.From || Value > ExistingRange.To;

        [Benchmark]
        public bool LightGuardClausesNewRangeInstance() => Value.IsNotIn(Range.FromInclusive(LowerBoundary).ToInclusive(UpperBoundary));

        [Benchmark]
        public bool LightGuardClausesExistingRangeInstance() => Value.IsNotIn(ExistingRange);
    }
}