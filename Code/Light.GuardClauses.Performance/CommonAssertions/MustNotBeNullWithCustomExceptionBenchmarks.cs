using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotBeNullWithCustomExceptionBenchmarks
    {
        public static readonly SampleEntity Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public SampleEntity BaseVersion()
        {
            if (Instance == null)
                throw new ArgumentException("Instance must not be null.", nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity MustNotBeNullV1() => Instance.MustNotBeNullV1(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity MustNotBeNullV2() => Instance.MustNotBeNullV2(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity MustNotBeNullV5() => Instance.MustNotBeNullV5(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity MustNotBeNullV6()
        {
            Instance.MustNotBeNullV6(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));
            return Instance;
        }

        [Benchmark]
        public SampleEntity MustNotBeNullV7() => Instance.MustNotBeNullV7(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity MustNotBeNullV9() => Instance.MustNotBeNullV9(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity MustNotBeNullV11() => Instance.MustNotBeNullV11(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity MustNotBeNullV12() => Instance.MustNotBeNullV12(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public SampleEntity LightGuardClauses() => Instance.MustNotBeNull(() => new ArgumentException("Instance must not be null.", nameof(Instance)));
    }
}