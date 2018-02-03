using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustBeNullBenchmarks
    {
        public static readonly SampleEntity NullReference = null;

        [Benchmark(Baseline = true)]
        public SampleEntity BaseVersionWithParameterName()
        {
            if (NullReference != null)
                throw new ArgumentNotNullException(nameof(NullReference));

            return NullReference;
        }

        [Benchmark]
        public SampleEntity LightGuardClausesWithParameterName() => NullReference.MustBeNull(nameof(NullReference));

        [Benchmark]
        public SampleEntity LightGuardClausesWithCustomException() => NullReference.MustBeNull(() => new Exception("Foo"));

        [Benchmark]
        public SampleEntity LightGuardClausesWithCustomParameterizedException() => NullReference.MustBeNull(value => new Exception($"{value} is not null."));
    }
}