using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsSubsetOfTests
    {
        [Theory(DisplayName = "IsSubsetOf must return true when all items of the specified collection are part of the superset collection.")]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, new[] { "Foo", "Bar", "Baz", "Qux" }, true)]
        [InlineData(new[] { "Qux", "Baz" }, new[] { "Foo", "Bar", "Baz", "Qux" }, true)]
        [InlineData(new[] { "Corge", "Baz" }, new[] { "Foo", "Bar", "Baz", "Qux" }, false)]
        [InlineData(new[] { 1, 3, 5 }, new[] { 1, 4, 5, 7, 9 }, false)]
        [InlineData(new string[] { }, new string[] { }, true)]
        public void CheckIsSubsetOf<T>(T[] collection, T[] superset, bool expected)
        {
            collection.IsSubsetOf(superset).Should().Be(expected);
        }

        [Theory(DisplayName = "IsSubsetOf must throw an ArgumentNullException when the any of the specified collections are null.")]
        [InlineData(null, new string[] { })]
        [InlineData(new string[] { }, null)]
        public void ArgumentNull(string[] collection, string[] superset)
        {
            Action act = () => collection.IsSubsetOf(superset);

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}