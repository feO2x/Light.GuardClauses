using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustHaveMinimumCountBenchmark : DefaultBenchmark
    {
        public IReadOnlyList<string> Collection = new List<string>{"Foo", "Bar", "Baz"};
        public int MinimumCount = 3;

        [Benchmark(Baseline = true)]
        public IReadOnlyList<string> ImperativeVersion()
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Collection.Count < MinimumCount) throw new InvalidCollectionCountException(nameof(Collection));

            return Collection;
        }

        [Benchmark]
        public IReadOnlyList<string> LightGuardClauses() => Collection.MustHaveMinimumCount(MinimumCount, nameof(Collection));

        [Benchmark]
        public IReadOnlyList<string> LightGuardClausesCustomException() => Collection.MustHaveMinimumCount(MinimumCount, (i, c) => new Exception("More items for collection"));

        [Benchmark]
        public IEnumerable<string> OldVersion() => Collection.OldMustHaveMinimumCount(MinimumCount, nameof(Collection));
    }

    public static class MustHaveMinimumCountExtensions
    {
        public static IEnumerable<T> OldMustHaveMinimumCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            count.MustNotBeLessThan(0);

            var collectionCount = parameter.Count();
            if (collectionCount >= count)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must have at least count {count}, but it actually has count {count}.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
