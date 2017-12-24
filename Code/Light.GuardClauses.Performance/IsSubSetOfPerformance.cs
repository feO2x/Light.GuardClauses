using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance
{
    [CoreJob, ClrJob]
    [MemoryDiagnoser]
    public class IsSubSetOfPerformance
    {
        public int[] Array;
        public List<int> List;

        [Params(10, 100, 1000, 10_000, 100_000, 1_000_000)]
        public int NumberOfItems;

        public int[] SubSet;

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

            SubSet = new int[5];
            for (var i = 0; i < 5; i++)
            {
                SubSet[i] = Array[Array.Length - 1 - i];
            }
        }

        [Benchmark]
        public object IsSubSetOfWithArray()
        {
            return SubSet.IsSubsetOf(Array);
        }
    }
}