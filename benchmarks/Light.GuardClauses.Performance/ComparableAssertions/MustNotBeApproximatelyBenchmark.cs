using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions;

[MemoryDiagnoser]
// ReSharper disable once ClassCanBeSealed.Global -- Benchmark.NET derives from this class with dynamically created code
public class MustNotBeApproximatelyBenchmark
{
    [Benchmark]
    public double MustNotBeApproximately() => 5.2.MustNotBeApproximately(5.1, 0.5);

    [Benchmark(Baseline = true)]
    public double Imperative() => Imperative(5.2, 5.1, 0.5);

    private static double Imperative(double parameter, double other, double tolerance)
    {
        if (Math.Abs(parameter - other) < tolerance)
        {
            throw new ArgumentOutOfRangeException(nameof(parameter), $"Value is approximately {other}");
        }

        return parameter;
    }
}