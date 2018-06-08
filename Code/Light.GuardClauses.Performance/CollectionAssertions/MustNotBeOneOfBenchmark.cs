using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustNotBeOneOfBenchmark
    {
        public int Value = 42;
        public IList<int> Items = new[] { 1, 4, 6, 5 };

        [Benchmark(Baseline = true)]
        public int ImperativeVersion()
        {
            if (Items.Contains(Value)) throw new ValueIsOneOfException(nameof(Value));

            return Value;
        }

        [Benchmark]
        public int LightGuardClauses() => Value.MustNotBeOneOf(Items, nameof(Value));

        [Benchmark]
        public int LightGuardClausesCustomException() => Value.MustNotBeOneOf(Items, (v, i) => new Exception($"{v} is present, you fool"));

        [Benchmark]
        public int OldVersion() => Value.OldMustNotBeOneOf(Items, parameterName: nameof(Value));
    }

    public static class MustNotBeOneOfExtensions
    {
        public static T OldMustNotBeOneOf<T>(this T parameter, IEnumerable<T> items, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            items.MustNotBeNull(nameof(items));
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (items.Contains(parameter, equalityComparer) == false)
                return parameter;

            throw exception?.Invoke() ??
                  new ArgumentOutOfRangeException(parameterName,
                                                  parameter,
                                                  message ??
                                                  new StringBuilder().AppendLine($"{parameterName ?? "The value"} must not be one of the items")
                                                                     .AppendItemsWithNewLine(items)
                                                                     .AppendLine($"but you specified {parameter}.")
                                                                     .ToString());
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}
