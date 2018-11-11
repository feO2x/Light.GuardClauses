using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustBeBenchmark
    {
        public string X = "{\"foo\":42}";
        public string Y = "{\r\n  \"foo\": 42\r\n}\r\n";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (!X.Equals(Y, StringComparisonType.OrdinalIgnoreWhiteSpace))
                throw new ValuesNotEqualException(nameof(X));
            return X;
        }

        [Benchmark]
        public string LightGuardClauses() => X.MustBe(Y, StringComparisonType.OrdinalIgnoreWhiteSpace);

        [Benchmark]
        public string LightGuardClausesCustomException() => 
            X.MustBe(Y, StringComparisonType.OrdinalIgnoreWhiteSpace, (x, y) => new Exception("The strings are not equal."));
    }
}