using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsLetterBenchmark
    {
        public char Letter = 'w';
        public char NoLetter = '%';

        [Benchmark(Baseline = true)]
        public bool ImperativeLetter() => char.IsLetter(Letter);

        [Benchmark]
        public bool ImperativeNoLetter() => char.IsLetter(NoLetter);

        [Benchmark]
        public bool LightGuardClausesLetter() => Letter.IsLetter();

        [Benchmark]
        public bool LightGuardClausesNoLetter() => NoLetter.IsLetter();
    }
}