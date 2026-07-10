using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustBeSubstringOfBenchmark
    {
        public string Other = "I shall wear this as a badge of honor.";
        public string Substring = "wear this";

        [Benchmark(Baseline = true)]
        public string ImperativeVersion()
        {
            if (Substring == null) throw new ArgumentNullException(nameof(Substring));
            if (!Other.Contains(Substring)) throw new SubstringException(nameof(Substring));

            return Substring;
        }

        [Benchmark]
        public string ImperativeVersionCustomComparisonType()
        {
            if (Substring == null) throw new ArgumentNullException(nameof(Substring));
            if (Other.IndexOf(Substring, StringComparison.CurrentCultureIgnoreCase) < 0) throw new SubstringException(nameof(Substring));

            return Substring;
        }

        [Benchmark]
        public string LightGuardClauses() => Substring.MustBeSubstringOf(Other, nameof(Substring));

        [Benchmark]
        public string LightGuardClausesCustomComparisonType() => Substring.MustBeSubstringOf(Other, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public string LightGuardClausesCustomException() => Substring.MustBeSubstringOf(Other, (s1, s2) => new Exception($"Where is your honor, {s1}?"));

        [Benchmark]
        public string LightGuardClausesCustomExceptionCustomComparisonType() =>
            Substring.MustBeSubstringOf(Other, StringComparison.CurrentCultureIgnoreCase, (x, y, c) => new Exception($"Where is your honor, {x}? ({c})"));

        [Benchmark]
        public string OldVersion() => Substring.OldMustBeSubstringOf(Other, parameterName: nameof(Other));
    }

    public static class MustBeSubstringOfExtensions
    {
        public static string OldMustBeSubstringOf(this string parameter, string text, IgnoreCaseInfo ignoreCase = default, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (text.IsContaining(parameter, ignoreCase)) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must be a substring of \"{text}\", but it is not.", parameterName);
        }

        public static bool IsContaining(this string @string, string text, IgnoreCaseInfo ignoreCase = default)
        {
            @string.MustNotBeNull(nameof(@string));
            text.MustNotBeNull(nameof(text));

            return ignoreCase.StringContains(@string, text);
        }
    }

    public readonly struct IgnoreCaseInfo
    {
        public readonly CultureInfo CultureInfo;

        public readonly CompareOptions CompareOptions;

        public IgnoreCaseInfo(CultureInfo cultureInfo, CompareOptions compareOptions)
        {
            CultureInfo = cultureInfo.MustNotBeNull(nameof(cultureInfo));
            CompareOptions = compareOptions.MustBeValidEnumValue(nameof(compareOptions));
        }

        public static implicit operator IgnoreCaseInfo(bool ignoreCase)
        {
            return ignoreCase ? new IgnoreCaseInfo(CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) : new IgnoreCaseInfo();
        }

        public static implicit operator IgnoreCaseInfo(CultureInfo cultureInfo)
        {
            return new IgnoreCaseInfo(cultureInfo, CompareOptions.OrdinalIgnoreCase);
        }

        public bool StringContains(string @string, string substring)
        {
            if (CultureInfo == null)
                return @string.MustNotBeNull(nameof(@string)).Contains(substring);

            return CultureInfo.CompareInfo.IndexOf(@string, substring, CompareOptions) != -1;
        }
    }
}