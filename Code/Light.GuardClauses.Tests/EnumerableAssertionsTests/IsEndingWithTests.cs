using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class IsEndingWithTests
    {
        [Fact(DisplayName = "IsEndingWith must return true when the specified enumerable starts with items that are equal to the items of the specified set (same order).")]
        public void EndsWith()
        {
            var result = new[] { "Foo", "Bar", "Baz" }.IsEndingWith(new[] { "Baz" });

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsEndingWith must return false when the specified enumerable contains any items at the end that are not items of the specified set (same order).")]
        public void EndsNotWith()
        {
            var result = new[] { 1, 2, 5, 6, 9, 5 }.IsEndingWith(new[] { 5, 9, 5 });

            result.Should().BeFalse();
        }

        [Theory(DisplayName = "IsEndingWith must throw an ArgumentNullException when enumerable or set are null")]
        [InlineData(null, new string[] { })]
        [InlineData(new string[] { }, null)]
        public void ArgumentNull(string[] first, string[] second)
        {
            Action act = () => first.IsEndingWith(second);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}