using System;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Light.GuardClauses.Performance
{
    [DisassemblyDiagnoser]
    public static class Program
    {
        private static IConfig DefaultConfiguration =>
            DefaultConfig
               .Instance
               .With(Job.Clr, Job.Core)
               .With(MemoryDiagnoser.Default)
               .With(DisassemblyDiagnoser.Create(DisassemblyDiagnoserConfig.Asm));

        public static void Main(string[] arguments)
        {
            Console.WriteLine($"Benchmarking started on {DateTimeOffset.Now}");
            var stopWatch = Stopwatch.StartNew();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(arguments, DefaultConfiguration);
            stopWatch.Stop();
            Console.WriteLine($"Benchmarking finished on {DateTimeOffset.Now}");
            Console.WriteLine($"It took {stopWatch.Elapsed:g}");
        }
    }
}