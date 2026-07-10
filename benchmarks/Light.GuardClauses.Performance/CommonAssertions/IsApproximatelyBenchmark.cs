using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class IsApproximatelyDoubleBenchmark
    {
        public double DoubleX = 35.5;
        public double DoubleY = 35.8;
        public double DoubleZ = 35.5000000121;
        public double CustomTolerance = 0.01;

        [Benchmark(Baseline = true)]
        public bool ImperativeConstantToleranceFalse() => Math.Abs(DoubleX - DoubleY) < 0.0001;

        [Benchmark]
        public bool ImperativeConstantToleranceTrue() => Math.Abs(DoubleX - DoubleZ) < 0.0001;

        [Benchmark]
        public bool ImperativeCustomToleranceFalse() => Math.Abs(DoubleX - DoubleY) < CustomTolerance;

        [Benchmark]
        public bool ImperativeCustomToleranceTrue() => Math.Abs(DoubleX - DoubleZ) < CustomTolerance;

        [Benchmark]
        public bool LightGuardClausesConstantToleranceFalse() => DoubleX.IsApproximately(DoubleY);

        [Benchmark]
        public bool LightGuardClausesConstantToleranceTrue() => DoubleX.IsApproximately(DoubleZ);

        [Benchmark]
        public bool LightGuardClausesCustomToleranceFalse() => DoubleX.IsApproximately(DoubleY, CustomTolerance);

        [Benchmark]
        public bool LightGuardClausesCustomToleranceTrue() => DoubleX.IsApproximately(DoubleZ, CustomTolerance);
    }

    public class IsApproximatelyFloatBenchmark
    {
        public float FloatX = 17.99f;
        public float FloatY = 18.01f;
        public float FloatZ = 17.9999f;
        public float CustomTolerance = 0.0001f;

        [Benchmark(Baseline = true)]
        public bool ImperativeConstantToleranceFalse() => Math.Abs(FloatX - FloatY) < 0.0001f;

        [Benchmark]
        public bool ImperativeConstantToleranceTrue() => Math.Abs(FloatX - FloatZ) < 0.0001f;

        [Benchmark]
        public bool ImperativeCustomToleranceFalse() => Math.Abs(FloatX - FloatY) < CustomTolerance;

        [Benchmark]
        public bool ImperativeCustomToleranceTrue() => Math.Abs(FloatX - FloatZ) < CustomTolerance;

        [Benchmark]
        public bool LightGuardClausesConstantToleranceFalse() => FloatX.IsApproximately(FloatY);

        [Benchmark]
        public bool LightGuardClausesConstantToleranceTrue() => FloatX.IsApproximately(FloatZ);

        [Benchmark]
        public bool LightGuardClausesCustomToleranceFalse() => FloatX.IsApproximately(FloatY, CustomTolerance);

        [Benchmark]
        public bool LightGuardClausesCustomToleranceTrue() => FloatX.IsApproximately(FloatZ, CustomTolerance);
    }
}