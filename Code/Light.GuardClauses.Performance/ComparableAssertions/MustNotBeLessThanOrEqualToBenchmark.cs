using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.ComparableAssertions
{
    public class MustNotBeLessThanOrEqualToBenchmark
    {
        public short First = 13;
        public short Second = 12;

        [Benchmark(Baseline = true)]
        public short ImperativeVersion()
        {
            if (First <= Second) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public double ImperativeVersionWithCompareTo()
        {
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            if (First.CompareTo(Second) <= 0) throw new ArgumentOutOfRangeException(nameof(First));
            return First;
        }

        [Benchmark]
        public short LightGuardClausesWithParameterName() => First.MustNotBeLessThanOrEqualTo(Second, nameof(First));

        [Benchmark]
        public short LightGuardClausesCustomException() => First.MustNotBeLessThanOrEqualTo(Second, (p, b) => new Exception($"{p} is not greater than {b}."));

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