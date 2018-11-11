using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustHaveCountBenchmark
    {
        public List<string> Collection = new List<string>{"Foo", "Bar"};
        public string String = "Baz";

        [Benchmark(Baseline = true)]
        public List<string> ImperativeVersionCollection()
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Collection.Count != 2) throw new InvalidCollectionCountException(nameof(Collection));
            return Collection;
        }

        [Benchmark]
        public string ImperativeVersionString()
        {
            if (String == null) throw new ArgumentNullException(nameof(String));
            if (String.Length != 3) throw new InvalidCollectionCountException(nameof(String));
            return String;
        }

        [Benchmark]
        public List<string> LightGuardClausesCollection() => Collection.MustHaveCount(2, nameof(Collection));

        [Benchmark]
        public string LightGuardClausesString() => String.MustHaveCount(3, nameof(String));

        [Benchmark]
        public List<string> LightGuardClausesCustomExceptionCollection() => Collection.MustHaveCount(2, (collection, count) => new Exception($"Collection has not count {count}"));

        [Benchmark]
        public string LightGuardClausesCustomExceptionString() => String.MustHaveCount(3, (collection, count) => new Exception($"Collection has not count {count}"));

        [Benchmark]
        public IEnumerable<string> OldVersionCollection() => Collection.OldMustHaveCount(2, nameof(Collection));

        [Benchmark]
        public IEnumerable<char> OldVersionString() => String.OldMustHaveCount(3, nameof(String));
    }

    public static class MustHaveCountExtensions
    {
        public static IEnumerable<T> OldMustHaveCount<T>(this IEnumerable<T> parameter, int count, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull();
            count.MustNotBeLessThan(0, nameof(count));

            if (parameter.Count() == count)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          $"{parameterName ?? "The collection"} must have count {count}, but you specified a collection with count {parameter.Count()}.",
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
