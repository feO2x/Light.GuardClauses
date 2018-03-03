using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.TypeAssertions
{
    public class MustBeEquivalentToBenchmarks : DefaultBenchmark
    {
        public Type ClosedConstructedGenericType = typeof(IList<SampleEntity>);
        public Type GenericTypeDefinition = typeof(IList<>);
        public Type NonGenericType = typeof(ArgumentDefaultException);

        [Benchmark(Baseline = true)]
        public Type OldVersionSameReference() => NonGenericType.OldMustBeEquivalentTo(NonGenericType, nameof(NonGenericType));

        [Benchmark]
        public Type LightGuardClausesSameReference() => NonGenericType.MustBeEquivalentTo(NonGenericType, nameof(NonGenericType));

        [Benchmark]
        public Type OldVersionEqualType() => NonGenericType.OldMustBeEquivalentTo(typeof(ArgumentDefaultException), nameof(NonGenericType));

        [Benchmark]
        public Type LightGuardClausesEqualType() => NonGenericType.MustBeEquivalentTo(typeof(ArgumentDefaultException), nameof(NonGenericType));

        [Benchmark]
        public Type OldVersionEquivalentType() => ClosedConstructedGenericType.OldMustBeEquivalentTo(GenericTypeDefinition, nameof(NonGenericType));

        [Benchmark]
        public Type LightGuardEquivalentType() => ClosedConstructedGenericType.MustBeEquivalentTo(GenericTypeDefinition, nameof(NonGenericType));
    }

    public static class MustBeEquivalentToExtensions
    {
        public static Type OldMustBeEquivalentTo(this Type parameter, Type other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.OldIsEquivalentTypeTo(other))
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be equivalent to \"{other?.ToString() ?? "null"}\", but it is not.", parameterName);
        }
    }
}