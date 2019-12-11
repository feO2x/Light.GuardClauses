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
               .With(Job.Default.With(ClrRuntime.Net48))
               .With(Job.Default.With(CoreRuntime.Core31))
               .With(MemoryDiagnoser.Default)
               .With(DisassemblyDiagnoser.Create(DisassemblyDiagnoserConfig.Asm));

        public static void Main(string[] arguments) =>
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(arguments, DefaultConfiguration);
    }
}