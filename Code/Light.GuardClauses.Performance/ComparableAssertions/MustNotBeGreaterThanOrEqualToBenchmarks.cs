using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotBeGreaterThanOrEqualToBenchmarks
    {
        private static readonly double First = 42.7;
        private static readonly double Second = 78.21;

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
        public double LightGuardClausesWithParameterName() => First.MustNotBeGreaterThanOrEqualTo(Second, nameof(First));

        [Benchmark]
        public double LightGuardClausesWithCustomException() => First.MustNotBeGreaterThanOrEqualTo(Second, () => new Exception());

        [Benchmark]
        public double LightGuardClausesWithExceptionOneParameter() => First.MustNotBeGreaterThanOrEqualTo(Second, p => new Exception($"{p} is not less."));

        [Benchmark]
        public double LightGuardClausesWithExceptionTwoParameters() => First.MustNotBeGreaterThanOrEqualTo(Second, (p, b) => new Exception($"{p} is not less than {b}."));

        [Benchmark]
        public double OldVersion() => First.OldMustNotBeGreaterThanOrEqualTo(Second, nameof(First));
    }

    public static class MustNotBeGreaterThanOrEqualToExtensionMethods
    {
        public static T OldMustNotBeGreaterThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) < 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but you specified {parameter}.");
        }
    }
}