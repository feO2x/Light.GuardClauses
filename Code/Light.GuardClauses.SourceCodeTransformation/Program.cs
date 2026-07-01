using System;
using Microsoft.Extensions.Configuration;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("Creating configuration...");
        var configuration = new ConfigurationBuilder().AddJsonFile("settings.json", true)
                                                      .AddJsonFile("settings.local.json", true)
                                                      .AddCommandLine(args)
                                                      .Build();

        var options = new SourceFileMergeOptions();
        configuration.Bind(options);
        options = ApplyDefaultPaths(options);
        try
        {
            Console.WriteLine("Merging source files...");
            SourceFileMerger.CreateSingleSourceFile(options);
            var buildValidationExitCode = GeneratedFileBuildValidator.Validate(options.TargetFile);
            if (buildValidationExitCode != 0)
            {
                return buildValidationExitCode;
            }

            Console.WriteLine("Source file export completed successfully.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred during source file export:");
            Console.WriteLine(ex);
            return -1;
        }
    }

    private static SourceFileMergeOptions ApplyDefaultPaths(SourceFileMergeOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.SourceFolder) &&
            !string.IsNullOrWhiteSpace(options.TargetFile))
        {
            return options;
        }

        var defaultOptions = new SourceFileMergeOptions();
        return options with
        {
            SourceFolder = string.IsNullOrWhiteSpace(options.SourceFolder) ? defaultOptions.SourceFolder : options.SourceFolder,
            TargetFile = string.IsNullOrWhiteSpace(options.TargetFile) ? defaultOptions.TargetFile : options.TargetFile
        };
    }
}
