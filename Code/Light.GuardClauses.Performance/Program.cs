using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Light.GuardClauses.Performance
{
    public static class Program
    {
        private static IConfig DefaultConfiguration =>
            DefaultConfig
               .Instance
               .AddJob(Job.Default.WithRuntime(CoreRuntime.Core70))
               .AddJob(Job.Default.WithRuntime(ClrRuntime.Net48))
               .AddDiagnoser(MemoryDiagnoser.Default, new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig()));

        public static void Main(string[] arguments) =>
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(arguments, DefaultConfiguration);
    }
}