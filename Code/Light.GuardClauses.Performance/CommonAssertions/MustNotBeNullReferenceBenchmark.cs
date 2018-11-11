using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeNullReferenceBenchmark
    {
        public SampleEntity Instance = new SampleEntity(Guid.NewGuid());
        public int Value = 42;

        [Benchmark(Baseline = true)]
        public int ImperativeValueType() => Value;

        [Benchmark]
        public SampleEntity ImperativeReference()
        {
            if (Instance == null) throw new ArgumentNullException(nameof(Instance));
            return Instance;
        }

        [Benchmark]
        public SampleEntity ReferenceV1() => Instance.MustNotBeNullReferenceV1(nameof(Instance));

        [Benchmark]
        public SampleEntity ReferenceV2() => Instance.MustNotBeNullReferenceV2(nameof(Instance));

        [Benchmark]
        public SampleEntity LightGuardClausesReference() => Instance.MustNotBeNullReference(nameof(Instance));

        [Benchmark]
        public SampleEntity LightGuardClausesReferenceCustomException() => Instance.MustNotBeNullReference(() => new Exception());

        [Benchmark]
        public int ValueTypeV1() => Value.MustNotBeNullReferenceV1(nameof(Value));

        [Benchmark]
        public int ValueTypeV2() => Value.MustNotBeNullReferenceV2(nameof(Value));

        [Benchmark]
        public int LightGuardClausesValueType() => Value.MustNotBeNullReference(nameof(Value));

        [Benchmark]
        public int LightGuardClausesValueTypeCustomException() => Value.MustNotBeNullReference(() => new Exception());
    }

    public static class MustNotBeNullReferenceExtensionMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullReferenceV1<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) == null)
            {
                if (parameter != null)
                    return parameter;

                Throw.ArgumentNull(parameterName, message);
                return default(T);
            }

            return parameter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullReferenceV2<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) != null) return parameter;

            if (parameter != null)
                return parameter;

            Throw.ArgumentNull(parameterName, message);
            return default(T);
        }
    }
}