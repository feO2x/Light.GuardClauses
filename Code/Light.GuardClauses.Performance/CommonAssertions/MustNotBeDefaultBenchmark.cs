using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.CommonAssertions
{
    public class MustNotBeDefaultBenchmark : DefaultBenchmark
    {
        public SampleEntity Reference = new SampleEntity(Guid.NewGuid());
        public readonly int Value = 42;

        [Benchmark(Baseline = true)]
        public int ImperativeVersionForValueType()
        {
            if (Value == default) throw new ArgumentDefaultException(nameof(Value));
            return Value;
        }

        [Benchmark]
        public int MustNotBeDefaultValueV1() => Value.MustNotBeDefaultV1(nameof(Value));

        [Benchmark]
        public int MustNotBeDefaultValueV2() => Value.MustNotBeDefaultV2(nameof(Value));

        [Benchmark]
        public int OldVersionForValueType() => Value.OldMustNotBeDefault(nameof(Value));

        [Benchmark]
        public int LightGuardClausesValueType() => Value.MustNotBeDefault(nameof(Value));

        [Benchmark]
        public SampleEntity ImperativeVersionForReferenceType()
        {
            if (Reference == null) throw new ArgumentNullException(nameof(Reference));
            return Reference;
        }

        [Benchmark]
        public SampleEntity MustNotBeDefaultReferenceV1() => Reference.MustNotBeDefaultV1(nameof(Reference));

        [Benchmark]
        public SampleEntity MustNotBeDefaultReferenceV2() => Reference.MustNotBeDefaultV2(nameof(Reference));

        [Benchmark]
        public SampleEntity OldVersionForReferenceType() => Reference.OldMustNotBeDefault(nameof(Reference));

        [Benchmark]
        public SampleEntity LightGuardClausesReferenceType() => Reference.MustNotBeDefault(nameof(Reference));
    }

    public static class MustNotBeDefaultExtensionMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeDefaultV1<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) == null)
            {
                if (parameter != null)
                    return parameter;

                ThrowMustNotBeNullException(parameterName, message);
                return default;
            }

            if (parameter.Equals(default(T)) == false)
                return parameter;

            ThrowParameterDefaultException(parameterName, message);
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeDefaultV2<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (parameter == null)
                ThrowMustNotBeNullException(parameterName, message);

            // ReSharper disable once PossibleNullReferenceException
            if (parameter.Equals(default(T)))
                ThrowParameterDefaultException(parameterName, message);

            return parameter;
        }

        private static void ThrowParameterDefaultException(string parameterName, string message)
        {
            throw new ArgumentException(message ?? $"{parameterName ?? "The value"} must not be the default value.", parameterName);
        }

        private static void ThrowMustNotBeNullException(string parameterName, string message)
        {
            throw new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");
        }

        public static T OldMustNotBeDefault<T>(this T parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter == null)
                throw exception?.Invoke() ?? new ArgumentNullException(parameterName, message ?? $"{parameterName ?? "The value"} must not be null.");

            if (parameter.Equals(default(T)))
                throw exception?.Invoke() ?? new ArgumentDefaultException(parameterName, message ?? $"{parameterName ?? "The value"} must not be the default value.");

            return parameter;
        }
    }
}