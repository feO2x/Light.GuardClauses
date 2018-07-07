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
        [Theory(DisplayName = "ForEach must call the specified action for each element in the given enumerable.")]
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
            new TheoryData<IEnumerable<string>>
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

        [Theory(DisplayName = "ForEach must throw a CollectionException when the specified collection contains an item that is null and throwWhenItemIsNull is set to true.")]
        [MemberData(nameof(ItemNullData))]
        public static void ItemNullException(IEnumerable<object> collection)
        {
            Action act = () => collection.ForEach(item => { });

            act.Should().Throw<CollectionException>();
        }

        [Theory(DisplayName = "ForEach must not throw a CollectionException when the specified collection contains an item that is null and throwWhenItemIsNull is set to false.")]
        [MemberData(nameof(ItemNullData))]
        public static void ItemNullOmit(IEnumerable<object> collection)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var callCount = 0;

            collection.ForEach(item => callCount++, false);

            callCount.Should().Be(collection.Count(item => item != null));
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TheoryData<IEnumerable<object>> ItemNullData =
            new TheoryData<IEnumerable<object>>
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

        [Theory(DisplayName = "ForEach must throw an ArgumentNullException when either enumerable or action is null.")]
        [MemberData(nameof(ArgumentNullData))]
        public static void ArgumentNull(IEnumerable<int> collection, Action<int> action)
        {
            Action act = () => collection.ForEach(action);

            act.Should().Throw<ArgumentNullException>();
        }

        public static readonly TheoryData<IEnumerable<int>, Action<int>> ArgumentNullData =
            new TheoryData<IEnumerable<int>, Action<int>>
            {
                { null, item => { } },
                { new[] { 1, 2, 3 }, null }
            };
    }
}