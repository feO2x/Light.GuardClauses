using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

// ReSharper disable UnusedTypeParameter

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsEquivalentToTests
    {
        [Theory(DisplayName = "IsEquivalentTo must return true when two non-generic types are passed in that are equal, else false must be returned.")]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(double), typeof(double), true)]
        [InlineData(typeof(int), typeof(object), false)]
        [InlineData(typeof(ArgumentException), typeof(Exception), false)]
        [InlineData(null, null, true)]
        [InlineData(null, typeof(object), false)]
        [InlineData(typeof(object), null, false)]
        public void NonGenericTypes(Type type, Type other, bool expected)
        {
            var result = type.IsEquivalentTo(other);

            result.Should().Be(expected);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when two generic type definitions of the same type are passed in.")]
        public void GenericTypeDefinition()
        {
            TestEquivalence(typeof(List<>), typeof(List<>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when two closed bound generic types are passed in that are equal.")]
        public void ClosedBoundGenericType()
        {
            TestEquivalence(typeof(IList<string>), typeof(IList<string>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return false when two closed bound generic types are passed in that are not equal.")]
        public void UnequalClosedBoundGenericType()
        {
            TestEquivalence(typeof(IReadOnlyList<string>), typeof(IReadOnlyList<object>), false);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when the open bound generic type (base class) is compared to the generic type definition.")]
        public void OpenBoundGenericTypeAndGenericTypeDefinition()
        {
            TestEquivalence(typeof(SubTypeA<>).GetTypeInfo().BaseType, typeof(GenericType<>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when the closed bound generic type (base class) is compared to the closed bound generic type. ")]
        public void ClosedBoundBaseType()
        {
            TestEquivalence(typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<string>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when the closed bound generic type (base class) is compared to the generic type definition.")]
        public void ClosedBoundBaseTypeAndGenericTypeDefinition()
        {
            TestEquivalence(typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return false when the closed bound generic type (base class) is compared to another generic type definition.")]
        public void DifferentTypes()
        {
            TestEquivalence(typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(List<>), false);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when a open bound generic type (return type) is compared to the generic type definition.")]
        public void OpenBoundReturnTypeAndGenericTypeDefinition()
        {
            TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, typeof(Dictionary<,>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return true when a partially open bound generic type (return type) is compared to the generic type definition.")]
        public void PartiallyOpenBoundReturnTypeAndGenericTypeDefinition()
        {
            TestEquivalence(CreateDictionaryOfTKey.ReturnType, typeof(Dictionary<,>), true);
        }

        [Fact(DisplayName = "IsEquivalentTo must return false when two different open bound generic types having the same generic type definition are compared.")]
        public void DifferentOpenBoundGenericTypes()
        {
            TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, CreateDictionaryOfTKey.ReturnType, false);
        }

        private static void TestEquivalence(Type first, Type second, bool expected)
        {
            first.IsEquivalentTo(second).Should().Be(expected);
            second.IsEquivalentTo(first).Should().Be(expected);
        }

        public class GenericType<T> { }

        public class SubTypeA<T> : GenericType<T> { }

        public class SubTypeB : GenericType<string> { }

        public static Dictionary<TKey, TValue> CreateDictionaryA<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        public static Dictionary<TKey, object> CreateDictionaryB<TKey>()
        {
            return new Dictionary<TKey, object>();
        }

        private static readonly MethodInfo CreateDictionaryOfTKeyTValue;
        private static readonly MethodInfo CreateDictionaryOfTKey;

        static IsEquivalentToTests()
        {
            var typeInfo = typeof(IsEquivalentToTests).GetTypeInfo();
            CreateDictionaryOfTKeyTValue = typeInfo.DeclaredMethods.First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryA)));
            CreateDictionaryOfTKey = typeInfo.DeclaredMethods.First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryB)));
        }
    }
}