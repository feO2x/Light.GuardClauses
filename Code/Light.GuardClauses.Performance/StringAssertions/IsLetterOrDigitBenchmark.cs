using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsLetterOrDigitBenchmark
    {
        public char Digit = '4';
        public char Letter = 'P';
        public char NoLetterOrDigit = '/';

        [Benchmark(Baseline = true)]
        public bool ImperativeLetter() => char.IsLetterOrDigit(Letter);

        [Benchmark]
        public bool ImperativeDigit() => char.IsLetterOrDigit(Digit);

        [Benchmark]
        public bool ImperativeNoLetterOrDigit() => char.IsLetterOrDigit(NoLetterOrDigit);

        [Benchmark]
        public bool LightGuardClausesLetter() => Letter.IsLetterOrDigit();

        [Benchmark]
        public bool LightGuardClausesDigit() => Digit.IsLetterOrDigit();

        [Benchmark]
        public bool LightGuardClausesNoLetterOrDigit() => NoLetterOrDigit.IsLetterOrDigit();
    }
}