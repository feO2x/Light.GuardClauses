using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class IsSubstringOfOrdinalBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string NoSubstring = "age";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public bool Short() => Substring.IsSubstringOf(ShortString, StringComparison.Ordinal);

        [Benchmark]
        public bool ShortNoSubstring() => NoSubstring.IsSubstringOf(ShortString, StringComparison.Ordinal);

        [Benchmark]
        public bool Long() => Substring.IsSubstringOf(LongString, StringComparison.Ordinal);

        [Benchmark]
        public bool LongNoSubstring() => NoSubstring.IsSubstringOf(LongString, StringComparison.Ordinal);
    }

    public class IsSubstringOfCurrentCultureBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string NoSubstring = "age";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public bool Short() => Substring.IsSubstringOf(ShortString, StringComparison.CurrentCulture);

        [Benchmark]
        public bool ShortNoSubstring() => NoSubstring.IsSubstringOf(ShortString, StringComparison.CurrentCulture);

        [Benchmark]
        public bool Long() => Substring.IsSubstringOf(LongString, StringComparison.CurrentCulture);

        [Benchmark]
        public bool LongNoSubstring() => NoSubstring.IsSubstringOf(LongString, StringComparison.CurrentCulture);
    }

    public class IsSubstringOfCurrentCultureIgnoreCaseBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string NoSubstring = "age";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public bool Short() => Substring.IsSubstringOf(ShortString, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public bool ShortNoSubstring() => NoSubstring.IsSubstringOf(ShortString, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public bool Long() => Substring.IsSubstringOf(LongString, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public bool LongNoSubstring() => NoSubstring.IsSubstringOf(LongString, StringComparison.CurrentCultureIgnoreCase);
    }
}