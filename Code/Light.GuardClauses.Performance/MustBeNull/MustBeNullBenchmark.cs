using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.MustBeNull
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class MustBeNullBenchmark
    {
        public static readonly SampleEntity NullReference = null;

        [Benchmark(Baseline = true)]
        public object BaseVersionWithParameterName()
        {
            if (NullReference != null)
                throw new ArgumentNotNullException(nameof(NullReference));

            return NullReference;
        }

        [Benchmark]
        public object LightGuardClausesWithParameterName() => NullReference.MustBeNull(nameof(NullReference));

        [Benchmark]
        public object LightGuardClausesWithCustomException() => NullReference.MustBeNull(() => new Exception("Foo"));

        [Benchmark]
        public object LightGuardClausesWithCustomParameterizedException() => NullReference.MustBeNull(value => new Exception($"{value} is not null."));
    }
}