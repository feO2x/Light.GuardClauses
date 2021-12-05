using System;
using Microsoft.Extensions.Configuration;

namespace Light.GuardClauses.SourceCodeTransformation;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("Creating configuration...");
        var configuration = new ConfigurationBuilder().AddJsonFile("settings.json", true)
                                                      .AddCommandLine(args)
                                                      .Build();

        var options = new SourceFileMergeOptions();
        configuration.Bind(options);
        try
        {
            Console.WriteLine("Merging source files...");
            SourceFileMerger.CreateSingleSourceFile(options);
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
}