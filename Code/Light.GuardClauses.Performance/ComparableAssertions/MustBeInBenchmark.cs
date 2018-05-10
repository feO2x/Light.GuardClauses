using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustBeInBenchmark : DefaultBenchmark
    {
        public Range<int> Range = Range<int>.FromInclusive(30).ToExclusive(50);
        public int Value = 42;

        [Benchmark(Baseline = true)]
        public int ImperativeVersion()
        {
            if (Value < 30 || Value >= 50) throw new ArgumentOutOfRangeException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int LightGuardClausesNewRange() => Value.MustBeIn(Range<int>.FromInclusive(30).ToExclusive(50), nameof(Value));

        [Benchmark]
        public int LightGuardClausesExistingRange() => Value.MustBeIn(Range, nameof(Value));

        [Benchmark]
        public int LightGuardClausesCustomException() => Value.MustBeIn(Range, (value, range) => new Exception($"{value} is not in {range}."));

        [Benchmark]
        public int OldVersion() => Value.OldMustBeIn(Range, nameof(Value));
    }

    public static class MustBeInExtensionMethod
    {
        public static T OldMustBeIn<T>(this T parameter, Range<T> range, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (range.IsValueWithinRange(parameter))
                return parameter;

            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";
            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but you specified {parameter}.");
        }
    }
}