using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.CollectionAssertions
{
    public class MustBeOneOfBenchmark : DefaultBenchmark
    {
        public int Value = 4;
        public IList<int> Items = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

        [Benchmark(Baseline = true)]
        public int ImperativeVersion()
        {
            if (!Items.Contains(Value)) throw new ValueNotOneOfException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int LightGuardClauses() => Value.MustBeOneOf(Items, nameof(Value));

        [Benchmark]
        public int LightGuardClausesCustomException() => Value.MustBeOneOf(Items, (v, i) => new Exception($"{i} is simply wrong"));

        [Benchmark]
        public int OldVersion() => Value.OldMustBeOneOf(Items, parameterName: nameof(Value));
    }

    public static class MustBeOneOfExtensions
    {
        public static T OldMustBeOneOf<T>(this T parameter, IEnumerable<T> items, IEqualityComparer<T> equalityComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            items.MustNotBeNull(nameof(items));
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            if (items.Contains(parameter, equalityComparer))
                return parameter;

            throw exception?.Invoke() ??
                  new ArgumentOutOfRangeException(parameterName,
                                                  parameter,
                                                  message ??
                                                  new StringBuilder().AppendLine($"{parameterName ?? "The value"} must be one of the items")
                                                                     .AppendItems(items, ErrorMessageExtensions.DefaultNewLineSeparator).AppendLine()
                                                                     .AppendLine($"but you specified {parameter}.")
                                                                     .ToString());

            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}