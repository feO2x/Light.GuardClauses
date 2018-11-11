using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustContainBenchmark
    {
        public IList<int> Collection = new[] { 42, 35, 89 };

        [Benchmark(Baseline = true)]
        public IList<int> ImperativeVersion()
        {
            if (Collection == null) throw new ArgumentNullException(nameof(Collection));
            if (!Collection.Contains(89)) throw new MissingItemException(nameof(Collection));

            return Collection;
        }

        [Benchmark]
        public IList<int> LightGuardClauses() => Collection.MustContain(89, nameof(Collection));

        [Benchmark]
        public IList<int> LightGuardClausesCustomException() => Collection.MustContain(89, (c, i) => new Exception($"Collection does not contain {i}."));

        [Benchmark]
        public IEnumerable<int> OldVersion() => Collection.OldMustContain(89, nameof(Collection));
    }

    public static class MustContainExtensions
    {
        public static IEnumerable<T> OldMustContain<T>(this IEnumerable<T> parameter, T item, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            parameter.MustNotBeNull(parameterName);

            if (parameter.Contains(item))
                return parameter;

            throw exception?.Invoke() ??
                  new CollectionException(message ??
                                          new StringBuilder().AppendLine($"{parameterName ?? "The collection"} must contain value \"{item.ToStringOrNull()}\", but does not.")
                                                             .AppendCollectionContent(parameter)
                                                             .ToString(),
                                          parameterName);
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}