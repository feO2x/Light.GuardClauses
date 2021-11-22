using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions
{
    public static class ForEachTests
    {
        [Theory]
        [MemberData(nameof(PerformOnEachElementData))]
        public static void PerformOnEachElement(IEnumerable<string> collection)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var clonedCollection = new List<string>(collection);

            collection.ForEach(item => clonedCollection.Remove(item).Should().BeTrue());

            clonedCollection.Should().BeEmpty();
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TheoryData<IEnumerable<string>> PerformOnEachElementData =
            new()
            {
                new[] { "Foo", "Bar", "Baz" },
                new[] { "Foo" },
                new string[] { },
                LazyStringEnumerable()
            };

        private static IEnumerable<string> LazyStringEnumerable()
        {
            yield return "Foo";
            yield return "Bar";
            yield return "Baz";
        }

        [Theory]
        [MemberData(nameof(ItemNullData))]
        public static void ItemNullException(IEnumerable<object> collection)
        {
            Action act = () => collection.ForEach(_ => { });

            act.Should().Throw<CollectionException>();
        }

        [Theory]
        [MemberData(nameof(ItemNullData))]
        public static void ItemNullOmit(IEnumerable<object> collection)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var callCount = 0;

            collection.ForEach(_ => callCount++, false);

            callCount.Should().Be(collection.Count(item => item != null));
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TheoryData<IEnumerable<object>> ItemNullData =
            new()
            {
                new[] { new object(), new object(), null },
                LazyObjectEnumerableWithNull()
            };

        private static IEnumerable<object> LazyObjectEnumerableWithNull()
        {
            yield return new object();
            yield return null;
            yield return new object();
            yield return new object();
        }

        [Theory]
        [MemberData(nameof(ArgumentNullData))]
        public static void ArgumentNull(IEnumerable<int> collection, Action<int> action)
        {
            Action act = () => collection.ForEach(action);

            act.Should().Throw<ArgumentNullException>();
        }

        public static readonly TheoryData<IEnumerable<int>, Action<int>> ArgumentNullData =
            new()
            {
                { null, _ => { } },
                { new[] { 1, 2, 3 }, null }
            };
    }
}