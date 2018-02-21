using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.UriAssertions
{
    public class MustHaveOneSchemeOfBenchmarks : DefaultBenchmark
    {
        public Uri Uri;
        public string[] Array;
        public List<string> List;
        public IReadOnlyList<string> ReadOnlyListAbstraction;
        public IList<string> ListAbstraction;
        public ReadOnlyCollection<string> ReadOnlyCollection;
        public ObservableCollection<string> ObservableCollection;
        public IEnumerable<string> Enumerable;

        public MustHaveOneSchemeOfBenchmarks()
        {
            Uri = new Uri("ftps://www.myftpserver.com", UriKind.Absolute);
            Array = new[] { "http", "https", "file", "ssh", "ftp", "ftps" };
            List = new List<string>(Array);
            ReadOnlyListAbstraction = List;
            ListAbstraction = List;
            ReadOnlyCollection = new ReadOnlyCollection<string>(List);
            ObservableCollection = new ObservableCollection<string>(List);
            Enumerable = Array;
        }

        [Benchmark(Baseline = true)]
        public Uri LightGuardClausesArray() => Uri.MustHaveOneSchemeOf(Array, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesList() => Uri.MustHaveOneSchemeOf(List, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesReadOnlyListAbstraction() => Uri.MustHaveOneSchemeOf(ReadOnlyListAbstraction, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesListAbstraction() => Uri.MustHaveOneSchemeOf(ListAbstraction, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesReadOnlyCollection() => Uri.MustHaveOneSchemeOf(ReadOnlyCollection, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesObservableCollection() => Uri.MustHaveOneSchemeOf(ReadOnlyCollection, nameof(Uri));

        [Benchmark]
        public Uri LightGuardClausesEnumerable() => Uri.MustHaveOneSchemeOf(Enumerable, nameof(Uri));

        [Benchmark]
        public Uri OldVersion() => Uri.OldMustHaveOneSchemeOf(Array, nameof(Uri));
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
