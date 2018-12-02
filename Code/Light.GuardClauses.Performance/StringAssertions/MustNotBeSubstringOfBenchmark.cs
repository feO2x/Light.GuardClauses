using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustNotBeSubstringOfBenchmark
    {
        public string StringA = "There is no creature on earth half so terrifying as a truly just man.";
        public string StringB = "truly just woman";

        [Benchmark(Baseline = true)]
        public string Imperative()
        {
            if (StringA.Contains(StringB))
                throw new SubstringException(nameof(StringB));

            return StringB;
        }

        [Benchmark]
        public string LightGuardClauses() => StringB.MustNotBeSubstringOf(StringA, nameof(StringB));

        [Benchmark]
        public string LightGuardClausesCustomException() => StringB.MustNotBeSubstringOf(StringA, (x, y) => new Exception($"{x} is substring of {y}."));

        [Benchmark]
        public string OldVersion() => StringB.OldMustNotBeSubstringOf(StringA, parameterName: nameof(StringB));
    }

    public static class MustNotBeSubstringOfExtensions
    {
        public static string OldMustNotBeSubstringOf(this string parameter, string text, IgnoreCaseInfo ignoreCase = default, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNullOrEmpty(parameter);
            text.MustNotBeNullOrEmpty(nameof(text));

            if (text.IsContaining(parameter, ignoreCase) == false) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must not be a substring of \"{text}\", but it is.", parameterName);
        }
    }
}