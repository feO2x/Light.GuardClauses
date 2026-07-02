using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class GeneratedFileBuildValidatorTests
{
    [Fact]
    public static void TryFindSourceValidationProjectFindsProjectFromRepositoryRoot()
    {
        var repositoryRoot = TestEnvironment.CodeDirectory.Parent!.FullName;
        var expectedProjectPath = Path.Combine(
            TestEnvironment.CodeDirectory.FullName,
            "Light.GuardClauses.SourceValidation",
            "Light.GuardClauses.SourceValidation.csproj"
        );

        GeneratedFileBuildValidator.TryFindSourceValidationProject([repositoryRoot]).Should().Be(expectedProjectPath);
    }

    [Fact]
    public static void ValidateReturnsNonZeroExitCodeAndLeavesFileOnBuildFailure()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { this is invalid }");

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.Net8_0, targetFile).Should().NotBe(0);
        File.Exists(targetFile).Should().BeTrue();
    }

    [Fact]
    public static void ValidateReturnsZeroForCompilingFileOnNet8()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { }");

        GeneratedFileBuildValidator.Validate(SourceTargetFramework.Net8_0, targetFile).Should().Be(0);
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
