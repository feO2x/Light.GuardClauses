using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class GeneratedFileBuildValidator
{
    private const string SourceValidationProjectFileName = "Light.GuardClauses.SourceValidation.csproj";

    public static int Validate(SourceTargetFramework targetFramework, string targetFile)
    {
        var absoluteTargetPath = Path.GetFullPath(targetFile);
        var targetDirectory = Path.GetDirectoryName(absoluteTargetPath);
        if (string.IsNullOrWhiteSpace(targetDirectory) || !Directory.Exists(targetDirectory))
        {
            Console.WriteLine("Generated file build validation skipped because the target directory does not exist.");
            return 0;
        }

        var projectPath = FindRequiredSourceValidationProject();

        var targetFrameworkMoniker = MapToTargetFrameworkMoniker(targetFramework);

        Console.WriteLine(
            $"Building generated project \"{projectPath}\" targeting {targetFrameworkMoniker} with \"{absoluteTargetPath}\"..."
        );
        var startInfo = new ProcessStartInfo(
            "dotnet",
            $"build \"{projectPath}\" -f {targetFrameworkMoniker} -p:GeneratedSourceFile=\"{absoluteTargetPath}\""
        )
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        using var process = Process.Start(startInfo) ??
                            throw new InvalidOperationException("Could not start dotnet build process.");
        var standardOutputTask = process.StandardOutput.ReadToEndAsync();
        var standardErrorTask = process.StandardError.ReadToEndAsync();
        process.WaitForExit();
        var standardOutput = standardOutputTask.GetAwaiter().GetResult();
        var standardError = standardErrorTask.GetAwaiter().GetResult();

        if (process.ExitCode == 0)
        {
            Console.WriteLine("Generated project build validation completed successfully.");
            return 0;
        }

        Console.WriteLine("Generated project build validation failed.");
        Console.WriteLine(standardOutput);
        if (!string.IsNullOrWhiteSpace(standardError))
        {
            Console.WriteLine(standardError);
        }

        return process.ExitCode;
    }

    private static string MapToTargetFrameworkMoniker(SourceTargetFramework targetFramework) =>
        targetFramework switch
        {
            SourceTargetFramework.Net8_0 => "net8.0",
            _ => "netstandard2.0",
        };

    private static string FindRequiredSourceValidationProject()
    {
        var searchRoots = new[]
        {
            AppContext.BaseDirectory,
            Directory.GetCurrentDirectory(),
        };

        var projectPath = TryFindSourceValidationProject(searchRoots);
        if (projectPath != null)
        {
            return projectPath;
        }

        throw new InvalidOperationException(
            $"Could not find \"{SourceValidationProjectFileName}\" by searching from " +
            $"\"{AppContext.BaseDirectory}\" and \"{Directory.GetCurrentDirectory()}\"."
        );
    }

    public static string? TryFindSourceValidationProject(IEnumerable<string> searchRoots)
    {
        var repositoryRoot = RepositoryLayout.TryFindRepositoryRoot(searchRoots);
        if (repositoryRoot == null)
        {
            return null;
        }

        var projectPath = Path.Combine(
            repositoryRoot.FullName,
            "tools",
            "source-export",
            "Light.GuardClauses.SourceValidation",
            SourceValidationProjectFileName
        );
        return File.Exists(projectPath) ? projectPath : null;
    }
}
