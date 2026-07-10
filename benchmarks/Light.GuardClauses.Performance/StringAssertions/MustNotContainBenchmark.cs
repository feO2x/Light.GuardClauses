using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class MustNotContainBenchmark
    {
        public string Substring = "reality";
        public string Text = "If you can’t tell the difference, does it matter if I'm real or not?";

        [Benchmark(Baseline = true)]
        public string Imperative()
        {
            if (Text.Contains(Substring))
                throw new SubstringException(nameof(Text), $"Text contains \"{Substring}\".");
            return Text;
        }

        [Benchmark]
        public string LightGuardClauses() => Text.MustNotContain(Substring, nameof(Text));

        [Benchmark]
        public string LightGuardClausesCustomException() => Text.MustNotContain(Substring, (x, y) => new Exception($"{x} contains {y}"));

        [Benchmark]
        public string OldVersion() => Text.OldMustNotContain(Substring, parameterName: nameof(Text));
    }

    public static class MustNotContainExtensions
    {
        public static string OldMustNotContain(this string parameter, string text, IgnoreCaseInfo ignoreCase = default, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            text.MustNotBeNull(nameof(text));

            if (parameter.IsContaining(text, ignoreCase) == false) return parameter;

            throw exception?.Invoke() ?? new StringException(message ?? $"\"{parameter}\" must not contain \"{text}\", but it does.", parameterName);
        }
    }
}