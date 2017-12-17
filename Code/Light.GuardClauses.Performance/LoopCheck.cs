using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

// ReSharper disable UnusedVariable

namespace Light.GuardClauses.Performance
{
    [CoreJob, ClrJob]
    [LegacyJitX86Job, LegacyJitX64Job, RyuJitX64Job]
    [MemoryDiagnoser]
    public class LoopCheck
    {
        public int[] Array;
        public List<int> List;

        [Params(10, 100, 1000, 10_000, 100_000, 1_000_000)] public int NumberOfItems;

        public IReadOnlyList<int> ReadOnlyList => List;
        public IEnumerable<int> Enumerable => List;

        [GlobalSetup]
        public void GlobalSetup()
        {
            var random = new Random();

            Array = new int[NumberOfItems];
            for (var i = 0; i < NumberOfItems; i++)
            {
                Array[i] = random.Next();
            }

            List = new List<int>(Array);
        }

        [Benchmark(Baseline = true)]
        public object ArrayForLoop()
        {
            for (var i = 0; i < Array.Length; i++)
            {
                var element = Array[i];
            }
            return Array;
        }

        [Benchmark]
        public object ArrayForEachLoop()
        {
            foreach (var element in Array) { }
            return Array;
        }

        [Benchmark]
        public object ListForLoop()
        {
            for (var i = 0; i < List.Count; i++)
            {
                var element = List[i];
            }
            return List;
        }

        [Benchmark]
        public object ListForEachLoop()
        {
            foreach (var element in List) { }
            return List;
        }

        [Benchmark]
        public object ReadOnlyListForLoop()
        {
            var list = ReadOnlyList;

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
            }
            return list;
        }

        [Benchmark]
        public object EnumerableForEachLoop()
        {
            var enumerable = Enumerable;

            foreach (var element in enumerable) { }
            return enumerable;
        }
    }
}