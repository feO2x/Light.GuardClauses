using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.MustBeFalse
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class MustBeFalseBenchmarks
    {
        public static readonly bool False = false;

        [Benchmark(Baseline = true)]
        public bool BaseVersion()
        {
            if (False) throw new ArgumentException(null, nameof(False));
            return False;
        }

        [Benchmark]
        public bool LightGuardClausesWithParameterName() => False.MustBeFalse(nameof(False));

        [Benchmark]
        public bool LightGuardClausesWithCustomException() => False.MustBeFalse(() => new Exception());

        [Benchmark]
        public bool OldVersion() => False.OldMustBeFalse(nameof(False));
    }

    public static class MustBeFalseExtensionMethods
    {
        public static bool OldMustBeFalse(this bool parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter == false)
                return false;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The value"} must be false, but you specified true.", parameterName);
        }
    }
}
