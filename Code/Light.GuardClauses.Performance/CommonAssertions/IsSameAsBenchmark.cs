using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class IsSameAsBenchmark : DefaultBenchmark
    {
        public object First = new object();
        public object Second = new object();

        [Benchmark(Baseline = true)]
        public bool EqualityOperator() => First == Second;

        [Benchmark]
        public bool ReferenceEquals() => ReferenceEquals(First, Second);

        [Benchmark]
        public bool Equals() => First.Equals(Second);

        [Benchmark]
        public bool LightGuardClauses() => First.IsSameAs(Second);
    }
}