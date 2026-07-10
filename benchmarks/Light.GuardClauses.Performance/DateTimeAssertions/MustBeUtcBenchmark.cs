using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.DateTimeAssertions
{
    public class MustBeUtcBenchmark
    {
        public DateTime UtcDateTime = DateTime.UtcNow;

        [Benchmark(Baseline = true)]
        public DateTime ImperativeVersion()
        {
            if (UtcDateTime.Kind != DateTimeKind.Utc) throw new InvalidDateTimeException(nameof(UtcDateTime));
            return UtcDateTime;
        }

        [Benchmark]
        public DateTime LightGuardClauses() => UtcDateTime.MustBeUtc(nameof(UtcDateTime));

        [Benchmark]
        public DateTime LightGuardClausesCustomException() => UtcDateTime.MustBeUtc(date => new Exception("Why do you not use UTC? Are you crazy?")); // https://nodatime.org/2.3.x/userguide/trivia

        [Benchmark]
        public DateTime OldVersion() => UtcDateTime.OldMustBeUtc(nameof(UtcDateTime));
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