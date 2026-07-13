using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustNotBeBenchmark
    {
        public string X = "This is the first string";

        // ReSharper disable once StringLiteralTypo
        public string Y = "THISISTHESECONDSTRING";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (X.Equals(Y, StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace))
                throw new ValuesEqualException(nameof(X));

            return X;
        }

        [Benchmark]
        public string LightGuardClauses() => X.MustNotBe(Y, StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace);

        [Benchmark]
        public string LightGuardClausesCustomException() =>
            X.MustNotBe(Y, StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace, (_, _, _) => new Exception("The strings are equal."));
    }
}