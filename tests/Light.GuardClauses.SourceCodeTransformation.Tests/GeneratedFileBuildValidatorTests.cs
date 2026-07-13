using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class GeneratedFileBuildValidatorTests
{
    [Fact]
    public static void TryFindSourceValidationProjectFindsProjectFromRepositoryRoot()
    {
        var expectedProjectPath = Path.Combine(
            TestEnvironment.RepositoryRoot.FullName,
            "tools",
            "source-export",
            "Light.GuardClauses.SourceValidation",
            "Light.GuardClauses.SourceValidation.csproj"
        );

        GeneratedFileBuildValidator.TryFindSourceValidationProject([TestEnvironment.RepositoryRoot.FullName])
            .Should().Be(expectedProjectPath);
    }

    [Fact]
    public static void TryFindSourceValidationProjectFindsProjectFromTestOutputDirectory()
    {
        GeneratedFileBuildValidator.TryFindSourceValidationProject([AppContext.BaseDirectory])
            .Should().NotBeNull();
    }

    [Fact]
    public static void SourceFileMergeOptionsUseRepositoryLayoutDefaults()
    {
        var options = new SourceFileMergeOptions();

        options.SourceFolder.Should().Be(TestEnvironment.SourceDirectory.FullName);
        options.TargetFile.Should().Be(
            Path.Combine(TestEnvironment.RepositoryRoot.FullName, "Light.GuardClauses.SingleFile.cs")
        );
    }

    [Fact]
    public static void ValidateReturnsNonZeroExitCodeAndLeavesFileOnBuildFailure()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { this is invalid }");

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.Net10_0, targetFile).Should().NotBe(0);
        File.Exists(targetFile).Should().BeTrue();
    }

    [Fact]
    public static void ValidateReturnsZeroForCompilingFileOnNet10()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { }");

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.Net10_0, targetFile).Should().Be(0);
    }

    [Fact]
    public static void ValidateReturnsZeroForCompilingFileOnNetStandard()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { }");

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.NetStandard2_0, targetFile).Should().Be(0);
    }

    [Fact]
    public static void ProgramRunsNonWhitelistNetStandardTransformationAndValidates()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var sourceFolder = TestEnvironment.SourceDirectory.FullName;
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");

        Program.Main(
            [
                $"SourceFolder={sourceFolder}",
                $"TargetFile={targetFile}",
                "IncludeVersionComment=false",
                "AssertionWhitelist:IsEnabled=false",
                "TargetFramework=NetStandard2_0",
            ]
        ).Should().Be(0);

        File.Exists(targetFile).Should().BeTrue();
    }
}
