using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustBeGreaterThanOrEqualToBenchmark : DefaultBenchmark
    {
        public int First = 42;
        public int Second = 3;

        [Benchmark(Baseline = true)]
        public int ImperativeVersion()
        {
            if (First < Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public int ImperativeVersionWithCompareTo()
        {
            if (First.CompareTo(Second) < 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public int LightGuardClausesWithParameterName() => First.MustBeGreaterThanOrEqualTo(Second, nameof(First));

        [Benchmark]
        public double LightGuardClausesWithExceptionTwoParameters() => First.MustBeGreaterThanOrEqualTo(Second, (p, b) => new Exception($"{p} is not greater than or equal to {b}."));

        [Benchmark]
        public int OldVersion() => First.OldMustBeGreaterThanOrEqualTo(Second, nameof(First));
    }

    public static class MustBeGreaterThanOrEqualToExtensionMethods
    {
        public static T OldMustBeGreaterThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but you specified {parameter}.");
        }
    }
}