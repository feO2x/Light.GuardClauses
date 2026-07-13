using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsWhiteSpaceBenchmark
    {
        public char NoWhiteSpace = 'K';
        public char WhiteSpace = '\t';

        [Benchmark(Baseline = true)]
        public bool ImperativeWhiteSpace() => char.IsWhiteSpace(WhiteSpace);

        [Benchmark]
        public bool ImperativeNoWhiteSpace() => char.IsWhiteSpace(NoWhiteSpace);

        [Benchmark]
        public bool LightGuardClausesWhiteSpace() => WhiteSpace.IsWhiteSpace();

        [Benchmark]
        public bool LightGuardClausesNoWhiteSpace() => NoWhiteSpace.IsWhiteSpace();
    }
}