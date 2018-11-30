using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsNullOrEmptyBenchmark
    {
        public string EmptyString = "";
        public string NullString = null;
        public string StringWithContent = "Hello";

        [Benchmark(Baseline = true)]
        public bool ImperativeNull() => string.IsNullOrEmpty(NullString);

        [Benchmark]
        public bool ImperativeEmpty() => string.IsNullOrEmpty(EmptyString);

        [Benchmark]
        public bool ImperativeStringWithContent() => string.IsNullOrEmpty(StringWithContent);

        [Benchmark]
        public bool LightGuardClausesNull() => NullString.IsNullOrEmpty();

        [Benchmark]
        public bool LightGuardClausesEmpty() => EmptyString.IsNullOrEmpty();

        [Benchmark]
        public bool LightGuardClausesStringWithContent() => StringWithContent.IsNullOrEmpty();
    }
}