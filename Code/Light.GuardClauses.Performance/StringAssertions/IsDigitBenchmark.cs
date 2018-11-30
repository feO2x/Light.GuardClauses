using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsDigitBenchmark
    {
        public char Digit = '9';
        public char NoDigit = 't';

        [Benchmark(Baseline = true)]
        public bool ImperativeDigit() => char.IsDigit(Digit);

        [Benchmark]
        public bool ImperativeNoDigit() => char.IsDigit(NoDigit);

        [Benchmark]
        public bool LightGuardClausesDigit() => Digit.IsDigit();

        [Benchmark]
        public bool LightGuardClausesNoDigit() => NoDigit.IsDigit();
    }
}