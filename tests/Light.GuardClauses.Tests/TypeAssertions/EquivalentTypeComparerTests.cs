using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class EquivalentTypeComparerTests
{
    [Theory]
    [MemberData(nameof(EqualityData))]
    public static void CheckEquality(Type x, Type y, bool expected) =>
        EquivalentTypeComparer.Instance.Equals(x, y).Should().Be(expected);

    public static readonly TheoryData<Type, Type, bool> EqualityData =
        new ()
        {
            { typeof(string), typeof(string), true },
            { typeof(IComparer), typeof(IEnumerator), false },
            { typeof(List<string>), typeof(List<string>), true },
            { typeof(List<string>), typeof(List<>), true },
            { typeof(List<>), typeof(List<string>), true },
            { typeof(IDictionary<,>), typeof(IDictionary<,>), true },
            { typeof(IDictionary<,>), typeof(Dictionary<,>), false },
            { null!, typeof(Dictionary<,>), false },
            { typeof(double), null!, false },
            { null!, null!, true },
        };

    [Theory]
    [MemberData(nameof(HashCodesShouldBeEqualData))]
    public static void HashCodesOfEquivalentTypesAreEqual(Type x, Type y) =>
        EquivalentTypeComparer.Instance.GetHashCode(x).Should().Be(EquivalentTypeComparer.Instance.GetHashCode(y));

    public static readonly TheoryData<Type, Type> HashCodesShouldBeEqualData =
        new ()
        {
            { typeof(int), typeof(int) },
            { typeof(IList<string>), typeof(IList<>) },
            { typeof(IDictionary<int, string>), typeof(IDictionary<,>) },
        };

    [Fact]
    public static void HashCodeOfNullIsZero() =>
        EquivalentTypeComparer.Instance.GetHashCode(null!).Should().Be(0);
}
