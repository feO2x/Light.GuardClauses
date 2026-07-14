using System;
using System.Buffers.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Light.GuardClauses.Performance.StringAssertions;

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class StringInspectionBenchmark
{
    public enum InspectionScenario
    {
        ShortSuccess,
        LongSuccess,
        EarlyFailure,
        MiddleFailure,
        LateFailure,
        MixedUnicodeSuccess,
    }

    private string _alphanumeric = null!;
    private string _digits = null!;

    public string SuccessfulGuardInput = new ('7', 256);

    [Params(
        InspectionScenario.ShortSuccess,
        InspectionScenario.LongSuccess,
        InspectionScenario.EarlyFailure,
        InspectionScenario.MiddleFailure,
        InspectionScenario.LateFailure,
        InspectionScenario.MixedUnicodeSuccess
    )]
    public InspectionScenario Scenario { get; set; }

    [GlobalSetup]
    public void Setup() =>
        (_digits, _alphanumeric) = Scenario switch
        {
            InspectionScenario.ShortSuccess => ("12345678", "Abcd1234"),
            InspectionScenario.LongSuccess => (new ('7', 4_096), new ('A', 4_096)),
            InspectionScenario.EarlyFailure => ("x" + new string('7', 4_095), "!" + new string('A', 4_095)),
            InspectionScenario.MiddleFailure =>
                (new string('7', 2_048) + "x" + new string('7', 2_047),
                 new string('A', 2_048) + "!" + new string('A', 2_047)),
            InspectionScenario.LateFailure => (new string('7', 4_095) + "x", new string('A', 4_095) + "!"),
            InspectionScenario.MixedUnicodeSuccess => (new ('٢', 4_096), new ('Ω', 4_096)),
            _ => throw new ArgumentOutOfRangeException(),
        };

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Digits")]
    public bool ImperativeDigits()
    {
        foreach (var character in _digits.AsSpan())
        {
            if (!char.IsDigit(character))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    [BenchmarkCategory("Digits")]
    public bool ContainsOnlyDigits() => _digits.AsSpan().ContainsOnlyDigits();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("LettersOrDigits")]
    public bool ImperativeLettersOrDigits()
    {
        foreach (var character in _alphanumeric.AsSpan())
        {
            if (!char.IsLetterOrDigit(character))
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    [BenchmarkCategory("LettersOrDigits")]
    public bool ContainsOnlyLettersOrDigits() => _alphanumeric.AsSpan().ContainsOnlyLettersOrDigits();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("StringPredicate")]
    public bool ImperativeStringDigits() => ImperativeDigits();

    [Benchmark]
    [BenchmarkCategory("StringPredicate")]
    public bool StringContainsOnlyDigits() => _digits.ContainsOnlyDigits();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("SuccessfulGuard")]
    public string ImperativeSuccessfulGuard()
    {
        foreach (var character in SuccessfulGuardInput.AsSpan())
        {
            if (!char.IsDigit(character))
            {
                throw new InvalidOperationException();
            }
        }

        return SuccessfulGuardInput;
    }

    [Benchmark]
    [BenchmarkCategory("SuccessfulGuard")]
    public string MustContainOnlyDigits() => SuccessfulGuardInput.MustContainOnlyDigits();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Base64")]
    public bool FrameworkBase64() => Base64.IsValid("VGhlIHF1aWNrIGJyb3duIGZveA==".AsSpan());

    [Benchmark]
    [BenchmarkCategory("Base64")]
    public bool IsBase64() => "VGhlIHF1aWNrIGJyb3duIGZveA==".IsBase64();

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("NoWhiteSpaceGuard")]
    public string ImperativeNoWhiteSpaceGuard()
    {
        foreach (var character in SuccessfulGuardInput.AsSpan())
        {
            if (char.IsWhiteSpace(character))
            {
                throw new InvalidOperationException();
            }
        }

        return SuccessfulGuardInput;
    }

    [Benchmark]
    [BenchmarkCategory("NoWhiteSpaceGuard")]
    public string MustNotContainWhiteSpace() => SuccessfulGuardInput.MustNotContainWhiteSpace();
}
