using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class ContainsOrdinalBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string NoSubstring = "age";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public bool Short() => ShortString.Contains(Substring, StringComparison.Ordinal);

        [Benchmark]
        public bool ShortNoSubstring() => ShortString.Contains(NoSubstring, StringComparison.Ordinal);

        [Benchmark]
        public bool Long() => LongString.Contains(Substring, StringComparison.Ordinal);

        [Benchmark]
        public bool LongNoSubstring() => LongString.Contains(NoSubstring, StringComparison.Ordinal);
    }

    public class ContainsCurrentCultureBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string NoSubstring = "age";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public bool Short() => ShortString.Contains(Substring, StringComparison.CurrentCulture);

        [Benchmark]
        public bool ShortNoSubstring() => ShortString.Contains(NoSubstring, StringComparison.CurrentCulture);

        [Benchmark]
        public bool Long() => LongString.Contains(Substring, StringComparison.CurrentCulture);

        [Benchmark]
        public bool LongNoSubstring() => LongString.Contains(NoSubstring, StringComparison.CurrentCulture);
    }

    public class ContainsCurrentCultureIgnoreCaseBenchmark
    {
        public string LongString = "A mind needs books as a sword needs a whetstone, if it is to keep its edge.";
        public string NoSubstring = "age";
        public string ShortString = "Edge";
        public string Substring = "dge";

        [Benchmark(Baseline = true)]
        public bool Short() => ShortString.Contains(Substring, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public bool ShortNoSubstring() => ShortString.Contains(NoSubstring, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public bool Long() => LongString.Contains(Substring, StringComparison.CurrentCultureIgnoreCase);

        [Benchmark]
        public bool LongNoSubstring() => LongString.Contains(NoSubstring, StringComparison.CurrentCultureIgnoreCase);
    }
}