using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotBeNullReferenceWithParameterNameBenchmarks
    {
        public static readonly SampleEntity Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public int BaseValueType() => 42;

        [Benchmark]
        public SampleEntity BaseReference()
        {
            if (Instance == null) throw new ArgumentNullException(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity ReferenceV1() => Instance.MustNotBeNullReferenceV1(nameof(Instance));

        [Benchmark]
        public SampleEntity ReferenceV2() => Instance.MustNotBeNullReferenceV2(nameof(Instance));

        [Benchmark]
        public int ValueTypeV1() => 42.MustNotBeNullReferenceV1("Fourty Two");

        [Benchmark]
        public int ValueTypeV2() => 42.MustNotBeNullReferenceV2("Fourty Two");
    }
}