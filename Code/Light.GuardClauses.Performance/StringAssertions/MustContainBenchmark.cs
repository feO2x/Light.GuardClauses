using System;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustContainBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public string ImperativeVersionShort()
        {
            if (ShortString == null) throw new ArgumentNullException(nameof(ShortString));
            if (!ShortString.Contains(Substring)) throw new SubstringException(nameof(ShortString));

            return ShortString;
        }

        [Benchmark]
        public string ImperativeVersionLong()
        {
            if (LongString == null) throw new ArgumentNullException(nameof(LongString));
            if (!LongString.Contains(Substring)) throw new SubstringException(nameof(LongString));

            return LongString;
        }

        [Benchmark]
        public string LightGuardClausesShort() => ShortString.MustContain(Substring, nameof(ShortString));

        [Benchmark]
        public string LightGuardClausesShortCustomException() => ShortString.MustContain(Substring, (s1, s2) => new Exception("Not contained."));

        [Benchmark]
        public string LightGuardClausesLong() => LongString.MustContain(Substring, nameof(LongString));

        [Benchmark]
        public string LightGuardClausesLongCustomException() => LongString.MustContain(Substring, (s1, s2) => new Exception("Not contained."));

        [Benchmark]
        public string LightGuardClausesShortCustomSearch() => ShortString.MustContain(Substring, StringComparison.OrdinalIgnoreCase, nameof(ShortString));

        [Benchmark]
        public string LightGuardClausesShortCustomSearchCustomException() => ShortString.MustContain(Substring, StringComparison.OrdinalIgnoreCase, (s1, s2, c) => new Exception("Not contained."));

        [Benchmark]
        public string LightGuardClausesLongCustomSearch() => LongString.MustContain(Substring, StringComparison.OrdinalIgnoreCase, nameof(ShortString));

        [Benchmark]
        public string LightGuardClausesLongCustomSearchCustomException() => LongString.MustContain(Substring, StringComparison.OrdinalIgnoreCase, (s1, s2, c) => new Exception("Not contained."));

        [Benchmark]
        public string OldVersionShort() => ShortString.OldMustContain(Substring, parameterName: nameof(ShortString));

        [Benchmark]
        public string OldVersionLong() => LongString.OldMustContain(Substring, parameterName: nameof(LongString));

        [Benchmark]
        public string OldVersionShortCustomSearch() => ShortString.OldMustContain(Substring, true, nameof(ShortString));

        [Benchmark]
        public string OldVersionLOngCustomSearch() => LongString.OldMustContain(Substring, true, nameof(LongString));
    }

    public static class MustContainExtensions
    {
        public static string OldMustContain(this string parameter, string text, IgnoreCaseInfo ignoreCase = default, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.IsContaining(text, ignoreCase)) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must contain \"{text}\", but it does not.", parameterName);
        }

        public static bool IsContaining(this string @string, string text, IgnoreCaseInfo ignoreCase = default)
        {
            @string.MustNotBeNull(nameof(@string));
            text.MustNotBeNull(nameof(text));

            return ignoreCase.StringContains(@string, text);
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
}