using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.DateTimeAssertions
{
    public class MustBeLocalBenchmark
    {
        public DateTime LocalDateTime = DateTime.Now;

        [Benchmark(Baseline = true)]
        public DateTime ImperativeVersion()
        {
            if (LocalDateTime.Kind != DateTimeKind.Local) throw new InvalidDateTimeException(nameof(DateTime));
            return LocalDateTime;
        }

        [Benchmark]
        public DateTime LightGuardClausesParameterName() => LocalDateTime.MustBeLocal(nameof(DateTime));

        [Benchmark]
        public DateTime LightGuardClausesCustomException() => LocalDateTime.MustBeLocal(dt => new Exception($"{dt} is not local"));

        [Benchmark]
        public DateTime OldVersion() => LocalDateTime.OldMustBeLocal(nameof(DateTime));
    }

    public static class MustBeLocalExtensionMethods
    {
        public static DateTime OldMustBeLocal(this DateTime dateTime, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (dateTime.Kind == DateTimeKind.Local) return dateTime;

            throw exception != null ? exception() : new InvalidDateTimeException(parameterName, message ?? $"The specified date time \"{dateTime:O}\" must be of kind {DateTimeKind.Local}, but actually is {dateTime.Kind}.");
        }
    }
}