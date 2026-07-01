using System;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class GeneratedFileBuildValidatorTests
{
    [Fact]
    public static void ValidateBuildsSingleProjectNextToGeneratedFile()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { }");
        File.WriteAllText(
            Path.Combine(temporaryDirectory.DirectoryPath, "ValidationSample.csproj"),
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
                <ImplicitUsings>enable</ImplicitUsings>
                <Nullable>enable</Nullable>
              </PropertyGroup>
            </Project>
            """
        );

        GeneratedFileBuildValidator.Validate(targetFile).Should().Be(0);
    }

    [Fact]
    public static void ValidateReturnsNonZeroExitCodeAndLeavesFileOnBuildFailure()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { this is invalid }");
        File.WriteAllText(
            Path.Combine(temporaryDirectory.DirectoryPath, "ValidationSample.csproj"),
            """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net8.0</TargetFramework>
              </PropertyGroup>
            </Project>
            """
        );

        GeneratedFileBuildValidator.Validate(targetFile).Should().NotBe(0);
        File.Exists(targetFile).Should().BeTrue();
    }

    [Fact]
    public static void ValidateSkipsWhenNoProjectFileExists()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { }");

        GeneratedFileBuildValidator.Validate(targetFile).Should().Be(0);
    }

    [Fact]
    public static void ValidateSkipsWhenMultipleProjectFilesExist()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");
        File.WriteAllText(targetFile, "namespace ValidationSample; public static class Generated { }");
        File.WriteAllText(
            Path.Combine(temporaryDirectory.DirectoryPath, "First.csproj"),
            "<Project Sdk=\"Microsoft.NET.Sdk\" />"
        );
        File.WriteAllText(
            Path.Combine(temporaryDirectory.DirectoryPath, "Second.csproj"),
            "<Project Sdk=\"Microsoft.NET.Sdk\" />"
        );

        GeneratedFileBuildValidator.Validate(targetFile).Should().Be(0);
    }

    [Fact]
    public static void ProgramRunsNonWhitelistTransformationAndSkipsValidationWithoutProject()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var codeDirectory = FindCodeDirectory();
        var sourceFolder = Path.Combine(codeDirectory.FullName, "Light.GuardClauses");
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "Generated.cs");

        Program.Main(
            [
                $"SourceFolder={sourceFolder}",
                $"TargetFile={targetFile}",
                "IncludeVersionComment=false",
                "AssertionWhitelist:IsEnabled=false",
            ]
        ).Should().Be(0);

        File.Exists(targetFile).Should().BeTrue();
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
