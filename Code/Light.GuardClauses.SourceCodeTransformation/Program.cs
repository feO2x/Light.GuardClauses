using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Light.GuardClauses.SourceCodeTransformation
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration =
                new ConfigurationBuilder()
                   .AddCommandLine(args)
                   .Build();

            var mergeOptionsBuilder = new SourceFileMergeOptionsBuilder();
            configuration.Bind(mergeOptionsBuilder);
            var merger = new SourceFileMerger(mergeOptionsBuilder.Build());
            await merger.CreateSingleSourceFileAsync();
        }
    }
}