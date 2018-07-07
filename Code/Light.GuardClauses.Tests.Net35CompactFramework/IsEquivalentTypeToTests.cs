using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Light.GuardClauses.Tests.Net35CompactFramework
{
    [TestFixture]
    public class IsEquivalentTypeToTests
    {
        [Test]
        public void NonGenericTypes()
        {
            TestEquivalence(typeof(string), typeof(string), true);
            TestEquivalence(typeof(double), typeof(double), true);
            TestEquivalence(typeof(int), typeof(object), false);
            TestEquivalence(typeof(ArgumentException), typeof(Exception), false);
            TestEquivalence(null, null, true);
            TestEquivalence(null, typeof(object), false);
            TestEquivalence(typeof(object), null, false);
        }

        [Test]
        public void GenericTypeDefinition() =>
            TestEquivalence(typeof(List<>), typeof(List<>), true);

        [Test]
        public void ClosedConstructedGenericType() =>
            TestEquivalence(typeof(IList<string>), typeof(IList<string>), true);

        [Test]
        public void UnequalClosedConstructedGenericType() =>
            TestEquivalence(typeof(ICollection<string>), typeof(ICollection<object>), false);

        [Test]
        public void OpenConstructedGenericTypeAndGenericTypeDefinition() =>
            TestEquivalence(typeof(SubTypeA<>).BaseType, typeof(GenericType<>), true);

        [Test]
        public void ClosedConstructedBaseType() =>
            TestEquivalence(typeof(SubTypeB).BaseType, typeof(GenericType<string>), true);

        [Test]
        public void ClosedConstructedBaseTypeAndGenericTypeDefinition() =>
            TestEquivalence(typeof(SubTypeA<int>).BaseType, typeof(GenericType<>), true);

        [Test]
        public void DifferentTypes() =>
            TestEquivalence(typeof(SubTypeA<int>).BaseType, typeof(List<>), false);

        [Test]
        public void OpenConstructedReturnTypeAndGenericTypeDefinition() =>
            TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, typeof(Dictionary<,>), true);

        [Test]
        public void PartiallyConstructedReturnTypeAndGenericTypeDefinition() =>
           TestEquivalence(CreateDictionaryOfTKey.ReturnType, typeof(Dictionary<,>), true);

        [Test]
        public void DifferentConstructedGenericTypes() =>
            TestEquivalence(CreateDictionaryOfTKeyTValue.ReturnType, CreateDictionaryOfTKey.ReturnType, false);

        public class GenericType<T> { }

        public class SubTypeA<T> : GenericType<T> { }

        public class SubTypeB : GenericType<string> { }

        public static Dictionary<TKey, TValue> CreateDictionaryA<TKey, TValue>() =>
            new Dictionary<TKey, TValue>();

        public static Dictionary<TKey, object> CreateDictionaryB<TKey>() =>
            new Dictionary<TKey, object>();

        private static readonly MethodInfo CreateDictionaryOfTKeyTValue;
        private static readonly MethodInfo CreateDictionaryOfTKey;

        static IsEquivalentTypeToTests()
        {
            var thisType = typeof(IsEquivalentTypeToTests);
            CreateDictionaryOfTKeyTValue = thisType.GetMethods().First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryA)));
            CreateDictionaryOfTKey = thisType.GetMethods().First(m => m.IsStatic && m.Name.StartsWith(nameof(CreateDictionaryB)));
        }

        private static void TestEquivalence(Type first, Type second, bool expected)
        {
            Assert.AreEqual(expected, first.IsEquivalentTypeTo(second));
            Assert.AreEqual(expected, second.IsEquivalentTypeTo(first));
        }
    }
}
