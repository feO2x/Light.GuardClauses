using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.TypeAssertions
{
    public class IsEquivalentTypeToBenchmarks : DefaultBenchmark
    {
        public Type NormalType = typeof(ArgumentDefaultException);
        public Type ClosedConstructedGenericType = typeof(IList<SampleEntity>);
        public Type GenericTypeDefinition = typeof(IList<>);
        

        [Benchmark(Baseline = true)]
        public bool OldVersionSameReference() => NormalType.OldIsEquivalentTypeTo(NormalType);

        [Benchmark]
        public bool LightGuardClausesSameReference() => NormalType.IsEquivalentTypeTo(NormalType);

        [Benchmark]
        public bool OldVersionEqualType() => NormalType.OldIsEquivalentTypeTo(typeof(ArgumentDefaultException));

        [Benchmark]
        public bool LightGuardClausesEqualType() => NormalType.IsEquivalentTypeTo(typeof(ArgumentDefaultException));

        [Benchmark]
        public bool OldVersionEquivalentType() => ClosedConstructedGenericType.OldIsEquivalentTypeTo(GenericTypeDefinition);

        [Benchmark]
        public bool LightGuardClausesEquivalentType() => ClosedConstructedGenericType.IsEquivalentTypeTo(GenericTypeDefinition);

        [Benchmark]
        public bool OldVersionDifferentType() => NormalType.OldIsEquivalentTypeTo(ClosedConstructedGenericType);

        [Benchmark]
        public bool LightGuardDifferentType() => NormalType.IsEquivalentTypeTo(ClosedConstructedGenericType);
    }

    public static class IsEquivalentTypeToExtensions
    {
        public static bool OldIsEquivalentTypeTo(this Type type, Type other)
        {
            if (ReferenceEquals(type, other)) return true;
            if (type is null || other is null) return false;

            if (type == other) return true;

            if (type.IsConstructedGenericType == other.IsConstructedGenericType)
                return false;

            if (type.IsConstructedGenericType)
                return type.GetGenericTypeDefinition() == other;
            return other.GetGenericTypeDefinition() == type;
        }
    }
}
