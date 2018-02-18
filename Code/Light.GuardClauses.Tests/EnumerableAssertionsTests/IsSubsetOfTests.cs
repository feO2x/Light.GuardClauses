using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class IsSubsetOfTests
    {
        [Theory(DisplayName = "IsSubsetOf must return true when all items of the specified collection are part of the superset collection.")]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, new[] { "Foo", "Bar", "Baz", "Qux" }, true)]
        [InlineData(new[] { "Qux", "Baz" }, new[] { "Foo", "Bar", "Baz", "Qux" }, true)]
        [InlineData(new[] { "Corge", "Baz" }, new[] { "Foo", "Bar", "Baz", "Qux" }, false)]
        [InlineData(new[] { "Foo", "Baz", "Quux" }, new[] { "Foo", "Qux", "Quux", "Corge", "Grault" }, false)]
        [InlineData(new string[] { }, new string[] { }, true)]
        public void CheckIsSubsetOf(string[] collection, string[] superset, bool expected)
        {
            collection.IsSubsetOf(superset).Should().Be(expected);
        }

        [Theory(DisplayName = "IsSubsetOf must throw an ArgumentNullException when the any of the specified collections are null.")]
        [InlineData(null, new string[] { })]
        [InlineData(new string[] { }, null)]
        public void ArgumentNull(string[] collection, string[] superset)
        {
            Action act = () => collection.IsSubsetOf(superset);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}