using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustNotBeInBenchmark
    {
        public Range<int> Range = Range<int>.FromInclusive(1000).ToExclusive(15000);
        public int Value = 42;

        [Benchmark(Baseline = true)]
        public int ImperativeVersion()
        {
            if (Value >= 1000 && Value <= 150000) throw new ArgumentOutOfRangeException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int LightGuardClausesNewRange() => Value.MustNotBeIn(Range<int>.FromInclusive(1000).ToExclusive(15000), nameof(Value));

        [Benchmark]
        public int LightGuardClausesExistingRange() => Value.MustNotBeIn(Range, nameof(Value));

        [Benchmark]
        public int LightGuardClausesCustomException() => Value.MustNotBeIn(Range, (value, range) => new Exception($"{value} is in {range}."));

        [Benchmark]
        public int OldVersion() => Value.OldMustNotBeIn(Range, nameof(Value));
    }

    public static class MustNotBeInExtensionMethod
    {
        public static T OldMustNotBeIn<T>(this T parameter, Range<T> range, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (range.IsValueWithinRange(parameter) == false)
                return parameter;

            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";
            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }
    }
}