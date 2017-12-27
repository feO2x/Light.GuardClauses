using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance
{
    [CoreJob, ClrJob]
    [MemoryDiagnoser]
    public class IsSubsetOfGist
    {
        [Params(10, 100, 1000, 10_000, 100_000, 1_000_000)]
        public int NumberOfItems;

        public int[] SubsetArray;
        public int[] SupersetArray;

        [GlobalSetup]
        public void GlobalArraySetup()
        {
            SupersetArray = new int[NumberOfItems];
            for (var i = 0; i < NumberOfItems; i++)
            {
                SupersetArray[i] = i + 1;
            }

            SubsetArray = new int[5];
            for (var i = 0; i < 5; i++)
            {
                SubsetArray[i] = SupersetArray[SupersetArray.Length - 1 - i];
            }
        }

        [Benchmark(Baseline = true)]
        public bool ArrayWithForLoops() => IsSubsetOfWithForLoops(SubsetArray, SupersetArray);

        private static bool IsSubsetOfWithForLoops<T>(T[] set, T[] superset, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;

            for (var i = 0; i < set.Length; i++)
            {
                var wasItemFound = false;
                for (var j = 0; j < superset.Length; j++)
                {
                    if (!comparer.Equals(superset[j], set[i]))
                        continue;

                    wasItemFound = true;
                    break;
                }

                if (!wasItemFound)
                    return false;
            }

            return true;
        }

        [Benchmark]
        public bool ArrayWithLinq() => SubsetArray.All(i => SupersetArray.Contains(i, null));

        [Benchmark]
        public bool ArrayWithForeachLoops() => IsSubsetOfWithForeachLoops(SubsetArray, SupersetArray);

        private static bool IsSubsetOfWithForeachLoops<T>(T[] set, T[] superset, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            foreach (var itemFromSet in set)
            {
                var wasItemFound = false;
                foreach (var itemFromSuperset in superset)
                {
                    if (!comparer.Equals(itemFromSet, itemFromSuperset))
                        continue;

                    wasItemFound = true;
                    break;
                }

                if (!wasItemFound)
                    return false;
            }

            return true;
        }
    }
}