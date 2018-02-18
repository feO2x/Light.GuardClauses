using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.DateTimeAssertions
{
    public class MustBeUtcBenchmarks : DefaultBenchmark
    {
        public DateTime DateTime = new DateTime(2018, 2, 18, 15, 33, 00, DateTimeKind.Utc);

        [Benchmark(Baseline = true)]
        public DateTime ImperativeVersion()
        {
            if (DateTime.Kind != DateTimeKind.Utc) throw new InvalidDateTimeException(nameof(DateTime));
            return DateTime;
        }

        [Benchmark]
        public DateTime LightGuardClausesParameterName() => DateTime.MustBeUtc(nameof(DateTime));

        [Benchmark]
        public DateTime LightGuardClausesCustomException() => DateTime.MustBeUtc(dt => new Exception($"{dt} is not UTC"));

        [Benchmark]
        public DateTime OldVersion() => DateTime.OldMustBeUtc(nameof(DateTime));
    }

    public static class MustBeUtcExtensionMethods
    {
        public static DateTime OldMustBeUtc(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Utc) return dateTime;

            throw exception != null ? exception() : new InvalidDateTimeException(parameterName, message ?? $"The specified date time \"{dateTime:O}\" must be of kind {DateTimeKind.Utc}, but actually is {dateTime.Kind}.");
        }
    }
}