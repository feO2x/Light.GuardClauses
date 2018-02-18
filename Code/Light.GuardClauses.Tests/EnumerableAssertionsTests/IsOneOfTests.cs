using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class IsOneOfTests
    {
        [Theory(DisplayName = "IsOneOf must return true when the specified item is part of the collection.")]
        [InlineData("Foo", new[] { "Bar", "Baz", "Foo" })]
        [InlineData("Foo", new[] { "Foo" })]
        public void IsOneOf(string item, string[] items)
        {
            item.IsOneOf(items).Should().BeTrue();
        }

        [Theory(DisplayName = "IsOneOf must return false when the specified item is not part of the collection.")]
        [InlineData("Foo", new[] { "Bar", "Baz" })]
        [InlineData("Foo", new[] { "Qux", "Quux" })]
        [InlineData("Foo", new string[] { })]
        public void IsNotOneOf(string item, string[] items)
        {
            item.IsOneOf(items).Should().BeFalse();
        }

        [Fact(DisplayName = "IsOneOf must throw an ArgumentNullException when items is null.")]
        public void CollectionNull()
        {
            Action act = () => 42.IsOneOf(null);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("items");
        }
    }
}