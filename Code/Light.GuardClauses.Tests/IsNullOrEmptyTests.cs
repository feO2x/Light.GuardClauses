using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsNullOrEmptyTests
    {
        [Fact(DisplayName = "IsNullOrEmpty must return true when the specified collection is null.")]
        public void CollectionNull()
        {
            var collection = (IEnumerable<int>) null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = collection.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsNullOrEmpty must return true when the specified collection is empty.")]
        public void CollectionEmpty()
        {
            var emptyCollection = new int[0];

            var result = emptyCollection.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsNullOrEmpty must return false when the specified collection contains at least one item.")]
        public void CollectionNotEmpty()
        {
            var collection = new[] { "Foo", "Bar" };

            var result = collection.IsNullOrEmpty();

            result.Should().BeFalse();
        }

        [Fact(DisplayName = "IsNullOrEmpty must return true when the specified string is null.")]
        public void StringNull()
        {
            string @string = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            var result = @string.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsNullOrEmpty must return true when the specified string is an empty one.")]
        public void StringEmpty()
        {
            var result = string.Empty.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsNullOrEmpty must return false when the specified string is not empty.")]
        [InlineData("abcdef")]
        [InlineData("\t")]
        [InlineData("  ")]
        [InlineData("\u2028")]
        public void StringNotEmpty(string @string)
        {
            var result = @string.IsNullOrEmpty();

            result.Should().BeFalse();
        }
    }
}