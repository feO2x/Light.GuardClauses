using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
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
        [InlineData(typeof(List<>), typeof(List<>), true)]
        [InlineData(typeof(IList<string>), typeof(IList<string>), true)]
        [MemberData(nameof(GenericTypesData))]
        public void GenericTypes(Type type, Type other, bool expected)
        {
            var result = type.IsEquivalentTo(other);

            result.Should().Be(expected);
        }

        public static readonly TestData GenericTypesData =
            new[]
            {
                new object[] { typeof(SubTypeA<>).GetTypeInfo().BaseType, typeof(GenericType<>), true },
                new object[] { typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<int>), true },
                new object[] { typeof(SubTypeA<int>).GetTypeInfo().BaseType, typeof(GenericType<>), true },
                new object[] { typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<string>), true },
                new object[] { typeof(SubTypeB).GetTypeInfo().BaseType, typeof(GenericType<>), true },
                new object[] { typeof(SubTypeB).GetTypeInfo().BaseType, typeof(List<>), false}
            };

        public class GenericType<T> { }

        public class SubTypeA<T> : GenericType<T> { }

        public class SubTypeB : GenericType<string> { }
    }
}