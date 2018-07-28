using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustNotBeNullOrEmptyBenchmark : DefaultBenchmark
    {
        public List<string> Collection = new List<string> { "Foo", "Bar", "Baz", "Qux" };

        [Benchmark(Baseline = true)]
        public List<string> ImperativeVersion()
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Collection.Count == 0) throw new EmptyCollectionException(nameof(Collection));

            return Collection;
        }

        [Benchmark]
        public List<string> LightGuardClauses() => Collection.MustNotBeNullOrEmpty(nameof(Collection));

        [Benchmark]
        public List<string> LightGuardClausesCustomException() => Collection.MustNotBeNullOrEmpty(c => new Exception("The collection is empty, you fool"));

        [Benchmark]
        public IEnumerable<string> OldVersion() => Collection.OldMustNotBeNullOrEmpty(nameof(Collection));
    }

    public static class MustNotBeNullOrEmptyExtensions
    {
        public static IEnumerable<T> OldMustNotBeNullOrEmpty<T>(this IEnumerable<T> parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Any())
                return parameter;

            throw exception != null ? exception() : (message == null ? new EmptyCollectionException(parameterName) : new EmptyCollectionException(message, parameterName));
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}