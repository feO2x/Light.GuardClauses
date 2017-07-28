using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsStartingWithTests
    {
        [Fact(DisplayName = "IsStartingWith must return true when the specified enumerable has items that are equal to the items of the specified set (same order).")]
        public void StartsWith()
        {
            var result = new[] { "Foo", "Bar", "Baz" }.IsStartingWith(new[] { "Foo", "Bar" });

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsStartingWith must return false when the specified enumerable contains any items at the start that are not items of the specified set (same order).")]
        public void StartsNotWith()
        {
            var result = new[] { 1, 2, 5, 6, 9, 5 }.IsStartingWith(new[] { 1, 5, 9 });

            result.Should().BeFalse();
        }

        [Theory(DisplayName = "IsStartingWith must throw an ArgumentNullException when enumerable or set are null")]
        [InlineData(null, new string[] { })]
        [InlineData(new string[] { }, null)]
        public void ArgumentNull(string[] first, string[] second)
        {
            Action act = () => first.IsStartingWith(second);

            act.ShouldThrow<ArgumentNullException>();
        }
    }
}