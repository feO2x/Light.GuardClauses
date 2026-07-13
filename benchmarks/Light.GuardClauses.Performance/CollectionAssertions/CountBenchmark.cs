using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions;

public class ListCountBenchmark
{
    public List<int> List;
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [Params(100, 1000)]
    public int NumberOfItems { get; set; }

    [GlobalSetup]
    public void Setup() => List = Enumerable.Range(1, NumberOfItems).ToList();

    // ReSharper disable once UseCollectionCountProperty
    [Benchmark(Baseline = true)]
    public int LinqCount() => List.Count();

    [Benchmark]
    public int ExistingEnumerableCount() => EnumerableExtensions.Count(List);

    [Benchmark]
    public int NewEnumerableOfTGetCount() => List.GetCount();
}

public class StringCountBenchmark
{
    public string String;
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [Params(100, 1000)]
    public int NumberOfItems { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        var array = new char[NumberOfItems];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = (char) random.Next('a', 'z' + 1);
        }

        String = new (array);
    }

    // ReSharper disable once UseCollectionCountProperty
    [Benchmark(Baseline = true)]
    public int LinqCount() => String.Count();

    [Benchmark]
    public int ExistingEnumerableCount() => EnumerableExtensions.Count(String);

    [Benchmark]
    public int NewEnumerableOfTGetCount() => String.GetCount();
}

public class MyImmutableArrayCountBenchmark
{
    public MyImmutableArray<int> MyArray;
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [Params(100, 1000)]
    public int NumberOfItems { get; set; }
    
    [GlobalSetup]
    public void Setup() => MyArray = new (Enumerable.Range(1, NumberOfItems).ToArray());
    
    // ReSharper disable once UseCollectionCountProperty
    [Benchmark(Baseline = true)]
    public int LinqCount() => MyArray.Count();

    [Benchmark]
    public int ExistingEnumerableCount() => EnumerableExtensions.Count(MyArray);

    [Benchmark]
    public int NewEnumerableOfTGetCount() => MyArray.GetCount();
}

public sealed class MyImmutableArray<T> : IReadOnlyList<T>
{
    private readonly T[] _array;
    
    public MyImmutableArray(T[] array) => _array = array;

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) _array).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();

    public int Count => _array.Length;

    public T this[int index] => _array[index];
}
