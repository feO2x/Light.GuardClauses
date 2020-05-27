using System;
using Microsoft.Extensions.Configuration;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Creating configuration...");
            var configuration =
                new ConfigurationBuilder()
                   .AddJsonFile("settings.json", true)
                   .AddCommandLine(args)
                   .Build();

            var mergeOptionsBuilder = new SourceFileMergeOptions.Builder();
            configuration.Bind(mergeOptionsBuilder);
            try
            {
                Console.WriteLine("Merging source files...");
                SourceFileMerger.CreateSingleSourceFile(mergeOptionsBuilder.Build());
                Console.WriteLine("Source file export completed successfully.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured during source file export:");
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}