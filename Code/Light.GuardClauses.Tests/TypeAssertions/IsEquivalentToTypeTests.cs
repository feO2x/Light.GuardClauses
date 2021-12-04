using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class IsEquivalentTypeToTests
{
    [Theory]
    [InlineData(typeof(string), typeof(string), true)]
    [InlineData(typeof(double), typeof(double), true)]
    [InlineData(typeof(int), typeof(object), false)]
    [InlineData(typeof(ArgumentException), typeof(Exception), false)]
    [InlineData(null, null, true)]
    [InlineData(null, typeof(object), false)]
    [InlineData(typeof(object), null, false)]
    public static void NonGenericTypes(Type type, Type other, bool expected) => 
        type.IsEquivalentTypeTo(other).Should().Be(expected);

    [Fact]
    public static void GenericTypeDefinition() => 
        TestEquivalence(typeof(List<>), typeof(List<>), true);

    [Fact]
    public static void ClosedConstructedGenericType() => 
        TestEquivalence(typeof(IList<string>), typeof(IList<string>), true);

    [Fact]
    public static void UnequalClosedConstructedGenericType() => 
        TestEquivalence(typeof(IReadOnlyList<string>), typeof(IReadOnlyList<object>), false);

    [Fact]
    public static void OpenConstructedGenericTypeAndGenericTypeDefinition() => 
        TestEquivalence(typeof(SubTypeA<>).GetTypeInfo().BaseType, typeof(GenericType<>), true);

    [Fact]
    public static void ClosedConstructedBaseType() => 
        TestEquivalence(typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<string>), true);

    [Fact]
    public static void ClosedConstructedBaseTypeAndGenericTypeDefinition() => 
        TestEquivalence(typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<>), true);

    [Fact]
    public static void DifferentTypes() => 
        TestEquivalence(typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(List<>), false);

    [Fact]
    public static void OpenConstructedReturnTypeAndGenericTypeDefinition() => 
        TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, typeof(Dictionary<,>), true);

    [Fact]
    public static void PartiallyConstructedReturnTypeAndGenericTypeDefinition() => 
        TestEquivalence(CreateDictionaryOfTKey.ReturnType, typeof(Dictionary<,>), true);

    [Fact]
    public static void DifferentConstructedGenericTypes() => 
        TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, CreateDictionaryOfTKey.ReturnType, false);

    private static void TestEquivalence(Type first, Type second, bool expected)
    {
        first.IsEquivalentTypeTo(second).Should().Be(expected);
        second.IsEquivalentTypeTo(first).Should().Be(expected);
    }

    public class GenericType<T> { }

    public class SubTypeA<T> : GenericType<T> { }

    public class SubTypeB : GenericType<string> { }

    public static Dictionary<TKey, TValue> CreateDictionaryA<TKey, TValue>() => new ();

    public static Dictionary<TKey, object> CreateDictionaryB<TKey>() => new ();

    private static readonly MethodInfo CreateDictionaryOfTKeyTValue;
    private static readonly MethodInfo CreateDictionaryOfTKey;

    static IsEquivalentTypeToTests()
    {
        var typeInfo = typeof(IsEquivalentTypeToTests).GetTypeInfo();
        CreateDictionaryOfTKeyTValue = typeInfo.DeclaredMethods.First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryA)));
        CreateDictionaryOfTKey = typeInfo.DeclaredMethods.First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryB)));
    }
}