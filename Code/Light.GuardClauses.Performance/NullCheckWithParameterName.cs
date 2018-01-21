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
        public object V1() => Instance.V1(nameof(Instance));

        [Benchmark]
        public object V2() => Instance.V2(nameof(Instance));

        [Benchmark]
        public object V3() => Instance.V3(nameof(Instance));

        [Benchmark]
        public object V4() => Instance.V4(nameof(Instance));

        [Benchmark]
        public object V5() => Instance.V5(nameof(Instance));

        [Benchmark]
        public object V6()
        {
            Instance.V6(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public object V7() => Instance.V7(nameof(Instance));
    }

    public class SampleEntity
    {
        public readonly Guid Id;

        public SampleEntity(Guid id)
        {
            Id = id.MustNotBeEmpty(nameof(id));
        }
    }
}