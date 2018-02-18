using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeNullReferenceWithParameterNameBenchmarks : DefaultBenchmark
    {
        public readonly SampleEntity Instance = new SampleEntity(Guid.NewGuid());

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