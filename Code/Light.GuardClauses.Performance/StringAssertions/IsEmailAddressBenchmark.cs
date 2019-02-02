using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsEmailAddressBenchmark
    {
        public string InvalidEmail = "email@domain@domain.com";
        public string ValidEmail = "firstname-lastname@domain.com";

        private static readonly Regex SimpleRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        [Benchmark(Baseline = true)]
        public bool ImperativeValidEmail() => ValidEmail != null && RegularExpressions.EmailRegex.IsMatch(ValidEmail);

        [Benchmark]
        public bool ImperativeInvalidEmail() => InvalidEmail != null && RegularExpressions.EmailRegex.IsMatch(InvalidEmail);

        [Benchmark]
        public bool LightGuardClausesValidEmail() => ValidEmail.IsEmailAddress();

        [Benchmark]
        public bool LightGuardClausesInvalidEmail() => InvalidEmail.IsEmailAddress();

        [Benchmark]
        public bool LightGuardClausesSimpleRegexValidEmail() => ValidEmail.IsEmailAddress(SimpleRegex);

        [Benchmark]
        public bool LightGuardClausesSimpleRegexInvalidEmail() => InvalidEmail.IsEmailAddress(SimpleRegex);

        [Benchmark]
        public bool AllocatingVersionValidEmail() => ValidEmail.AllocatingIsEmailAddress();

        [Benchmark]
        public bool AllocatingVersionInvalidEmail() => InvalidEmail.AllocatingIsEmailAddress();
    }

    public static class IsEmailAddressExtensions
    {
        public static bool AllocatingIsEmailAddress(this string emailAddress)
        {
            if (emailAddress == null) return false;

            var regex = new Regex(
                @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((((\w+\-?)+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$",
                RegexOptions.CultureInvariant | RegexOptions.ECMAScript
            );

            return regex.IsMatch(emailAddress);
        }
    }
}