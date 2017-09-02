using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class CollectionExtensionsTests
    {
        [Theory(DisplayName = "IndexOf must return the index of the item when the target collection contains it.")]
        [InlineData(new[] { 1, 2, 3 }, 2, 1)]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, 5, 4)]
        [InlineData(new[] { 42 }, 42, 0)]
        [InlineData(new[] { 42 }, 47, -1)]
        [InlineData(new string[] { }, "Foo", -1)]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, "Qux", -1)]
        public void IndexOf<T>(IReadOnlyList<T> collection, T item, int expectedIndex)
        {
            var index = collection.IndexOf(item);

            index.Should().Be(expectedIndex);
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