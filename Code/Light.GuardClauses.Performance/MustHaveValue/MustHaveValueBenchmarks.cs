using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.MustHaveValue
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustHaveValueBenchmarks
    {
        public static readonly int? Nullable = 42;

        [Benchmark(Baseline = true)]
        public int? BaseVersion()
        {
            if (Nullable.HasValue == false) throw new NullableHasNoValueException(nameof(Nullable));
            return Nullable;
        }

        [Benchmark]
        public int? OldVersionWithParameterName() => Nullable.OldMustHaveValue(nameof(Nullable));

        [Benchmark]
        public int? LightGuardClausesWithParameterName() => Nullable.MustHaveValue(nameof(Nullable));

        [Benchmark]
        public int? LightGuardClausesWithCustomException() => Nullable.MustHaveValue(() => new Exception());
    }

    public static class MustHaveValueExtensionMethods
    {
        public static T? OldMustHaveValue<T>(this T? parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
            where T : struct
        {
            if (parameter.HasValue)
                return parameter;

            throw exception?.Invoke() ?? (message != null ? new NullableHasNoValueException(message, parameterName) : new NullableHasNoValueException(parameterName));
        }
    }
}