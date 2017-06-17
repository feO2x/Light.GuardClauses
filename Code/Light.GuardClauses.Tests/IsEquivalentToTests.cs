using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

// ReSharper disable UnusedTypeParameter

namespace Light.GuardClauses.Tests
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

        [Theory(DisplayName = "IsEquivalentTo must return true when one type is a constructed generic type and the other one is the corresponding generic type definition.")]
        [MemberData(nameof(GenericTypesData))]
        public void GenericTypes(Type type, Type other, bool expected)
        {
            var result = type.IsEquivalentTo(other);

            result.Should().Be(expected);
        }

        public static TestData GenericTypesData =>
            new[]
            {
                // Simple type tests
                new object[] { typeof(List<>), typeof(List<>), true },
                new object[] { typeof(IList<string>), typeof(IList<string>), true },

                // Base types of generic types
                new object[] { typeof(SubTypeA<>).GetTypeInfo().BaseType, typeof(GenericType<>), true },
                new object[] { typeof(GenericType<>), typeof(SubTypeA<>).GetTypeInfo().BaseType, true },
                new object[] { typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<int>), true },
                new object[] { typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<int>), true },
                new object[] { typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<>), true },
                new object[] { typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<string>), true },
                new object[] { typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<>), true },
                new object[] { typeof(SubTypeB).GetTypeInfo().BaseType, typeof(List<>), false },

                // Same list as above, but switched first and second parameter
                new object[] { typeof(GenericType<>), typeof(SubTypeA<>).GetTypeInfo().BaseType, true },
                new object[] { typeof(GenericType<>), typeof(SubTypeA<>).GetTypeInfo().BaseType, true },
                new object[] { typeof(GenericType<int>), typeof(SubTypeA<int>).GetTypeInfo().BaseType, true },
                new object[] { typeof(GenericType<int>), typeof(SubTypeA<int>).GetTypeInfo().BaseType, true },
                new object[] { typeof(GenericType<>), typeof(SubTypeA<int>).GetTypeInfo().BaseType, true },
                new object[] { typeof(GenericType<string>), typeof(SubTypeB).GetTypeInfo().BaseType, true },
                new object[] { typeof(GenericType<>), typeof(SubTypeB).GetTypeInfo().BaseType, true },
                new object[] { typeof(List<>), typeof(SubTypeB).GetTypeInfo().BaseType, false },

                // Return types of generic methods
                new object[] { CreateDictionaryOfTKeyTValue.ReturnType, typeof(Dictionary<,>), true },
                new object[] { CreateDictionaryOfTKey.ReturnType, typeof(Dictionary<,>), true },
                new object[] { CreateDictionaryOfTKey.ReturnType, CreateDictionaryOfTKeyTValue.ReturnType, false },

                // Same list as above, but switched first and second parameter
                new object[] { typeof(Dictionary<,>), CreateDictionaryOfTKeyTValue.ReturnType, true },
                new object[] { typeof(Dictionary<,>), CreateDictionaryOfTKey.ReturnType, true },
                new object[] { CreateDictionaryOfTKeyTValue.ReturnType, CreateDictionaryOfTKey.ReturnType, false }
            };

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