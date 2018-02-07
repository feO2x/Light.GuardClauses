using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustBeLessThanBenchmarks
    {
        public readonly double First = 42.7;
        public readonly double Second = 78.21;

        [Benchmark(Baseline = true)]
        public double BaseVersion()
        {
            if (First >= Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public double BaseVersionWithCompareTo()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            if (First.CompareTo(Second) >= 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public double LightGuardClausesWithParameterName() => First.MustBeLessThan(Second, nameof(First));

        [Benchmark]
        public double LightGuardClausesWithCustomException() => First.MustBeLessThan(Second, () => new Exception());

        [Benchmark]
        public double LightGuardClausesWithExceptionOneParameter() => First.MustBeLessThan(Second, p => new Exception($"{p} is not less."));

        [Benchmark]
        public double LightGuardClausesWithExceptionTwoParameters() => First.MustBeLessThan(Second, (p, b) => new Exception($"{p} is not less than {b}."));

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