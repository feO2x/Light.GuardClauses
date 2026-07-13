using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.UriAssertions
{
    public class MustHaveOneSchemeOfBenchmark
    {
        public List<string> Schemes = new List<string> { "http", "https" };
        public Uri Uri = new Uri("https://www.microsoft.com");

        [Benchmark(Baseline = true)]
        public Uri ImperativeVersion()
        {
            if (Uri == null) throw new ArgumentNullException(nameof(Uri));
            if (!Uri.IsAbsoluteUri) throw new RelativeUriException(nameof(Uri));
            if (!Schemes.Contains(Uri.Scheme)) throw new InvalidUriSchemeException(nameof(Uri));

            return Uri;
        }

        [Benchmark]
        public Uri LightGuardClauses() => Uri.MustHaveOneSchemeOf(Schemes, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesCustomException() => Uri.MustHaveOneSchemeOf(Schemes, (u, s) => new Exception("Where is the scheme?"));

        [Benchmark]
        public Uri OldVersion() => Uri.OldMustHaveOneSchemeOf(Schemes, nameof(Uri));
    }

    public static class MustHaveOneSchemeOfExtensionMethods
    {
        public static Uri OldMustHaveOneSchemeOf(this Uri uri, IEnumerable<string> schemes, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            uri.MustNotBeNull(parameterName);
            var schemesCollection = schemes.MustNotBeNullOrEmpty(nameof(schemes), "Your precondition is set up wrongly: schemes is null or an empty collection.").AsReadOnlyList();

            if (uri.IsAbsoluteUri == false)
                goto ThrowException;

            for (var i = 0; i < schemesCollection.Count; i++)
                if (string.Equals(schemesCollection[i], uri.Scheme, StringComparison.OrdinalIgnoreCase))
                    return uri;

            ThrowException:
            var subclause = uri.IsAbsoluteUri ? $"but actually has scheme \"{uri.Scheme}\" (\"{uri}\")." : $"but it has none because it is a relative URI (\"{uri}\").";
            throw exception != null
                      ? exception()
                      : new ArgumentException(message ?? new StringBuilder().Append($"{parameterName ?? "The URI"} must have one of the following schemes:")
                                                                            .AppendItemsWithNewLine(schemesCollection)
                                                                            .Append(subclause)
                                                                            .ToString());
        }
    }
}