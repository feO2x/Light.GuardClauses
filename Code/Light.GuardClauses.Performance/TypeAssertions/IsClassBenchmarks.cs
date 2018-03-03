using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses.Performance.TypeAssertions
{
    public class IsClassBenchmarks : DefaultBenchmark
    {
        public Type ClassType = typeof(string);
        public Type DelegateType = typeof(Action);
        public Type StructType = typeof(int);

        [Benchmark(Baseline = true)]
        public bool ImperativeVersionClass() => ClassType.IsClass && ClassType.BaseType != Types.MulticastDelegateType;

        [Benchmark]
        public bool ImperativeVersionDelegate() => DelegateType.IsClass && DelegateType.BaseType != Types.MulticastDelegateType;

        [Benchmark]
        public bool ImperativeVersionStruct() => StructType.IsClass && StructType.BaseType != Types.MulticastDelegateType;

        [Benchmark]
        public bool LightGuardClausesClass() => ClassType.IsClass();

        [Benchmark]
        public bool LightGuardClausesDelegate() => DelegateType.IsClass();

        [Benchmark]
        public bool LightGuardClausesStruct() => StructType.IsClass();

        [Benchmark]
        public bool OldClausesClass() => ClassType.OldIsClass();

        [Benchmark]
        public bool OldClausesDelegate() => DelegateType.OldIsClass();

        [Benchmark]
        public bool OldClausesStruct() => StructType.OldIsClass();
    }

    public static class IsClassExtensions
    {
        public static bool OldIsClass(this Type type)
        {
            return type.GetTypeInfo().OldIsClass();
        }

        public static bool OldIsClass(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsClass && typeInfo.BaseType != Types.MulticastDelegateType;
        }
    }
}