using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustBeLessThanOrEqualToBenchmark
    {
        public float First = 42.7f;
        public float Second = 78.21f;

        [Benchmark(Baseline = true)]
        public float ImperativeVersion()
        {
            if (First > Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public float ImperativeVersionWithCompareTo()
        {
            if (First.CompareTo(Second) > 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public float LightGuardClausesParameterName() => First.MustBeLessThanOrEqualTo(Second, nameof(First));

        [Benchmark]
        public float LightGuardClausesCustomException() => First.MustBeLessThanOrEqualTo(Second, (p, b) => new Exception($"{p} is not less than {b}."));

        [Benchmark]
        public float OldVersion() => First.OldMustBeLessThanOrEqualTo(Second, nameof(First));

        [Benchmark]
        public float OnlyParameterNameVersion() => First.OnlyParameterNameMustBeLessThanOrEqualTo(Second, nameof(First));
    }

    public static class MustBeLessThanOrEqualToExtensionMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T OnlyParameterNameMustBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                ThrowException(parameter, boundary, parameterName);
            return parameter;
        }

        private static void ThrowException<T>(T parameter, T boundary, string paramterName)
        {
            throw new ArgumentOutOfRangeException(paramterName, $"The value must be less than or equal to {boundary}, but it actually is {parameter}.");
        }


        public static T OldMustBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) <= 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must be less than {boundary}, but you specified {parameter}.");
        }
    }
}