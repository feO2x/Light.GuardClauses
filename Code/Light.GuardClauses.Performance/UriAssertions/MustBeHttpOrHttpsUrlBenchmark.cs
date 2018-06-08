using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.UriAssertions
{
    public class MustBeHttpOrHttpsUrlBenchmark : DefaultBenchmark
    {
        public Uri Uri = new Uri("https://www.microsoft.com");

        [Benchmark(Baseline = true)]
        public Uri ImperativeVersion()
        {
            if (Uri == null) throw new ArgumentNullException(nameof(Uri));
            if (!Uri.IsAbsoluteUri) throw new RelativeUriException(nameof(Uri));
            if (!Uri.Scheme.Equals("https") && !Uri.Scheme.Equals("http")) throw new InvalidUriSchemeException(nameof(Uri));

            return Uri;
        }

        [Benchmark]
        public Uri LightGuardClauses() => Uri.MustBeHttpOrHttpsUrl(nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesCustomException() => Uri.MustBeHttpOrHttpsUrl(uri => new Exception("Why don't you use HTTP services?"));

        [Benchmark]
        public Uri OldVersion() => Uri.OldMustBeHttpOrHttpsUrl(nameof(Uri));
    }

    public static class MustBeHttpOrHttpsUrlExtensions
    {
        public static Uri OldMustBeHttpOrHttpsUrl(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            return uri.OldMustHaveOneSchemeOf(new[] { "http", "https" }, parameterName, message, exception);
        }
    }
}