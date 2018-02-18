using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustNotBeLessThanOrEqualToBenchmarks : DefaultBenchmark
    {
        public readonly short First = 13;
        public readonly short Second = 12;

        [Benchmark(Baseline = true)]
        public short BaseVersion()
        {
            if (First <= Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public double BaseVersionWithCompareTo()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            if (First.CompareTo(Second) <= 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public short LightGuardClausesWithParameterName() => First.MustNotBeLessThanOrEqualTo(Second, nameof(First));

        [Benchmark]
        public short LightGuardClausesWithCustomException() => First.MustNotBeLessThanOrEqualTo(Second, () => new Exception());

        [Benchmark]
        public short LightGuardClausesWithExceptionOneParameter() => First.MustNotBeLessThanOrEqualTo(Second, p => new Exception($"{p} is not greater than."));

        [Benchmark]
        public short LightGuardClausesWithExceptionTwoParameters() => First.MustNotBeLessThanOrEqualTo(Second, (p, b) => new Exception($"{p} is not greater than {b}."));

        [Benchmark]
        public short OldVersion() => First.OldMustNotBeLessThanOrEqualTo(Second, nameof(First));
    }

    public static class MustNotBeLessThanOrEqualToExtensionMethods
    {
        public static T OldMustNotBeLessThanOrEqualTo<T>(this T parameter, T boundary, string parameterName = null, string message = null, Func<Exception> exception = null) where T : IComparable<T>
        {
            if (parameter.CompareTo(boundary) > 0)
                return parameter;

            throw exception != null ? exception() : new ArgumentOutOfRangeException(parameterName, parameter, message ?? $"{parameterName ?? "The value"} must not be less than or equal to {boundary}, but you specified {parameter}.");
        }
    }
}