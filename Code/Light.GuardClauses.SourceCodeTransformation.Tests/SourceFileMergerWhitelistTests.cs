using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class SourceFileMergerWhitelistTests
{
    private static readonly DirectoryInfo CodeDirectory = FindCodeDirectory();

    private static readonly DirectoryInfo SourceDirectory =
        new (Path.Combine(CodeDirectory.FullName, "Light.GuardClauses"));

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

    private static SourceFileMergeOptions CreateOptions(
        string targetFile,
        AssertionWhitelist assertionWhitelist = null
    ) =>
        new ()
        {
            SourceFolder = SourceDirectory.FullName,
            TargetFile = targetFile,
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

    private static DirectoryInfo FindCodeDirectory()
    {
        var currentDirectory = new DirectoryInfo(".");
        do
        {
            if (currentDirectory.Name == "Code")
            {
                return currentDirectory;
            }

            currentDirectory = currentDirectory.Parent;
        } while (currentDirectory != null);

        throw new InvalidOperationException(
            "This test project does not reside in a folder called \"Code\" (directly or indirectly)."
        );
    }

    private readonly record struct AssertionSelection(
        string AssertionName,
        bool IncludeExceptionFactoryOverload
    );

    private sealed class TemporaryDirectory : IDisposable
    {
        public TemporaryDirectory()
        {
            DirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(DirectoryPath);
        }

        public string DirectoryPath { get; }

        public void Dispose()
        {
            if (Directory.Exists(DirectoryPath))
            {
                Directory.Delete(DirectoryPath, true);
            }
        }
    }
}
