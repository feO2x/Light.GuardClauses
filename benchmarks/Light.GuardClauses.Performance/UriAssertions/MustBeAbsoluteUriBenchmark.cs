using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.UriAssertions
{
    public class MustBeAbsoluteUriBenchmark
    {
        public Uri AbsoluteUri = new Uri("https://www.microsoft.com");

        [Benchmark(Baseline = true)]
        public Uri ImperativeUri()
        {
            if (AbsoluteUri == null) throw new ArgumentNullException(nameof(AbsoluteUri));
            if (AbsoluteUri.IsAbsoluteUri == false) throw new RelativeUriException(nameof(AbsoluteUri));
            return AbsoluteUri;
        }

        [Benchmark]
        public Uri LightGuardClauses() => AbsoluteUri.MustBeAbsoluteUri(nameof(AbsoluteUri));

        [Benchmark]
        public Uri LightGuardClausesCustomException() => AbsoluteUri.MustBeAbsoluteUri(uri => new Exception($"This uri {uri} is wrong"));

        [Benchmark]
        public Uri OldVersion() => AbsoluteUri.OldMustBeAbsoluteUri(nameof(AbsoluteUri));
    }

    public static class MustBeAbsoluteUriExtensionMethods
    {
        public static Uri OldMustBeAbsoluteUri(this Uri uri, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);

            if (uri.IsAbsoluteUri)
                return uri;

            throw exception != null ? exception() : new ArgumentException(message ?? $"{parameterName ?? "The URI"} must be an absolute URI, but you specified \"{uri}\".", parameterName);
        }
    }
}