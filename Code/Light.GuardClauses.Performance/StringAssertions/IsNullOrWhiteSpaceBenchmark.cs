using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsNullOrWhiteSpaceBenchmark
    {
        public string EmptyString = "";
        public string NullString = null;
        public string StringWithContent = "What`s up";
        public string WhiteSpaceString = "  \t\r\n";

        [Benchmark(Baseline = true)]
        public bool ImperativeNull() => string.IsNullOrWhiteSpace(NullString);

        [Benchmark]
        public bool ImperativeEmpty() => string.IsNullOrWhiteSpace(EmptyString);

        [Benchmark]
        public bool ImperativeWhiteSpace() => string.IsNullOrWhiteSpace(WhiteSpaceString);

        [Benchmark]
        public bool ImperativeStringWithContent() => string.IsNullOrWhiteSpace(StringWithContent);

        [Benchmark]
        public bool LightGuardClausesNull() => NullString.IsNullOrWhiteSpace();

        [Benchmark]
        public bool LightGuardClausesEmpty() => EmptyString.IsNullOrWhiteSpace();

        [Benchmark]
        public bool LightGuardClausesWhiteSpaceString() => WhiteSpaceString.IsNullOrWhiteSpace();

        [Benchmark]
        public bool LightGuardClausesStringWithContent() => StringWithContent.IsNullOrWhiteSpace();
    }
}