using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class IsContainingDuplicatesTests
    {
        [Fact(DisplayName = "IsContainingDuplicates must return false when all items are unique.")]
        public void NoDuplicates()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };

            var result = collection.IsContainingDuplicates();

            result.Should().BeFalse();
        }

        [Fact(DisplayName = "IsContainingDuplicates must return true when at least two items are equal.")]
        public void Duplicates()
        {
            var collection = new[] { "Foo", "Bar", "Foo", "Baz", "Bar" };

            var result = collection.IsContainingDuplicates();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsContainingDuplicates must throw an ArgumentNullException when the specified collection is null.")]
        public void ArgumentNull()
        {
            var enumerable = default(IEnumerable<object>);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => enumerable.IsContainingDuplicates();

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(enumerable));
        }
    }
}