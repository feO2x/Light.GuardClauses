using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustNotBeLessThanBenchmark : DefaultBenchmark
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
        public int LightGuardClausesWithParameterName() => First.MustNotBeLessThan(Second, nameof(First));

        [Benchmark]
        public int LightGuardClausesWithCustomException() => First.MustNotBeLessThan(Second, (x, y) => new Exception());

        [Benchmark]
        public int OldVersion() => First.OldMustNotBeLessThan(Second, nameof(First));
    }

    public static class MustNotBeLessThanExtensionMethods
    {
        public static T OldMustNotBeLessThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) >= 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be less than {boundary}, but you specified {parameter}.");
        }
    }
}