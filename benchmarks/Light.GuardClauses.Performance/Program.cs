using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Light.GuardClauses.Performance;

public static class Program
{
    private const string EnableDisassemblyOption = "--enable-disassembly";

    private static IConfig CreateConfiguration(bool enableDisassembly)
    {
        var configuration = DefaultConfig
                           .Instance
                           .AddJob(Job.Default.WithRuntime(CoreRuntime.Core10_0))
                           .AddDiagnoser(MemoryDiagnoser.Default);

        return enableDisassembly ?
            configuration.AddDiagnoser(new DisassemblyDiagnoser(new DisassemblyDiagnoserConfig())) :
            configuration;
    }

    public static void Main(string[] arguments)
    {
        var enableDisassembly = Array.Exists(
            arguments,
            argument => string.Equals(argument, EnableDisassemblyOption, StringComparison.OrdinalIgnoreCase)
        );
        var benchmarkArguments = Array.FindAll(
            arguments,
            argument => !string.Equals(argument, EnableDisassemblyOption, StringComparison.OrdinalIgnoreCase)
        );

        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
                         .Run(benchmarkArguments, CreateConfiguration(enableDisassembly));
    }
}
