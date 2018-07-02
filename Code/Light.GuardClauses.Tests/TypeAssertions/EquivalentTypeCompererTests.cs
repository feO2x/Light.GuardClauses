using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions
{
    public static class EquivalentTypeCompererTests
    {
        [Fact]
        public static void CheckEquality()
        {
            CheckEquals(typeof(string), typeof(string), true);
            CheckEquals(typeof(IComparer), typeof(IEnumerator), false);
            CheckEquals(typeof(List<string>), typeof(List<string>), true);
            CheckEquals(typeof(List<string>), typeof(List<>), true);
            CheckEquals(typeof(List<>), typeof(List<string>), true);
            CheckEquals(typeof(IDictionary<,>), typeof(IDictionary<,>), true);
            CheckEquals(typeof(IDictionary<,>), typeof(Dictionary<,>), false);
            CheckEquals(null, typeof(Dictionary<,>), false);
            CheckEquals(typeof(double), null, false);
            CheckEquals(null, null, true);
        }

        private static void CheckEquals(Type x, Type y, bool expected) =>
            new EqualivalentTypeComparer().Equals(x, y).Should().Be(expected);

        [Fact]
        public static void HashCode()
        {
            CheckGetHashCode(typeof(int), typeof(int), true);
            CheckGetHashCode(typeof(double), typeof(object), false);
            CheckGetHashCode(typeof(IList<string>), typeof(IList<>), true);
            CheckGetHashCode(typeof(IDictionary<int, string>), typeof(IDictionary<,>), true);
            CheckGetHashCode(typeof(Dictionary<string, object>), typeof(IDictionary<string, object>), false);
            CheckGetHashCode(typeof(Dictionary<string, object>), typeof(IDictionary<,>), false);
            CheckGetHashCode(null, typeof(int), false);
            CheckGetHashCode(typeof(short), null, false);
            CheckGetHashCode(null, null, false);
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