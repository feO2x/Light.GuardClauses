using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance
{
    [ClrJob, CoreJob]
    [MemoryDiagnoser]
    public class MustNotBeNullWithCustomException
    {
        public static readonly object Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public object BaseVersion()
        {
            if (Instance == null)
                throw new ArgumentException("Instance must not be null.", nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public object MustNotBeNullV1() => Instance.MustNotBeNullV1(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object MustNotBeNullV2() => Instance.MustNotBeNullV2(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object MustNotBeNullV5() => Instance.MustNotBeNullV5(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object MustNotBeNullV6()
        {
            Instance.MustNotBeNullV6(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));
            return Instance;
        }

        [Benchmark]
        public object MustNotBeNullV7() => Instance.MustNotBeNullV7(exception: () => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object MustNotBeNullV9() => Instance.MustNotBeNullV9(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object MustNotBeNullV11() => Instance.MustNotBeNullV11(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object MustNotBeNullV12() => Instance.MustNotBeNullV12(() => new ArgumentException("Instance must not be null.", nameof(Instance)));

        [Benchmark]
        public object LightGuardClauses() => Instance.MustNotBeNull(() => new ArgumentException("Instance must not be null.", nameof(Instance)));
    }
}