using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeDoubleBenchmark
    {
        public EqualityComparer<double> EqualityComparer = EqualityComparer<double>.Default;
        public double NumberA = 42.85;
        public double NumberB = -1592.642;

        [Benchmark(Baseline = true)]
        public double Imperative()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (NumberA == NumberB)
                throw new ValuesEqualException(nameof(NumberA), "The two numbers are equal.");

            return NumberA;
        }

        [Benchmark]
        public double LightGuardClauses() => NumberA.MustNotBe(NumberB, nameof(NumberA));

        [Benchmark]
        public double LightGuardClausesCustomEqualityComparer() => NumberA.MustNotBe(NumberB, EqualityComparer, nameof(NumberA));

        [Benchmark]
        public double LightGuardClausesCustomException() => NumberA.MustNotBe(NumberB, (x, y) => new Exception($"{x} and {y} are equal."));

        [Benchmark]
        public double LightGuardClausesCustomExceptionCustomEqualityComparer() => NumberA.MustNotBe(NumberB, EqualityComparer, (x, y, _) => new Exception($"{x} and {y} are equal."));
    }

    public class MustNotBeGuidBenchmark
    {
        public EqualityComparer<Guid> EqualityComparer = EqualityComparer<Guid>.Default;
        public Guid GuidA = Guid.NewGuid();
        public Guid GuidB = Guid.NewGuid();

        [Benchmark(Baseline = true)]
        public Guid Imperative()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (GuidA == GuidB)
                throw new ValuesEqualException(nameof(GuidA), "The two GUIDs are equal.");

            return GuidA;
        }

        [Benchmark]
        public Guid LightGuardClauses() => GuidA.MustNotBe(GuidB, nameof(GuidA));

        [Benchmark]
        public Guid LightGuardClausesCustomEqualityComparer() => GuidA.MustNotBe(GuidB, EqualityComparer, nameof(GuidA));

        [Benchmark]
        public Guid LightGuardClausesCustomException() => GuidA.MustNotBe(GuidB, (x, y) => new Exception($"{x} and {y} are equal."));

        [Benchmark]
        public Guid LightGuardClausesCustomExceptionCustomEqualityComparer() => GuidA.MustNotBe(GuidB, EqualityComparer, (x, y, _) => new Exception($"{x} and {y} are equal."));
    }

    public class MustNotBeStringBenchmark
    {
        public EqualityComparer<string> EqualityComparer = EqualityComparer<string>.Default;
        public string StringA = "The things I do for love.";
        public string StringB = "The things I do for you.";

        [Benchmark(Baseline = true)]
        public string Imperative()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (StringA == StringB)
                throw new ValuesEqualException(nameof(StringA), "The two strings are equal.");

            return StringA;
        }

        [Benchmark]
        public string LightGuardClauses() => StringA.MustNotBe(StringB, nameof(StringA));

        [Benchmark]
        public string LightGuardClausesCustomEqualityComparer() => StringA.MustNotBe(StringB, EqualityComparer, nameof(StringA));

        [Benchmark]
        public string LightGuardClausesCustomException() => StringA.MustNotBe(StringB, (x, y) => new Exception($"{x} and {y} are equal."));

        [Benchmark]
        public string LightGuardClausesCustomExceptionCustomEqualityComparer() => StringA.MustNotBe(StringB, EqualityComparer, (x, y, _) => new Exception($"{x} and {y} are equal."));
    }
}