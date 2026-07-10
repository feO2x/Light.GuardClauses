using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.DateTimeAssertions
{
    public class MustBeUnspecifiedBenchmark
    {
        public DateTime DateTime = new DateTime(2018, 2, 18, 16, 43, 00, DateTimeKind.Unspecified);

        [Benchmark(Baseline = true)]
        public DateTime ImperativeVersion()
        {
            if (DateTime.Kind != DateTimeKind.Unspecified) throw new InvalidDateTimeException(nameof(DateTime));
            return DateTime;
        }

        [Benchmark]
        public DateTime LightGuardClausesParameterName() => DateTime.MustBeUnspecified(nameof(DateTime));

        [Benchmark]
        public DateTime LightGuardClausesCustomException() => DateTime.MustBeUnspecified(dt => new Exception($"{dt} is not unspecified"));

        [Benchmark]
        public DateTime OldVersion() => DateTime.OldMustBeUnspecified(nameof(DateTime));
    }

    public static class MustBeUnspecifiedExtensionMethods
    {
        public static DateTime OldMustBeUnspecified(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified) return dateTime;

            throw exception != null ? exception() : new InvalidDateTimeException(parameterName, message ?? $"The specified date time \"{dateTime:O}\" must be of kind {DateTimeKind.Unspecified}, but actually is {dateTime.Kind}.");
        }
    }
}