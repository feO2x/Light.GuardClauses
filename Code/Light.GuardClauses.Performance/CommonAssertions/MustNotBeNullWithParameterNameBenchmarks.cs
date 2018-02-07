using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    [CoreJob, ClrJob]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class MustNotBeNullWithParameterNameBenchmarks
    {
        public readonly SampleEntity Instance = new SampleEntity(Guid.NewGuid());

        [Benchmark(Baseline = true)]
        public SampleEntity BaseVersionWithIf()
        {
            if (Instance == null)
                throw new ArgumentNullException(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity BaseVersionWithNullcoalescing() => Instance ?? throw new ArgumentNullException(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV1() => Instance.MustNotBeNullV1(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV2() => Instance.MustNotBeNullV2(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV3() => Instance.MustNotBeNullV3(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV4() => Instance.MustNotBeNullV4(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV5() => Instance.MustNotBeNullV5(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV6()
        {
            Instance.MustNotBeNullV6(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity MustNotBeNullV7() => Instance.MustNotBeNullV7(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV8() => Instance.MustNotBeNullV8(nameof(Instance));

        [Benchmark]
        public SampleEntity MustNotBeNullV10() => Instance.MustNotBeNullV10(nameof(Instance));

        [Benchmark]
        public SampleEntity LightGuardClausesVersion() => Instance.MustNotBeNull(nameof(Instance));
    }
}