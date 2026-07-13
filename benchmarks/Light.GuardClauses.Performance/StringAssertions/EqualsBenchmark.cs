using System;
using BenchmarkDotNet.Attributes;

namespace Light.GuardClauses.Performance.StringAssertions
{
    public class EqualsOrdinalBenchmark
    {
        public string StringA = "When dead men and worse come hunting… you think it matters who sits on the Iron Throne?";
        public string StringB = "When dead men and worse come hunting… you think it matters who sits on the Iron Throne?";
        public string StringC = "When dead men and worse come hunting… you think it matters who sits on the washing machine?";

        [Benchmark(Baseline = true)]
        public bool ImperativeEqual() => string.Equals(StringA, StringB, StringComparison.Ordinal);

        [Benchmark]
        public bool ImperativeNotEqual() => string.Equals(StringA, StringC, StringComparison.Ordinal);

        [Benchmark]
        public bool LightGuardClausesEqual() => StringA.Equals(StringB, StringComparisonType.Ordinal);

        [Benchmark]
        public bool LightGuardClausesNotEqual() => StringA.Equals(StringC, StringComparisonType.Ordinal);
    }

    public class EqualsOrdinalIgnoreCaseBenchmark
    {
        public string StringA = "A drunken dwarf will never be the savior of the Seven Kingdoms.";
        public string StringB = "A DRUNKEN DWARF will never be the savior of the seven kingdoms.";
        public string StringC = "A drunken DWARF will never be the savior of the seven realms.";

        [Benchmark(Baseline = true)]
        public bool ImperativeEqual() => string.Equals(StringA, StringB, StringComparison.OrdinalIgnoreCase);

        [Benchmark]
        public bool ImperativeNotEqual() => string.Equals(StringA, StringC, StringComparison.OrdinalIgnoreCase);

        [Benchmark]
        public bool LightGuardClausesEqual() => StringA.Equals(StringB, StringComparisonType.OrdinalIgnoreCase);

        [Benchmark]
        public bool LightGuardClausesNotEqual() => StringA.Equals(StringC, StringComparisonType.OrdinalIgnoreCase);
    }

    public class EqualsCurrentCultureBenchmark
    {
        // I love these GoT quotes ;-)
        public string StringA = "If you ever call me sister again, I'll have you strangled in your sleep.";
        public string StringB = "If you ever call me sister again, I'll have you strangled in your sleep.";
        public string StringC = "If you ever call me sister again, I'll have you strangled and quartered.";

        [Benchmark(Baseline = true)]
        public bool ImperativeEqual() => string.Equals(StringA, StringB, StringComparison.CurrentCulture);

        [Benchmark]
        public bool ImperativeNotEqual() => string.Equals(StringA, StringC, StringComparison.CurrentCulture);

        [Benchmark]
        public bool LightGuardClausesEqual() => StringA.Equals(StringB, StringComparisonType.CurrentCulture);

        [Benchmark]
        public bool LightGuardClausesNotEqual() => StringA.Equals(StringC, StringComparisonType.CurrentCulture);
    }

    public class EqualsOrdinalIgnoreWhiteSpaceBenchmark
    {
        // ReSharper disable StringLiteralTypo
        public string JsonMinified = "{\"firstName\":\"Kenny\",\"lastName\":\"Pflug\",\"nickName\":\"feO2x\"}";
        public string JsonReadable = @"{
  ""firstName"": ""Kenny"",
  ""lastName"": ""Pflug"",
  ""nickName"": ""feO2x""
}";
        // ReSharper restore StringLiteralTypo

        [Benchmark]
        public bool LightGuardClauses() => JsonMinified.Equals(JsonReadable, StringComparisonType.OrdinalIgnoreWhiteSpace);
    }
}