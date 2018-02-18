using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeDefaultBenchmarks : DefaultBenchmark
    {
        public readonly SampleEntity Reference = new SampleEntity(Guid.NewGuid());
        public readonly int Value = 42;

        [Benchmark(Baseline = true)]
        public int BaseVersionForValueType()
        {
            if (Value == default(int)) throw new ArgumentDefaultException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int MustNotBeDefaultValueV1() => Value.MustNotBeDefaultV1("parameter");

        [Benchmark]
        public int MustNotBeDefaultValueV2() => Value.MustNotBeDefaultV2("parameter");

        [Benchmark]
        public int OldVersionForValueType() => Value.OldMustNotBeDefault(nameof(Value));

        [Benchmark]
        public SampleEntity BaseVersionForReferenceType()
        {
            if (Reference == null) throw new ArgumentNullException(nameof(Reference));
            return Reference;
        }

        [Benchmark]
        public SampleEntity MustNotBeDefaultReferenceV1() => Reference.MustNotBeDefaultV1(nameof(Reference));

        [Benchmark]
        public SampleEntity MustNotBeDefaultReferenceV2() => Reference.MustNotBeDefaultV2(nameof(Reference));

        [Benchmark]
        public SampleEntity OldVersionForReferenceType() => Reference.OldMustNotBeDefault(nameof(Reference));
    }
}