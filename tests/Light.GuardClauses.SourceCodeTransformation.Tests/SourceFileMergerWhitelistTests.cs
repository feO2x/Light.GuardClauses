using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class SourceFileMergerWhitelistTests
{
    private static readonly DirectoryInfo SourceDirectory = TestEnvironment.SourceDirectory;

    [Fact]
    public static void DisabledWhitelistIgnoresAllEntriesAndProducesEquivalentOutput()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var baselineFile = Path.Combine(temporaryDirectory.DirectoryPath, "Baseline.cs");
        var disabledWhitelistFile = Path.Combine(temporaryDirectory.DirectoryPath, "DisabledWhitelist.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(baselineFile));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                disabledWhitelistFile,
                CreateWhitelist(
                    false,
                    [new ("MustBeGreaterThan", false)]
                )
            )
        );

        File.ReadAllText(disabledWhitelistFile).Should().Be(File.ReadAllText(baselineFile));
    }

    [Fact]
    public static void WhitelistModeTrimsUnrelatedHelpersAndPullsCrossAssertionDependencies()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeGreaterThan.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustBeGreaterThan", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustBeGreaterThan<T>");
        sourceCode.Should().Contain("MustNotBeNullReference<T>");
        sourceCode.Should().Contain("public static void MustBeGreaterThan");
        sourceCode.Should().NotContain("class EnumerableExtensions");
        sourceCode.Should().NotContain("struct Range<T>");
        sourceCode.Should().NotContain("Func<T, T, Exception> exceptionFactory");
    }

    [Fact]
    public static void WhitelistModePrunesBundledThrowMembersAndRetainsExceptionInheritance()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeAbsoluteUri.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustBeAbsoluteUri", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustBeAbsoluteUri(");
        sourceCode.Should().Contain("public static void MustBeAbsoluteUri");
        sourceCode.Should().NotContain("public static void MustBeRelativeUri");
        sourceCode.Should().NotContain("public static void UriMustHaveScheme");
        sourceCode.Should().Contain("class RelativeUriException : UriException");
        sourceCode.Should().Contain("class UriException : ArgumentException");
        sourceCode.Should().NotContain("class AbsoluteUriException : UriException");
    }

    [Fact]
    public static void WhitelistModeRetainsSupportClosures()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "SupportClosures.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("MustBeIn", false),
                        new ("MustBeValidEnumValue", false),
                        new ("MustBeEmailAddress", false),
                        new ("MustHaveLength", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("struct Range<T>");
        sourceCode.Should().Contain("class EnumInfo<T>");
        sourceCode.Should().Contain("class Types");
        sourceCode.Should().Contain("class RegularExpressions");
        sourceCode.Should().Contain("delegate Exception SpanExceptionFactory<TItem, in T>");
        sourceCode.Should().Contain("delegate Exception ReadOnlySpanExceptionFactory<TItem, in T>");
        sourceCode.Should().NotContain("GeneratedRegex");
    }

    [Fact]
    public static void PerAssertionExceptionFactoryTrimmingOnlyRemovesSelectedAssertionOverloads()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "ExceptionFactoryTrimming.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("MustHaveLength", false),
                        new ("MustBeGreaterThan", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().NotContain("Func<string?, int, Exception> exceptionFactory");
        sourceCode.Should().NotContain("SpanExceptionFactory<T, int> exceptionFactory");
        sourceCode.Should().Contain("Func<T, T, Exception> exceptionFactory");
        sourceCode.Should().Contain("public static void CustomException<T1, T2>");
    }

    [Fact]
    public static void AdditionalAssertionsRetainPortableSupportClosures()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "AdditionalPortableAssertions.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("IsUuidVersion7", false),
                        new ("MustBeUuidVersion7", true),
                        new ("MustHaveCountIn", true),
                        new ("MustBeAscii", true),
                        new ("MustNotBeEmptyOrWhiteSpace", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("struct GuidLayout");
        sourceCode.Should().Contain("StructLayout(LayoutKind.Explicit");
        sourceCode.Should().Contain("struct Range<T>");
        sourceCode.Should().Contain("class EnumerableExtensions");
        sourceCode.Should().Contain("delegate Exception ReadOnlySpanExceptionFactory<TItem>");
        sourceCode.Should().Contain("MustBeAscii(");
        sourceCode.Should().Contain("MustNotBeEmptyOrWhiteSpace(");
    }

    [Fact]
    public static void FiniteWhitelistUsesTargetSpecificSurface()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "FinitePortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "FiniteModern.cs");
        var whitelist = CreateWhitelist(
            includedAssertions:
            [
                new ("IsFinite", true),
                new ("MustBeFinite", true),
            ]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );

        File.ReadAllText(portableFile).Should().NotContain("IFloatingPointIeee754<T>");
        File.ReadAllText(modernFile).Should().Contain("IFloatingPointIeee754<T>");
    }

    [Fact]
    public static void SignGuardWhitelistsUseTargetSpecificSurface()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "SignGuardsPortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "SignGuardsModern.cs");
        var whitelist = CreateWhitelist(
            includedAssertions:
            [
                new ("MustBePositive", true),
                new ("MustBeNegative", true),
                new ("MustNotBePositive", true),
                new ("MustNotBeNegative", true),
                new ("MustNotBeZero", false),
            ]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );
        var portableCode = File.ReadAllText(portableFile);
        var modernCode = File.ReadAllText(modernFile);

        portableCode.Should().Contain("public static int MustBePositive(");
        portableCode.Should().Contain("public static decimal MustBeNegative(");
        portableCode.Should().Contain("public static TimeSpan MustNotBeNegative(");
        portableCode.Should().Contain("public static double MustNotBeZero(");
        portableCode.Should().Contain("MustBePositive(this int parameter, Func<int, Exception> exceptionFactory)");
        portableCode.Should().NotContain("MustNotBeZero(this int parameter, Func<int, Exception> exceptionFactory)");
        portableCode.Should().NotContain("INumber<T>");
        modernCode.Should().Contain("MustBePositive<T>");
        modernCode.Should().Contain("MustBeNegative<T>");
        modernCode.Should().Contain("MustNotBePositive<T>");
        modernCode.Should().Contain("MustNotBeNegative<T>");
        modernCode.Should().Contain("MustNotBeZero<T>");
        modernCode.Should().Contain("INumber<T>");
    }

    private static SourceFileMergeOptions CreateOptions(
        string targetFile,
        AssertionWhitelist assertionWhitelist = null,
        SourceTargetFramework targetFramework = SourceTargetFramework.NetStandard2_0
    ) =>
        new ()
        {
            SourceFolder = SourceDirectory.FullName,
            TargetFile = targetFile,
            TargetFramework = targetFramework,
            IncludeVersionComment = false,
            AssertionWhitelist = assertionWhitelist ?? new AssertionWhitelist(),
        };

    private static AssertionWhitelist CreateWhitelist(
        bool isEnabled = true,
        params AssertionSelection[] includedAssertions
    )
    {
        var whitelist = new AssertionWhitelist { IsEnabled = isEnabled };
        var selectedAssertions = includedAssertions.ToDictionary(
            selection => selection.AssertionName,
            StringComparer.Ordinal
        );

        foreach (var property in typeof(AssertionWhitelist).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                           .Where(
                                                                property => property.PropertyType ==
                                                                            typeof(AssertionWhitelist.AssertionEntry)
                                                            ))
        {
            var entry = selectedAssertions.TryGetValue(property.Name, out var selection) ?
                new AssertionWhitelist.AssertionEntry
                {
                    Include = true,
                    IncludeExceptionFactoryOverload = selection.IncludeExceptionFactoryOverload,
                } :
                new AssertionWhitelist.AssertionEntry { Include = false };
            property.SetValue(whitelist, entry);
        }

        return whitelist;
    }

    private readonly record struct AssertionSelection(
        string AssertionName,
        bool IncludeExceptionFactoryOverload
    );
}
