using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeSameAsBenchmark
    {
        public object Reference1 = new object();
        public object Reference2 = new List<string>();

        [Benchmark(Baseline = true)]
        public object ImperativeVersion()
        {
            if (ReferenceEquals(Reference1, Reference2))
                throw new SameObjectReferenceException(nameof(Reference1));
            return Reference1;
        }

        [Benchmark]
        public object LightGuardClauses() => Reference1.MustNotBeSameAs(Reference2);

        [Benchmark]
        public object LightGuardClausesCustomException() => Reference1.MustNotBeSameAs(Reference2, r => new Exception());

        [Benchmark]
        public object OldVersion() => Reference1.OldMustNotBeSameAs(Reference2, nameof(Reference1));
    }

    public static class MustNotBeSameAsExtensions
    {
        public static T OldMustNotBeSameAs<T>(this T parameter, T other, string parameterName = null, string message = null, Func<Exception> exception = null) where T : class
        {
            if (ReferenceEquals(parameter, other) == false)
                return parameter;

            throw exception?.Invoke() ?? new ArgumentException(message ?? $"{parameterName ?? "The reference"} must not point to the object instance \"{other}\", but it does.", parameterName);
        }
    }
}