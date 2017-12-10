using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

// ReSharper disable UnusedTypeParameter

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class IsEquivalentTypeToTests
    {
        [Theory(DisplayName = "IsEquivalentTypeTo must return true when two non-generic types are passed in that are equal, else false must be returned.")]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(double), typeof(double), true)]
        [InlineData(typeof(int), typeof(object), false)]
        [InlineData(typeof(ArgumentException), typeof(Exception), false)]
        [InlineData(null, null, true)]
        [InlineData(null, typeof(object), false)]
        [InlineData(typeof(object), null, false)]
        public void NonGenericTypes(Type type, Type other, bool expected)
        {
            var result = type.IsEquivalentTypeTo(other);

            result.Should().Be(expected);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when two generic type definitions of the same type are passed in.")]
        public void GenericTypeDefinition()
        {
            TestEquivalence(typeof(List<>), typeof(List<>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when two closed constructed generic types are passed in that are equal.")]
        public void ClosedConstructedGenericType()
        {
            TestEquivalence(typeof(IList<string>), typeof(IList<string>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return false when two closed constructed generic types are passed in that are not equal.")]
        public void UnequalClosedConstructedGenericType()
        {
            TestEquivalence(typeof(IReadOnlyList<string>), typeof(IReadOnlyList<object>), false);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when the open constructed generic type (base class) is compared to the generic type definition.")]
        public void OpenConstructedGenericTypeAndGenericTypeDefinition()
        {
            TestEquivalence(typeof(SubTypeA<>).GetTypeInfo().BaseType, typeof(GenericType<>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when the closed constructed generic type (base class) is compared to the closed constructed generic type. ")]
        public void ClosedConstructedBaseType()
        {
            TestEquivalence(typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<string>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when the closed constructed generic type (base class) is compared to the generic type definition.")]
        public void ClosedConstructedBaseTypeAndGenericTypeDefinition()
        {
            TestEquivalence(typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return false when the closed constructed generic type (base class) is compared to another generic type definition.")]
        public void DifferentTypes()
        {
            TestEquivalence(typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(List<>), false);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when a open constructed generic type (return type) is compared to the generic type definition.")]
        public void OpenConstructedReturnTypeAndGenericTypeDefinition()
        {
            TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, typeof(Dictionary<,>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return true when a partially open constructed generic type (return type) is compared to the generic type definition.")]
        public void PartiallyConstructedReturnTypeAndGenericTypeDefinition()
        {
            TestEquivalence(CreateDictionaryOfTKey.ReturnType, typeof(Dictionary<,>), true);
        }

        [Fact(DisplayName = "IsEquivalentTypeTo must return false when two different open constructed generic types having the same generic type definition are compared.")]
        public void DifferentConstructedGenericTypes()
        {
            TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, CreateDictionaryOfTKey.ReturnType, false);
        }

        private static void TestEquivalence(Type first, Type second, bool expected)
        {
            first.IsEquivalentTypeTo(second).Should().Be(expected);
            second.IsEquivalentTypeTo(first).Should().Be(expected);
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

        static IsEquivalentTypeToTests()
        {
            var typeInfo = typeof(IsEquivalentTypeToTests).GetTypeInfo();
            CreateDictionaryOfTKeyTValue = typeInfo.DeclaredMethods.First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryA)));
            CreateDictionaryOfTKey = typeInfo.DeclaredMethods.First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryB)));
        }
    }
}