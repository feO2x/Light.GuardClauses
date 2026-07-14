using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Light.GuardClauses.Performance.CollectionAssertions;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class CollectionContentGuardBenchmark
{
    public List<string> Collection = ["Alpha", "Beta", "Gamma", "Delta"];

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(nameof(MustNotContainNull))]
    public List<string> ImperativeNullCheck()
    {
        for (var i = 0; i < Collection.Count; ++i)
        {
            if (Collection[i] is null)
            {
                throw new InvalidOperationException();
            }
        }

        return Collection;
    }

    [Benchmark]
    [BenchmarkCategory(nameof(MustNotContainNull))]
    public List<string> MustNotContainNull() => Collection.MustNotContainNull();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(nameof(MustNotContainNullOrWhiteSpace))]
    public List<string> ImperativeNullOrWhiteSpaceCheck()
    {
        for (var i = 0; i < Collection.Count; ++i)
        {
            if (string.IsNullOrWhiteSpace(Collection[i]))
            {
                throw new InvalidOperationException();
            }
        }

        return Collection;
    }

    [Benchmark]
    [BenchmarkCategory(nameof(MustNotContainNullOrWhiteSpace))]
    public List<string> MustNotContainNullOrWhiteSpace() => Collection.MustNotContainNullOrWhiteSpace();
}
