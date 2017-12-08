using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class IsStructurallyEquivalentToTests
    {
        [Theory(DisplayName = "IsStructurallyEquivalentTo must return true when both collections have the same items in any order.")]
        [InlineData(new[] { "Foo", "Bar" }, new[] { "Foo", "Bar" })]
        [InlineData(new[] { "Foo", "Bar" }, new[] { "Bar", "Foo" })]
        [InlineData(new[] { "Qux", "Quux", "Corge" }, new[] { "Corge", "Qux", "Quux" })]
        [InlineData(new string[] { }, new string[] { })]
        public void StructurallyEqual(string[] first, string[] second)
        {
            first.IsStructurallyEquivalentTo(second).Should().BeTrue();
        }

        [Theory(DisplayName = "IsStructurallyEquivalentTo must return false when the collections do not have the same items in any order.")]
        [InlineData(new[] { 1, 5, 7, 9 }, new[] { 7, 9, 5 })]
        [InlineData(new[] { 1, 5, 7, 9 }, new int[] { })]
        [InlineData(new int[] { }, new[] { 1, 5, 7, 9 })]
        public void NotStructurallyEqual(int[] first, int[] second)
        {
            first.IsStructurallyEquivalentTo(second).Should().BeFalse();
        }

        [Theory(DisplayName = "IsStructurallyEquivalentTo must throw an ArgumentNullException when one of the enumerables is null.")]
        [InlineData(null, new object[] { })]
        [InlineData(new object[] { }, null)]
        public void ArgumentNull(IEnumerable<object> first, IEnumerable<object> second)
        {
            Action act = () => first.IsStructurallyEquivalentTo(second);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "IsStructurallyEquivalentTo must return true when both collections are null.")]
        public void BothNull()
        {
            ((IEnumerable<string>) null).IsStructurallyEquivalentTo(null).Should().BeTrue();
        }

        [Fact(DisplayName = "IsStructurallyEquivalentTo must support custom item comparers.")]
        public void CustomEqualityComperarer()
        {
            var comparerSpy = new EqualityComparerSpy<string>();

            var first = new[] { "Foo", "Bar" };
            var second = new[] { "Foo", "Bar" };

            var result = first.IsStructurallyEquivalentTo(second, comparerSpy);

            result.Should().BeTrue();

            comparerSpy.EqualsCallCount.Should().BeGreaterThan(0);
            comparerSpy.GetHashCodeCallCount.Should().BeGreaterThan(0);
        }
    }
}