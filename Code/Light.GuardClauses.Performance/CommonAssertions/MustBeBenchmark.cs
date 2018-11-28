using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustBeNumberBenchmark
    {
        public int NumberA = 284930;
        public int NumberB;
        public EqualityComparer<int> EqualityComparer = EqualityComparer<int>.Default;

        public MustBeNumberBenchmark() => NumberB = NumberA;

        [Benchmark(Baseline = true)]
        public int Imperative()
        {
            if (NumberA != NumberB)
                throw new ValuesNotEqualException(nameof(NumberA), "The two numbers are not equal");
            return NumberA;
        }

        [Benchmark]
        public int LightGuardClauses() => NumberA.MustBe(NumberB, nameof(NumberA));

        [Benchmark]
        public int LightGuardClausesCustomEqualityComparer() => NumberA.MustBe(NumberB, EqualityComparer, nameof(NumberA));

        [Benchmark]
        public int LightGuardClausesCustomException() => NumberA.MustBe(NumberB, (x, y) => new Exception($"{x} and {y} are not equal"));

        [Benchmark]
        public int LightGuardClausesCustomExceptionCustomEqualityComparer() => NumberA.MustBe(NumberB, EqualityComparer, (x, y, comparer) => new Exception($"{x} and {y} are not equal"));
    }

    public class MustBeGuidBenchmark
    {
        public Guid GuidA = Guid.NewGuid();
        public Guid GuidB;
        public EqualityComparer<Guid> EqualityComparer = EqualityComparer<Guid>.Default;

        public MustBeGuidBenchmark() => GuidB = GuidA;

        [Benchmark(Baseline = true)]
        public Guid Imperative()
        {
            if (GuidA != GuidB)
                throw new ValuesNotEqualException(nameof(GuidA), "The two GUIDs are not equal");
            return GuidA;
        }

        [Benchmark]
        public Guid LightGuardClauses() => GuidA.MustBe(GuidB, nameof(GuidA));

        [Benchmark]
        public Guid LightGuardClausesCustomEqualityComparer() => GuidA.MustBe(GuidB, EqualityComparer, nameof(GuidA));

        [Benchmark]
        public Guid LightGuardClausesCustomException() => GuidA.MustBe(GuidB, (x, y) => new Exception($"{x} and {y} are not equal"));

        [Benchmark]
        public Guid LightGuardClausesCustomExceptionCustomEqualityComparer() => GuidA.MustBe(GuidB, EqualityComparer, (x, y, comparer) => new Exception($"{x} and {y} are not equal"));
    }

    public class MustBeStringBenchmark
    {
        public string StringA = "This is a relatively long string";
        public string StringB;
        public EqualityComparer<string> EqualityComparer = EqualityComparer<string>.Default;

        public MustBeStringBenchmark() => StringB = new string(StringA.ToCharArray());

        [Benchmark(Baseline = true)]
        public string Imperative()
        {
            if (StringA != StringB)
                throw new ValuesNotEqualException(nameof(StringA), "The two strings are not equal");
            return StringA;
        }

        [Benchmark]
        public string LightGuardClauses() => StringA.MustBe(StringB, nameof(StringA));

        [Benchmark]
        public string LightGuardClausesCustomEqualityComparer() => StringA.MustBe(StringB, EqualityComparer, nameof(StringA));

        [Benchmark]
        public string LightGuardClausesCustomException() => StringA.MustBe(StringB, (x, y) => new Exception($"{x} and {y} are not equal"));

        [Benchmark]
        public string LightGuardClausesCustomExceptionCustomEqualityComparer() => StringA.MustBe(StringB, EqualityComparer, (x, y, comparer) => new Exception($"{x} and {y} are not equal"));
    }
}