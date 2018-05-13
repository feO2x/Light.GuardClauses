using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustNotBeNullOrWhiteSpaceBenchmark : DefaultBenchmark
    {
        public string NonEmptyString = "Foo";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (NonEmptyString == null) throw new ArgumentNullException(nameof(NonEmptyString));
            if (NonEmptyString == string.Empty) throw new EmptyStringException(nameof(NonEmptyString));
            if (string.IsNullOrWhiteSpace(NonEmptyString)) throw new WhiteSpaceStringException(nameof(NonEmptyString));
            return NonEmptyString;
        }

        [Benchmark]
        public string LightGuardClauses() => NonEmptyString.MustNotBeNullOrWhiteSpace(nameof(NonEmptyString));

        [Benchmark]
        public string LightGuardClausesCustomException() => NonEmptyString.MustNotBeNullOrWhiteSpace(_ => new Exception("The string must not contain only white space"));

        [Benchmark]
        public string OldVersion() => NonEmptyString.OldMustNotBeNullOrWhiteSpace(nameof(NonEmptyString));
    }

    public static class MustNotBeNullOrWhiteSpaceExtensions
    {
        public static string OldMustNotBeNullOrWhiteSpace(this string parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.OldMustNotBeNullOrEmpty(parameterName, message, exception);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var character in parameter)
                if (char.IsWhiteSpace(character) == false)
                    return parameter;
            throw exception?.Invoke() ?? new WhiteSpaceStringException();
        }
    }
}