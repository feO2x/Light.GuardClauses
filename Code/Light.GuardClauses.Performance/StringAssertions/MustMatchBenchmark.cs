using System;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustMatchBenchmark
    {
        public Regex ComplexRegex = new Regex("^[\\w!#$%&\'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&\'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$"); // as per https://stackoverflow.com/a/6893571/1560623
        public Regex SimpleRegex = new Regex(@"\w5");
        public string String = "some.email@address.com";

        [Benchmark(Baseline = true)]
        public string ImperativeSimpleVersion()
        {
            if (String == null) throw new ArgumentNullException(nameof(String));
            if (!SimpleRegex.IsMatch(String)) throw new StringDoesNotMatchException(nameof(String));
            return String;
        }

        [Benchmark]
        public string ImperativeComplexVersion()
        {
            if (String == null) throw new ArgumentNullException(nameof(String));
            if (!ComplexRegex.IsMatch(String)) throw new StringDoesNotMatchException(nameof(String));
            return String;
        }

        [Benchmark]
        public string LightGuardClausesSimple() => String.MustMatch(SimpleRegex, nameof(String));

        [Benchmark]
        public string LightGuardClausesComplex() => String.MustMatch(ComplexRegex, nameof(String));

        [Benchmark]
        public string LightGuardClausesCustomExceptionSimple() => String.MustMatch(SimpleRegex, (s, r) => new Exception("It doesn't match."));

        [Benchmark]
        public string OldVersion() => String.OldMustMatch(SimpleRegex, nameof(String));
    }

    public static class MustMatchExtensions
    {
        public static string OldMustMatch(this string parameter, Regex pattern, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            pattern.MustNotBeNull(nameof(pattern));

            if (pattern.IsMatch(parameter))
                return parameter;

            throw exception?.Invoke() ?? new StringDoesNotMatchException(parameterName, message ?? $"{parameterName ?? "The string"} must match the regular expression {pattern}, but you specified {parameterName}.");
        }
    }
}