using System;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.UriAssertions
{
    public class MustBeRelativeUriBenchmark
    {
        public Uri RelativeUri = new Uri("/api/login", UriKind.Relative);

        [Benchmark(Baseline = true)]
        public Uri ImperativeVersion()
        {
            if (RelativeUri == null) throw new ArgumentNullException(nameof(RelativeUri));
            if (RelativeUri.IsAbsoluteUri) throw new AbsoluteUriException(nameof(RelativeUri));

            return RelativeUri;
        }

        [Benchmark]
        public Uri LightGuardClauses() => RelativeUri.MustBeRelativeUri(nameof(RelativeUri));

        [Benchmark]
        public Uri LightGuardClausesCustomException() => RelativeUri.MustBeRelativeUri(_ => new Exception("The URI is not relative"));
    }
}