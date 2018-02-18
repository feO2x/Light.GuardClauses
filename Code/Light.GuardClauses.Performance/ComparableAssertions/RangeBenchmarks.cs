using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class RangeBenchmarks : DefaultBenchmark
    {
        public Range<int> Range = Range<int>.FromInclusive(30).ToInclusive(50);
        public int Value = 42;

        [Benchmark(Baseline = true)]
        public int BaseVersion()
        {
            if (Value < 30 || Value > 50) throw new ArgumentOutOfRangeException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int RangeWithInitialization()
        {
            if (Range<int>.FromInclusive(30).ToInclusive(50).IsValueWithinRange(Value) == false) throw new ArgumentOutOfRangeException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int PreinitializedRange()
        {
            if (Range.IsValueWithinRange(Value) == false) throw new ArgumentOutOfRangeException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int ExtensionMethod()
        {
            if (Value.IsNotIn(Range)) throw new ArgumentOutOfRangeException(nameof(Value));
            return Value;
        }
    }
}