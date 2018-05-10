using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustNotBeGreaterThanBenchmark : DefaultBenchmark
    {
        public long First = 14819929;
        public long Second = 60001200202;

        [Benchmark(Baseline = true)]
        public long ImperativeVersion()
        {
            if (First > Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public long ImperativeVersionWithCompareTo()
        {
            if (First.CompareTo(Second) > 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public long LightGuardClausesWithParameterName() => First.MustNotBeGreaterThan(Second, nameof(First));

        [Benchmark]
        public long LightGuardClausesCustomException() => First.MustNotBeGreaterThan(Second, (p, b) => new Exception($"{p} is not less than {b}."));

        [Benchmark]
        public long OldVersion() => First.OldMustNotBeGreaterThan(Second, nameof(First));
    }

    public static class MustNotBeGreaterThanExtensionMethods
    {
        public static T OldMustNotBeGreaterThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) <= 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but you specified {parameter}.");
        }
    }
}