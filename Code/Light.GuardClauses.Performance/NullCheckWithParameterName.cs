using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance
{
    [CoreJob, ClrJob]
    [MemoryDiagnoser]
    public class NullCheckWithParameterName
    {
        public static readonly object Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public object BaseVersion()
        {
            if (Instance == null)
                throw new ArgumentNullException(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public object MustNotBeNullV1() => Instance.MustNotBeNullV1(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV2() => Instance.MustNotBeNullV2(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV3() => Instance.MustNotBeNullV3(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV4() => Instance.MustNotBeNullV4(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV5() => Instance.MustNotBeNullV5(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV6()
        {
            Instance.MustNotBeNullV6(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public object MustNotBeNullV7() => Instance.MustNotBeNullV7(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV8() => Instance.MustNotBeNullV8(nameof(Instance));

        [Benchmark]
        public object MustNotBeNullV10() => Instance.MustNotBeNullV10(nameof(Instance));

        [Benchmark]
        public object LightGuardClausesVersion() => Instance.MustNotBeNull(nameof(Instance));
    }
}