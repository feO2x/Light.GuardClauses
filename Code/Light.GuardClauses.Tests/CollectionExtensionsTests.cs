using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class CollectionExtensionsTests
    {
        [Theory(DisplayName = "IndexOf must return the index of the item when the target collection contains it.")]
        [InlineData(new[] { 1, 2, 3 }, 2, 1)]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, 5, 4)]
        [InlineData(new[] { 42 }, 42, 0)]
        [InlineData(new[] { 42 }, 47, -1)]
        [InlineData(new int[] { }, 42, -1)]
        [InlineData(new[] { 1, 2, 3 }, 4, -1)]
        public void IndexOf(IReadOnlyList<int> collection, int item, int expectedIndex)
        {
            var index = collection.IndexOf(item);

            index.Should().Be(expectedIndex);
        }

        [Theory(DisplayName = "IsContaining must return true if the corresponding item was found, else false.")]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, "Baz", true)]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, "Bar", true)]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, "Foo", true)]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, "Qux", false)]
        [InlineData(new[] { "Foo", "Bar" }, "Baz", false)]
        [InlineData(new[] { "Foo", "Bar", "Baz", "Qux" }, "Foo", true)]
        [InlineData(new string[] { }, "Foo", false)]
        public void IsContaining(IReadOnlyList<string> collection, string item, bool expected)
        {
            collection.IsContaining(item).Should().Be(expected);
        }

        [Fact(DisplayName = "IsContaining must be able to be used with a custom equality comparer.")]
        public void ContainsWithEqualityComparer()
        {
            IReadOnlyList<string> collection = new[] { "Foo", "Bar", "Baz" };
            var equalityComparerSpy = new EqualityComparerSpy<string>();

            var result = collection.IsContaining("Bar", equalityComparerSpy);

            result.Should().BeTrue();
            equalityComparerSpy.EqualsCallCount.Should().BeGreaterThan(0);
            equalityComparerSpy.GetHashCodeCallCount.Should().BeGreaterThan(0);
        }
    }
}