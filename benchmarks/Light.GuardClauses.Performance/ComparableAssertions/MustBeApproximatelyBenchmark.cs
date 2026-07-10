using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions;

[MemoryDiagnoser]
// ReSharper disable once ClassCanBeSealed.Global -- Benchmark.NET derives from this class with dynamically created code
public class MustBeApproximatelyBenchmark
{
    [Benchmark]
    public double MustBeApproximately() => 5.100001.MustBeApproximately(5.100000, 0.0001);

    [Benchmark(Baseline = true)]
    public double Imperative() => Imperative(5.100001, 5.100000, 0.0001);

    private static double Imperative(double parameter, double other, double tolerance)
    {
        if (Math.Abs(parameter - other) > tolerance)
        {
            throw new ArgumentOutOfRangeException(nameof(parameter), $"Value is not approximately {other}");
        }

        return parameter;
    }
}
