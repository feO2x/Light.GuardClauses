using System;
using System.Diagnostics;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class GeneratedFileBuildValidator
{
    public static int Validate(SourceTargetFramework targetFramework, string targetFile)
    {
        var absoluteTargetPath = Path.GetFullPath(targetFile);
        var targetDirectory = Path.GetDirectoryName(absoluteTargetPath);
        if (string.IsNullOrWhiteSpace(targetDirectory) || !Directory.Exists(targetDirectory))
        {
            Console.WriteLine("Generated file build validation skipped because the target directory does not exist.");
            return 0;
        }

        var projectPath = FindSourceValidationProject();
        if (projectPath == null)
        {
            Console.WriteLine(
                "Generated file build validation skipped because the Light.GuardClauses.SourceValidation project could not be found."
            );
            return 0;
        }

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

    private static string? FindSourceValidationProject()
    {
        var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (currentDirectory != null && currentDirectory.Name != "Code")
        {
            currentDirectory = currentDirectory.Parent;
        }

        if (currentDirectory == null)
        {
            return null;
        }

        var projectPath = Path.Combine(
            currentDirectory.FullName,
            "Light.GuardClauses.SourceValidation",
            "Light.GuardClauses.SourceValidation.csproj"
        );
        return File.Exists(projectPath) ? projectPath : null;
    }
}
