using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class SourceFileMergerFrameworkTests
{
    [Fact]
    public static void Net10ExportContainsModernMembersAndNoDirectives()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Net10Export.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(targetFile, SourceTargetFramework.Net10_0));
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("using System.Numerics;");
        sourceCode.Should().Contain("using System.Buffers.Text;");
        sourceCode.Should().Contain("INumber<T>");
        sourceCode.Should().Contain("IFloatingPointIeee754<T>");
        sourceCode.Should().Contain("Ascii.IsValid(parameter)");
        sourceCode.Should().Contain("Base64.IsValid(parameter)");
        sourceCode.Should().Contain("char.IsAsciiHexDigit(character)");
        sourceCode.Should().Contain("MustBeGreaterThanOrApproximately<T>");
        sourceCode.Should().Contain("public static ReadOnlySpan<char> MustBeEmailAddress");
        sourceCode.Should().Contain("public static Span<char> MustBeEmailAddress");
        sourceCode.Should().Contain("public static Memory<char> MustBeEmailAddress");
        sourceCode.Should().NotContain("#if");
    }

    [Fact]
    public static void NetStandardExportOmitsModernMembersAndNoDirectives()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "NetStandardExport.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(targetFile, SourceTargetFramework.NetStandard2_0));
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().NotContain("using System.Numerics;");
        sourceCode.Should().NotContain("using System.Buffers.Text;");
        sourceCode.Should().NotContain("INumber<T>");
        sourceCode.Should().NotContain("IFloatingPointIeee754<T>");
        sourceCode.Should().NotContain("Ascii.IsValid(parameter)");
        sourceCode.Should().NotContain("Base64.IsValid(parameter)");
        sourceCode.Should().NotContain("char.IsAsciiHexDigit(character)");
        sourceCode.Should().Contain("IsBase64Portable(parameter)");
        sourceCode.Should().Contain("private static bool IsAsciiHexDigit(char character)");
        sourceCode.Should().NotContain("ReadOnlySpan<char> MustBeEmailAddress");
        sourceCode.Should().NotContain("Span<char> MustBeEmailAddress");
        sourceCode.Should().NotContain("Memory<char> MustBeEmailAddress");
        sourceCode.Should().NotContain("#if");
    }

    [Fact]
    public static void Net10ExportOmitsPolyfillAttributes()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Net10Polyfill.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(targetFile, SourceTargetFramework.Net10_0));
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().NotContain("class AllowNullAttribute");
        sourceCode.Should().NotContain("class CallerArgumentExpressionAttribute");
    }

    [Fact]
    public static void NetStandardExportIncludesPolyfillAttributes()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "NetStandardPolyfill.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(targetFile, SourceTargetFramework.NetStandard2_0));
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("class AllowNullAttribute");
        sourceCode.Should().Contain("class CallerArgumentExpressionAttribute");
    }

    [Fact]
    public static void Net10ExportValidatesAgainstNet10()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Net10Validation.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(targetFile, SourceTargetFramework.Net10_0));

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.Net10_0, targetFile).Should().Be(0);
    }

    [Fact]
    public static void NetStandardExportValidatesAgainstNetStandard()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "NetStandardValidation.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(targetFile, SourceTargetFramework.NetStandard2_0));

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.NetStandard2_0, targetFile).Should().Be(0);
    }

    [Fact]
    public static void SyntheticAssertionFileWithNoWhitelistPropertyThrows()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var sourceFolder = Path.Combine(temporaryDirectory.DirectoryPath, "Source");
        Directory.CreateDirectory(sourceFolder);
        File.WriteAllText(
            Path.Combine(sourceFolder, "Check.SyntheticNotWhitelisted.cs"),
            """
            namespace Light.GuardClauses;

            public static partial class Check
            {
                public static T SyntheticNotWhitelisted<T>(T parameter) => parameter;
            }
            """
        );
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Synthetic.cs");
        var options = new SourceFileMergeOptions
        {
            SourceFolder = sourceFolder,
            TargetFile = targetFile,
            TargetFramework = SourceTargetFramework.NetStandard2_0,
            IncludeVersionComment = false,
            AssertionWhitelist = new () { IsEnabled = true },
        };

        var act = () => SourceFileMerger.CreateSingleSourceFile(options);

        act.Should().Throw<InvalidOperationException>().WithMessage("*SyntheticNotWhitelisted*");
    }

    private static SourceFileMergeOptions CreateOptions(string targetFile, SourceTargetFramework targetFramework) =>
        new ()
        {
            SourceFolder = TestEnvironment.SourceDirectory.FullName,
            TargetFile = targetFile,
            TargetFramework = targetFramework,
            IncludeVersionComment = false,
        };
}
