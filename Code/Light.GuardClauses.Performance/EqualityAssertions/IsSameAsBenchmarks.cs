using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.EqualityAssertions
{
    public class IsSameAsBenchmarks : DefaultBenchmark
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

        [Benchmark]
        public bool OldVersion() => First.OldIsSameAs(Second);
    }

    public static class IsSameAsExtensionMethods
    {
        public static bool OldIsSameAs<T>(this T parameter, T reference) where T : class
        {
            return ReferenceEquals(parameter, reference);
        }
    }
}