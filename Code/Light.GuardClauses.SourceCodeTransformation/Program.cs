using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("Creating configuration...");
            var configuration =
                new ConfigurationBuilder()
                   .AddJsonFile("settings.json", true)
                   .AddCommandLine(args)
                   .Build();

            var mergeOptionsBuilder = new SourceFileMergeOptions.Builder();
            configuration.Bind(mergeOptionsBuilder);
            var merger = new SourceFileMerger(mergeOptionsBuilder.Build());
            try
            {
                Console.WriteLine("Merging source files...");
                await merger.CreateSingleSourceFileAsync();
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