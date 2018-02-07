using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustBeTrueBenchmarks
    {
        public readonly bool True = true;

        [Benchmark(Baseline = true)]
        public bool BaseVersion()
        {
            if (!True) throw new ArgumentException(null, nameof(True));
            return True;
        }

        [Benchmark]
        public bool LightGuardClausesWithParameterName() => True.MustBeTrue(nameof(True));

        [Benchmark]
        public bool LightGuardClausesWithCustomException() => True.MustBeTrue(() => new Exception());

        [Benchmark]
        public bool OldVersion() => True.OldMustBeTrue(nameof(True));
    }

    public static class MustBeTrueExtensionMethods
    {
        public static bool OldMustBeTrue(this bool parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter)
                return true;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must be true, but you specified false.", parameterName);
        }
    }
}
