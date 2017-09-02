using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsStructurallyEqualToTests
    {
        [Theory(DisplayName = "IsStructurallyEqualTo must return true when both collections have the same items in the same order.")]
        [InlineData(new[] { "Foo", "Bar" }, new[] { "Foo", "Bar" })]
        [InlineData(new[] { "Qux", "Quux" }, new[] { "Qux", "Quux" })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 })]
        [InlineData(new object[] { }, new object[] { })]
        public void StructurallyEqual<T>(T[] first, T[] second)
        {
            first.IsStructurallyEqualTo(second).Should().BeTrue();
        }

        [Theory(DisplayName = "IsStructurallyEqualTo must return false when the collections do not have the same items in the same order.")]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, new[] { "Foo", "Baz", "Bar" })]
        [InlineData(new[] { "Foo", "Bar", "Baz" }, new[] { "Foo", "Bar" })]
        [InlineData(new[] { 1, 5, 7, 9 }, new[] { 7, 1, 9, 5 })]
        [InlineData(new[] { 1, 5, 7, 9 }, new int[] { })]
        [InlineData(new int[] { }, new[] { 1, 5, 7, 9 })]
        public void NotStructurallyEqual<T>(T[] first, T[] second)
        {
            first.IsStructurallyEqualTo(second).Should().BeFalse();
        }

        [Theory(DisplayName = "IsStructurallyEqualTo must throw an ArgumentNullException when one of the enumerables is null.")]
        [InlineData(null, new object[] { })]
        [InlineData(new object[] { }, null)]
        public void ArgumentNull(IEnumerable<object> first, IEnumerable<object> second)
        {
            Action act = () => first.IsStructurallyEqualTo(second);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "IsStructurallyEqualTo must return true when both collections are null.")]
        public void BothNull()
        {
            ((IEnumerable<string>) null).IsStructurallyEqualTo(null).Should().BeTrue();
        }

        [Fact(DisplayName = "IsStructurallyEqualTo must support custom item comparers.")]
        public void CustomEqualityComperarer()
        {
            var comparerSpy = new EqualityComparerSpy<string>();

            var first = new[] { "Foo", "Bar" };
            var second = new[] { "Foo", "Bar" };

            var result = first.IsStructurallyEqualTo(second, comparerSpy);

            result.Should().BeTrue();

            comparerSpy.EqualsCallCount.Should().BeGreaterThan(0);
            comparerSpy.GetHashCodeCallCount.Should().BeGreaterThan(0);
        }
    }
}