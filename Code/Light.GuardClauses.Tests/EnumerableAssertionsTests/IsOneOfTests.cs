using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsOneOfTests
    {
        [Theory(DisplayName = "IsOneOf must return true when the specified item is part of the collection.")]
        [InlineData("Foo", new[] { "Bar", "Baz", "Foo" })]
        [InlineData("Foo", new[] { "Foo" })]
        [InlineData(1, new[] { 3, 1, 225, -554 })]
        public void IsOneOf<T>(T item, T[] items)
        {
            item.IsOneOf(items).Should().BeTrue();
        }

        [Theory(DisplayName = "IsOneOf must return false when the specified item is not part of the collection.")]
        [InlineData("Foo", new[] { "Bar", "Baz" })]
        [InlineData("Foo", new[] { "Qux", "Quux" })]
        [InlineData("Foo", new string[] { })]
        [InlineData(42, new[] { 1, 2, 3, 4 })]
        public void IsNotOneOf<T>(T item, T[] items)
        {
            item.IsOneOf(items).Should().BeFalse();
        }

        [Fact(DisplayName = "IsOneOf must throw an ArgumentNullException when items is null.")]
        public void CollectionNull()
        {
            Action act = () => 42.IsOneOf(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("items");
        }
    }
}