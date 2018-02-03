using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotBeLessThanBenchmarks
    {
        private static readonly int First = 42;
        private static readonly int Second = 3;

        [Benchmark(Baseline = true)]
        public int BaseVersion()
        {
            if (First < Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public int BaseVersionWithCompareTo()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            if (First.CompareTo(Second) < 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public int LightGuardClausesWithParameterName() => First.MustNotBeLessThan(Second, nameof(First));

        [Benchmark]
        public int LightGuardClausesWithCustomException() => First.MustNotBeLessThan(Second, () => new Exception());

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