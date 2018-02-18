using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustBeGreaterThanBenchmarks : DefaultBenchmark
    {
        public readonly int First = 5000;
        public readonly int Second = -1939;

        [Benchmark(Baseline = true)]
        public int BaseVersion()
        {
            if (First <= Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public int BaseVersionWithCompareTo()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            if (First.CompareTo(Second) <= 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public int LightGuardClausesWithParameterName() => First.MustBeGreaterThan(Second, nameof(First));

        [Benchmark]
        public int LightGuardClausesWithCustomException() => First.MustBeGreaterThan(Second, () => new Exception());

        [Benchmark]
        public int LightGuardClausesWithExceptionOneParameter() => First.MustBeGreaterThan(Second, p => new Exception($"{p} is not less."));

        [Benchmark]
        public int LightGuardClausesWithExceptionTwoParameters() => First.MustBeGreaterThan(Second, (p, b) => new Exception($"{p} is not less than {b}."));

        [Benchmark]
        public int OldVersion() => First.OldMustBeGreaterThan(Second, nameof(First));
    }

    public static class MustBeGreaterThanExtensionMethods
    {
        public static T OldMustBeGreaterThan<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but you specified {parameter}.");
        }
    }
}