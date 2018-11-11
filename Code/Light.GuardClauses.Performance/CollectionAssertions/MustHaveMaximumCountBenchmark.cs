using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustHaveMaximumCountBenchmark
    {
        public ICollection<int> Collection = new[] { 1, 2, 3, 4, 5 };
        public int MaximumCount = 5;

        [Benchmark(Baseline = true)]
        public ICollection<int> ImperativeVersion()
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Collection.Count > MaximumCount) throw new InvalidCollectionCountException(nameof(Collection));

            return Collection;
        }

        [Benchmark]
        public ICollection<int> LightGuardClauses() => Collection.MustHaveMaximumCount(MaximumCount, nameof(Collection));

        [Benchmark]
        public ICollection<int> LightGuardClausesCustomException() => Collection.MustHaveMaximumCount(MaximumCount, (i, c) => new Exception($"Too much items, man, too much items ({c})"));

        [Benchmark]
        public IEnumerable<int> OldVersion() => Collection.OldMustHaveMaximumCount(MaximumCount, nameof(Collection));
    }

    public static class MustHaveMaximumCountExtensions
    {
        public static IEnumerable<T> OldMustHaveMaximumCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);
            count.MustNotBeLessThan(0);

            var collectionCount = parameter.Count();
            if (collectionCount <= count)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must have at most count {count}, but it actually has count {collectionCount}.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
