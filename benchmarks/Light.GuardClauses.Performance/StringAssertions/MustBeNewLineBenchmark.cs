using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustBeNewLineBenchmark
    {
        public string NewLineString = "\r\n";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (NewLineString == null) throw new ArgumentNullException(nameof(NewLineString));
            if (NewLineString != "\n" && NewLineString != "\r\n") throw new StringException(nameof(NewLineString));
            return NewLineString;
        }

        [Benchmark]
        public string LightGuardClauses() => NewLineString.MustBeNewLine(nameof(NewLineString));

        [Benchmark]
        public string LightGuardClausesCustomException() => NewLineString.MustBeNewLine(_ => new Exception("No new line string"));
    }
}