using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotHaveValueDoubleBenchmarks
    {
        public static readonly double? Nullable = null;

        [Benchmark(Baseline = true)]
        public double? BaseVersion()
        {
            if (Nullable.HasValue) throw new NullableHasValueException(nameof(Nullable));
            return Nullable;
        }

        [Benchmark]
        public double? LightGuardClausesWithParameterName() => Nullable.MustNotHaveValue(nameof(Nullable));

        [Benchmark]
        public double? LightGuardClausesWithCustomException() => Nullable.MustNotHaveValue(() => new Exception());

        [Benchmark]
        public double? LightGuardClausesWithCustomParameterizedException() => Nullable.MustNotHaveValue(v => new Exception($"Nullable is not null, but {v}."));

        [Benchmark]
        public double? OldVersion() => Nullable.OldMustNotHaveValue(nameof(Nullable));
    }

    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotHaveValueIntBenchmarks
    {
        public static readonly int? Nullable = null;

        [Benchmark(Baseline = true)]
        public int? BaseVersion()
        {
            if (Nullable.HasValue) throw new NullableHasValueException(nameof(Nullable));
            return Nullable;
        }

        [Benchmark]
        public int? LightGuardClausesWithParameterName() => Nullable.MustNotHaveValue(nameof(Nullable));

        [Benchmark]
        public int? LightGuardClausesWithCustomException() => Nullable.MustNotHaveValue(() => new Exception());

        [Benchmark]
        public int? LightGuardClausesWithCustomParameterizedException() => Nullable.MustNotHaveValue(v => new Exception($"Nullable is not null, but {v}."));

        [Benchmark]
        public int? OldVersion() => Nullable.OldMustNotHaveValue(nameof(Nullable));
    }

    public static class MustNotHaveValueExtensionMethods
    {
        public static T? OldMustNotHaveValue<T>(this T? parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
            where T : struct
        {
            if (parameter.HasValue == false)
                return null;

            throw exception?.Invoke() ?? throw new NullableHasValueException(parameterName, message ?? $"{parameterName ?? "The nullable"} must have no value, but it actually is \"{parameter.Value}\".");
        }
    }
}