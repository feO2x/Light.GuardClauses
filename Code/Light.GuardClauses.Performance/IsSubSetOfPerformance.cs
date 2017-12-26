using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace Light.GuardClauses.Performance
{
    [CoreJob, ClrJob]
    [MemoryDiagnoser]
    public class IsSubsetOfPerformance
    {
        [Params(10, 100, 1000, 10_000, 100_000, 1_000_000)]
        public int NumberOfItems;

        public int[] SubsetArray;
        public int[] SupersetArray;
        public List<int> SubsetList;
        public List<int> SupersetList;

        [GlobalSetup(Target = nameof(ArrayWithForLoops) + "," + nameof(LightGuardClausesArrayWithForLoops) + "," + nameof(ArrayWithLinq) + "," + nameof(ArrayWithForeachLoops))]
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
        public bool ArrayWithForLoops() => ArrayWithForLoops(SubsetArray, SupersetArray);

        private static bool ArrayWithForLoops<T>(T[] array, T[] superset, IEqualityComparer<T> comparer = null)
        {
            array.MustNotBeNull(nameof(array));
            superset.MustNotBeNull(nameof(superset));
            comparer = comparer ?? EqualityComparer<T>.Default;

            for (var i = 0; i < array.Length; i++)
            {
                var currentItem = array[i];
                var wasItemFound = false;
                for (var j = 0; j < superset.Length; j++)
                {
                    if (!comparer.Equals(superset[i], currentItem))
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
        public bool LightGuardClausesArrayWithForLoops() => SubsetArray.IsSubsetOf(SupersetArray);

        [Benchmark]
        public bool ArrayWithLinq() => Linq(SubsetArray, SupersetArray);

        private static bool Linq<T>(IEnumerable<T> set, IEnumerable<T> superset)
        {
            // ReSharper disable PossibleMultipleEnumeration
            set.MustNotBeNull(nameof(set));
            superset.MustNotBeNull(nameof(superset));

            return set.All(superset.Contains);
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Benchmark]
        public bool ArrayWithForeachLoops() => ArrayWithForeachLoops(SubsetArray, SupersetArray);

        private static bool ArrayWithForeachLoops<T>(T[] set, T[] superset, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            foreach (var itemFromSet in set)
            {
                var wasItemFound = false;
                foreach (var itemFromSuperset in superset)
                {
                    if (comparer.Equals(itemFromSet, itemFromSuperset))
                    {
                        wasItemFound = true;
                        break;
                    }
                }

                if (!wasItemFound)
                    return false;
            }

            return true;
        }

        [GlobalSetup(Target = nameof(ListWithForLoops) + "," + nameof(ListWithLinq) + "," + nameof(ListWithForeachLoops))]
        public void GlobalListSetup()
        {
            SupersetList = new List<int>(NumberOfItems);
            for (var i = 0; i < NumberOfItems; i++)
            {
                SupersetList.Add(i + 1);
            }

            SubsetList = new List<int>(5);
            for (var i = 0; i < 5; i++)
            {
                SubsetList.Add(SupersetList[SupersetList.Count - 1 - i]);
            }
        }

        [Benchmark]
        public bool ListWithForLoops() => ListWithForLoops(SubsetList, SupersetList);

        private static bool ListWithForLoops<T>(List<T> subset, List<T> superset, IEqualityComparer<T> comparer = null)
        {
            subset.MustNotBeNull(nameof(subset));
            superset.MustNotBeNull(nameof(superset));
            comparer = comparer ?? EqualityComparer<T>.Default;

            for (var i = 0; i < subset.Count; i++)
            {
                var currentItem = subset[i];
                var wasItemFound = false;
                for (var j = 0; j < superset.Count; j++)
                {
                    if (!comparer.Equals(superset[i], currentItem))
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
        public bool ListWithLinq() => Linq(SubsetList, SupersetList);

        [Benchmark]
        public bool ListWithForeachLoops() => ListWithForeachLoops(SubsetList, SupersetList);

        private static bool ListWithForeachLoops<T>(List<T> set, List<T> superset, IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            foreach (var itemFromSet in set)
            {
                var wasItemFound = false;
                foreach (var itemFromSuperset in superset)
                {
                    if (comparer.Equals(itemFromSet, itemFromSuperset))
                    {
                        wasItemFound = true;
                        break;
                    }
                }

                if (!wasItemFound)
                    return false;
            }

            return true;
        }
    }
}