using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustNotBeEquivalentToBenchmark : DefaultBenchmark
    {
        public string First = "Foo";
        public string Second = "Bar";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (string.Equals(First, Second, StringComparison.OrdinalIgnoreCase))
                throw new StringException(nameof(First));
            return First;
        }

        [Benchmark]
        public string LightGuardClauses() => First.MustNotBeEquivalentTo(Second, parameterName: nameof(First));

        [Benchmark]
        public string LightGuardClausesCustomException() => First.MustNotBeEquivalentTo(Second, (x, y) => new Exception($"{x} is not equivalent to {y}"));

        [Benchmark]
        public string OldVersion() => First.OldMustNotBeEquivalentTo(Second, parameterName: nameof(First));
    }

    public static class MustNotBeEquivalentToExtensions
    {
        public static string OldMustNotBeEquivalentTo(this string parameter, string other, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            other.MustNotBeNull(nameof(other));

            if (parameter.IsEquivalentTo(other, comparisonType) == false) return parameter;

            throw exception?.Invoke() ?? new StringException(parameterName, message ?? $"\"{parameter}\" must not be equivalent to \"{other}\" (using {comparisonType}), but it is.");
        }
    }
}