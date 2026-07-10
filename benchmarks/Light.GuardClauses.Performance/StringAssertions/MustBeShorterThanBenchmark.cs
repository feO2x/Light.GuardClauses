using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustBeShorterThanBenchmark
    {
        public int LongLength = 111;
        public string LongString = "I’m quite good at spending money but a lifetime of outrageous wealth has not taught me much about managing it.";
        public int ShortLength = 6;
        public string ShortString = "Hello";

        [Benchmark(Baseline = true)]
        public string ImperativeShort()
        {
            if (ShortString == null)
                throw new ArgumentNullException(nameof(ShortString));
            if (ShortString.Length >= ShortLength)
                throw new StringLengthException(nameof(ShortString));
            return ShortString;
        }

        [Benchmark]
        public string ImperativeLong()
        {
            if (LongString == null)
                throw new ArgumentNullException(nameof(ShortString));
            if (LongString.Length >= LongLength)
                throw new StringLengthException(nameof(ShortString));
            return LongString;
        }

        [Benchmark]
        public string LightGuardClausesShort() => ShortString.MustBeShorterThan(ShortLength, nameof(ShortString));

        [Benchmark]
        public string LightGuardClausesLong() => LongString.MustBeShorterThan(LongLength, nameof(LongString));
    }
}