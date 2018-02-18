using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.DateTimeAssertions
{
    public class MustBeLocalBenchmarks : DefaultBenchmark
    {
        public DateTime DateTime = new DateTime(2018, 2, 18, 16, 39, 00, DateTimeKind.Local);

        [Benchmark(Baseline = true)]
        public DateTime ImperativeVersion()
        {
            if (DateTime.Kind != DateTimeKind.Local) throw new InvalidDateTimeException(nameof(DateTime));
            return DateTime;
        }

        [Benchmark]
        public DateTime LightGuardClausesParameterName() => DateTime.MustBeLocal(nameof(DateTime));

        [Benchmark]
        public DateTime LightGuardClausesCustomException() => DateTime.MustBeLocal(dt => new Exception($"{dt} is not local"));

        [Benchmark]
        public DateTime OldVersion() => DateTime.OldMustBeLocal(nameof(DateTime));
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