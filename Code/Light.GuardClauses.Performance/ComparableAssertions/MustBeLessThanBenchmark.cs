using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustBeLessThanBenchmark : DefaultBenchmark
    {
        public double First = 42.7;
        public double Second = 78.21;

        [Benchmark(Baseline = true)]
        public double ImperativeVersion()
        {
            if (First >= Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public double ImperativeVersionWithCompareTo()
        {
            if (First.CompareTo(Second) >= 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public double LightGuardClausesWithParameterName() => First.MustBeLessThan(Second, nameof(First));

        [Benchmark]
        public double LightGuardClausesCustomException() => First.MustBeLessThan(Second, (p, b) => new Exception($"{p} is not less than {b}."));

        [Benchmark]
        public double OldVersion() => First.OldMustBeLessThan(Second, nameof(First));
    }

    public static class MustBeLessThanExtensionMethods
    {
        public static T OldMustBeLessThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but you specified {parameter}.");
        }
    }
}