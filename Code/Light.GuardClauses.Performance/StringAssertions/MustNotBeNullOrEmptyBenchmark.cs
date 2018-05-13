using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustNotBeNullOrEmptyBenchmark : DefaultBenchmark
    {
        public string NonEmptyString = "Foo";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (NonEmptyString == null) throw new ArgumentNullException(nameof(NonEmptyString));
            if (NonEmptyString == string.Empty) throw new EmptyStringException(nameof(NonEmptyString));
            return NonEmptyString;
        }

        [Benchmark]
        public string LightGuardClauses() => NonEmptyString.MustNotBeNullOrEmpty(nameof(NonEmptyString));

        [Benchmark]
        public string LightGuardClausesCustomException() => NonEmptyString.MustNotBeNullOrEmpty(_ => new Exception("The string must not be null or empty"));

        [Benchmark]
        public string OldVersion() => NonEmptyString.OldMustNotBeNullOrEmpty(nameof(NonEmptyString));
    }

    public static class MustNotBeNullOrEmptyExtensions
    {
        public static string OldMustNotBeNullOrEmpty(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter == string.Empty)
                throw exception?.Invoke() ?? (message == null ? new EmptyStringException(parameterName) : new EmptyStringException(message, parameterName));

            return parameter;
        }
    }
}