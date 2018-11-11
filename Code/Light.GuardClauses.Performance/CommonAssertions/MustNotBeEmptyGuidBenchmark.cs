using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeEmptyGuidBenchmark
    {
        public Guid Guid = Guid.NewGuid();

        [Benchmark(Baseline = true)]
        public Guid BaseVersion()
        {
            if (Guid == Guid.Empty) throw new EmptyGuidException(nameof(Guid));
            return Guid;
        }

        [Benchmark]
        public Guid LightGuardClausesWithParameterName() => Guid.MustNotBeEmpty(nameof(Guid));

        [Benchmark]
        public Guid LightGuardClausesWithCustomException() => Guid.MustNotBeEmpty(() => new Exception());

        [Benchmark]
        public Guid OldVersion() => Guid.OldMustNotBeEmpty(nameof(Guid));
    }

    public static class GuidMustNotBeEmptyExtensionMethods
    {
        public static Guid OldMustNotBeEmpty(this Guid parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter != Guid.Empty)
                return parameter;
            throw exception?.Invoke() ?? new EmptyGuidException(parameterName, message ?? $"{parameterName ?? "The value"} must be a valid GUID, but it actually is an empty one.");
        }
    }
}