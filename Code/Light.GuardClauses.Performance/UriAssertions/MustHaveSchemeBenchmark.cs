using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.UriAssertions
{
    public class MustHaveSchemeBenchmarks : DefaultBenchmark
    {
        public Uri HttpsUrl = new Uri("https://ravendb.net");

        [Benchmark(Baseline = true)]
        public Uri ImperativeVersion()
        {
            if (HttpsUrl == null) throw new ArgumentNullException(nameof(HttpsUrl));
            if (HttpsUrl.IsAbsoluteUri == false) throw new RelativeUriException(nameof(HttpsUrl));
            if ("https".Equals(HttpsUrl.Scheme) == false) throw new InvalidUriSchemeException(nameof(HttpsUrl));
            return HttpsUrl;
        }

        [Benchmark]
        public Uri LightGuardClauses() => HttpsUrl.MustHaveScheme("https", nameof(HttpsUrl));

        [Benchmark]
        public Uri CustomException() => HttpsUrl.MustHaveScheme("https", (uri, scheme) => new Exception($"The {uri} does not use {scheme}"), nameof(HttpsUrl));

        [Benchmark]
        public Uri OldVersion() => HttpsUrl.OldMustHaveScheme("https", nameof(HttpsUrl));
    }

    public static class MustHaveSchemeExtensionMethods
    {
        public static Uri OldMustHaveScheme(this Uri uri, string scheme, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);

            if (uri.IsAbsoluteUri && string.Equals(uri.Scheme, scheme, StringComparison.OrdinalIgnoreCase))
                return uri;

            var subclause = uri.IsAbsoluteUri ? $"but actually has scheme \"{uri.Scheme}\" (\"{uri}\")." : $"but it has none because it is a relative URI (\"{uri}\").";
            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The URI"} must have scheme \"{scheme}\", {subclause}");
        }
    }
}