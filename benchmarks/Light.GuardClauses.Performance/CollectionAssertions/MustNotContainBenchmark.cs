using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustNotContainBenchmark
    {
        public List<string> Collection = new List<string> { "Foo", "Bar", "Baz", "Qux" };
        public string Item = "Corge";

        [Benchmark(Baseline = true)]
        public List<string> ImperativeVersion()
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (Collection.Contains(Item)) throw new ExistingItemException(nameof(Collection));

            return Collection;
        }

        [Benchmark]
        public List<string> LightGuardClauses() => Collection.MustNotContain(Item, nameof(Collection));

        [Benchmark]
        public List<string> LightGuardClausesCustomException() => Collection.MustNotContain(Item, (c, i) => new Exception($"The collection contain {i}."));

        [Benchmark]
        public IEnumerable<string> OldVersion() => Collection.OldMustNotContain(Item, nameof(Collection));
    }

    public static class MustNotContainExtensions
    {
        public static IEnumerable<T> OldMustNotContain<T>(this IEnumerable<T> parameter, T item, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(item) == false)
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder($"{parameterName ?? "The collection"} must not contain value \"{item.ToStringOrNull()}\", but it does.")
                                             .AppendCollectionContent(parameter)
                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}