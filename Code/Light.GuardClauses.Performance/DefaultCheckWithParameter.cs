using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class DefaultCheckWithParameter
    {
        public static readonly SampleEntity Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark]
        public object MustNotBeDefaultValueV1() => 4.MustNotBeDefaultV1("parameter");

        [Benchmark]
        public object MustNotBeDefaultValueV2() => 89.MustNotBeDefaultV2("parameter");

        [Benchmark]
        public object MustNotBeDefaultReferenceV1() => Instance.MustNotBeDefaultV1(nameof(Instance));

        [Benchmark]
        public object MustNotBeDefaultReferenceV2() => Instance.MustNotBeDefaultV2(nameof(Instance));
    }
}