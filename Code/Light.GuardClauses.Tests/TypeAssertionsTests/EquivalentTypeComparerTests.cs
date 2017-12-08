using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class EquivalentTypeComparerTests
    {
        [Fact(DisplayName = "Equals must return true when two types are equivalent.")]
        public void CheckEquality()
        {
            CheckEquals(typeof(string), typeof(string), true);
            CheckEquals(typeof(IComparer), typeof(IEnumerator), false);
            CheckEquals(typeof(List<string>), typeof(List<string>), true);
            CheckEquals(typeof(List<string>), typeof(List<>), true);
            CheckEquals(typeof(List<>), typeof(List<string>), true);
            CheckEquals(typeof(IDictionary<,>), typeof(IDictionary<,>), true);
            CheckEquals(typeof(IDictionary<,>), typeof(Dictionary<,>), false);
        }

        private static void CheckEquals(Type x, Type y, bool expected)
        {
            new EqualivalentTypeComparer().Equals(x, y).Should().Be(expected);
        }

        [Fact(DisplayName = "GetHashCode must produce the same hash code when the two types are equivalent.")]
        public void HashCode()
        {
            CheckGetHashCode(typeof(int), typeof(int), true);
            CheckGetHashCode(typeof(double), typeof(object), false);
            CheckGetHashCode(typeof(IList<string>), typeof(IList<>), true);
            CheckGetHashCode(typeof(IDictionary<int, string>), typeof(IDictionary<,>), true);
            CheckGetHashCode(typeof(Dictionary<string, object>), typeof(IDictionary<string, object>), false);
            CheckGetHashCode(typeof(Dictionary<string, object>), typeof(IDictionary<,>), false);
        }

        private static void CheckGetHashCode(Type x, Type y, bool shouldBeEqualHashCodes)
        {
            var comparer = new EqualivalentTypeComparer();
            var firstHashCode = comparer.GetHashCode(x);
            var secondHashCode = comparer.GetHashCode(y);
            if (shouldBeEqualHashCodes)
                firstHashCode.Should().Be(secondHashCode);
        }
    }
}