using System;
using System.Diagnostics;
using System.IO;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class GeneratedFileBuildValidator
{
    public static int Validate(string targetFile)
    {
        var targetDirectory = Path.GetDirectoryName(Path.GetFullPath(targetFile));
        if (string.IsNullOrWhiteSpace(targetDirectory) || !Directory.Exists(targetDirectory))
        {
            Console.WriteLine("Generated file build validation skipped because the target directory does not exist.");
            return 0;
        }

        var projectFiles = Directory.GetFiles(targetDirectory, "*.csproj", SearchOption.TopDirectoryOnly);
        switch (projectFiles.Length)
        {
            case 0:
                Console.WriteLine(
                    "Generated file build validation skipped because no project file was found next to the target file."
                );
                return 0;

            case > 1:
                Console.WriteLine(
                    $"Warning: generated file build validation skipped because multiple project files were found in \"{targetDirectory}\"."
                );
                return 0;
        }

        Console.WriteLine($"Building generated project \"{projectFiles[0]}\"...");
        var startInfo = new ProcessStartInfo("dotnet", $"build \"{projectFiles[0]}\"")
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
}
