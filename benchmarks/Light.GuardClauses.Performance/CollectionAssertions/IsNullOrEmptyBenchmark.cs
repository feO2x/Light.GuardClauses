using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class IsNullOrEmptyBenchmark
    {
        public List<string> CollectionWithItems = new List<string>
        {
            "Foo",
            "Bar",
            "Baz"
        };

        public ObservableCollection<object> EmptyCollection = new ObservableCollection<object>();
        public IEnumerable<string> LazyEnumerable = CreateLazyEnumerable();

        public IReadOnlyList<object> NullCollection = null;

        private static IEnumerable<string> CreateLazyEnumerable()
        {
            yield return "Foo";
            yield return "Bar";
            yield return "Baz";
        }

        [Benchmark(Baseline = true)]
        public bool ImperativeNull() => NullCollection == null || NullCollection.Count == 0;

        [Benchmark]
        public bool ImperativeEmpty() => EmptyCollection == null || EmptyCollection.Count == 0;

        [Benchmark]
        public bool ImperativeNotEmpty() => CollectionWithItems == null || CollectionWithItems.Count == 0;

        [Benchmark]
        public bool ImperativeLazyEnumerable() => LazyEnumerable == null || !LazyEnumerable.Any();

        [Benchmark]
        public bool LightGuardClausesNull() => NullCollection.IsNullOrEmpty();

        [Benchmark]
        public bool LightGuardClausesEmpty() => EmptyCollection.IsNullOrEmpty();

        [Benchmark]
        public bool LightGuardClausesNotEmpty() => CollectionWithItems.IsNullOrEmpty();

        [Benchmark]
        public bool LightGuardClausesLazyEnumerable() => LazyEnumerable.IsNullOrEmpty();
    }
}