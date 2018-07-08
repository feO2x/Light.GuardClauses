using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class IsNullOrEmptyTests
    {
        [Fact]
        public static void CollectionNull()
        {
            var collection = (IEnumerable<int>) null;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            var result = collection.IsNullOrEmpty();

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            result.Should().BeTrue();
        }

        [Fact]
        public static void CollectionEmpty()
        {
            var emptyCollection = new int[0];

            var result = emptyCollection.IsNullOrEmpty();

            result.Should().BeTrue();
        }

        [Fact]
        public static void CollectionNotEmpty()
        {
            var collection = new[] { "Foo", "Bar" };

            var result = collection.IsNullOrEmpty();

            result.Should().BeFalse();
        }
    }
}