using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class IsOrDerivesFromTests
{
    [Fact]
    public static void BasicFunctionality()
    {
        CheckIsOrDerivesFrom(typeof(int), typeof(int), true);
        CheckIsOrDerivesFrom(typeof(int), typeof(ValueType), true);
        CheckIsOrDerivesFrom(typeof(int), typeof(double), false);

        CheckIsOrDerivesFrom(typeof(List<>), typeof(IList<>), false);
        CheckIsOrDerivesFrom(typeof(ObservableCollection<string>), typeof(Collection<string>), true);
        CheckIsOrDerivesFrom(typeof(ObservableCollection<string>), typeof(Collection<>), true);
        CheckIsOrDerivesFrom(typeof(List<>), typeof(object), true);
        CheckIsOrDerivesFrom(typeof(List<>), typeof(HashSet<>), false);
    }

    [Fact]
    public static void TypeNull()
    {
        var type = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => type.IsOrDerivesFrom(typeof(object));

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(type));
    }

    [Fact]
    public static void OtherTypeNull()
    {
        var otherType = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => typeof(object).IsOrDerivesFrom(otherType);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(otherType));
    }

    private static void CheckIsOrDerivesFrom(Type type, Type otherType, bool expected) =>
        type.IsOrDerivesFrom(otherType).Should().Be(expected);
}