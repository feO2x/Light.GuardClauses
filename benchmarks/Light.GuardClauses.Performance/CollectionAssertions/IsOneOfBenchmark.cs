using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class IsOneOfBenchmark
    {
        public IList<int> InterfaceList;
        public IEnumerable<int> LazyEnumerable;

        public List<int> List = new List<int>
        {
            84,
            2003,
            -45423,
            2,
            50000
        };

        public int ValueA = 2003;
        public int ValueB = 100410;

        public IsOneOfBenchmark()
        {
            InterfaceList = List;
            LazyEnumerable = InitializeLazyEnumerable();
        }

        private static IEnumerable<int> InitializeLazyEnumerable()
        {
            yield return 84;
            yield return 2003;
            yield return -45423;
            yield return 2;
            yield return 50000;
        }

        [Benchmark(Baseline = true)]
        public bool ImperativeList() => List.Contains(ValueA);

        [Benchmark]
        public bool ImperativeListNoHit() => List.Contains(ValueB);

        [Benchmark]
        public bool ImperativeInterface() => InterfaceList.Contains(ValueA);

        [Benchmark]
        public bool ImperativeInterfaceNotHit() => InterfaceList.Contains(ValueB);

        [Benchmark]
        public bool ImperativeInterfaceForLoop()
        {
            for (var i = 0; i < InterfaceList.Count; ++i)
            {
                if (InterfaceList[i] == ValueA)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeInterfaceForLoopNoHit()
        {
            for (var i = 0; i < InterfaceList.Count; ++i)
            {
                if (InterfaceList[i] == ValueB)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeInterfaceForeachLoop()
        {
            foreach (var item in InterfaceList)
            {
                if (item == ValueA)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeInterfaceForeachLoopNotHit()
        {
            foreach (var item in InterfaceList)
            {
                if (item == ValueB)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeLazy()
        {
            foreach (var item in LazyEnumerable)
            {
                if (item == ValueA)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeLazyNoHit()
        {
            foreach (var item in LazyEnumerable)
            {
                if (item == ValueB)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeLazyToList()
        {
            var list = LazyEnumerable.ToList();

            foreach (var item in list)
            {
                if (item == ValueA)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool ImperativeLazyToListNoHit()
        {
            var list = LazyEnumerable.ToList();

            foreach (var item in list)
            {
                if (item == ValueB)
                    return true;
            }

            return false;
        }

        [Benchmark]
        public bool LightGuardClausesList() => ValueA.IsOneOf(List);

        [Benchmark]
        public bool LightGuardClausesListNoHit() => ValueB.IsOneOf(List);

        [Benchmark]
        public bool LightGuardClausesInterface() => ValueA.IsOneOf(InterfaceList);

        [Benchmark]
        public bool LightGuardClausesInterfaceNoHit() => ValueB.IsOneOf(InterfaceList);

        [Benchmark]
        public bool LightGuardClausesLazy() => ValueA.IsOneOf(LazyEnumerable);

        [Benchmark]
        public bool LightGuardClausesLazyNoHit() => ValueB.IsOneOf(LazyEnumerable);
    }
}