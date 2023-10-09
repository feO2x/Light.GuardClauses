using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class DerivesFromTests
{
    [Fact]
    public static void ReferenceTypeValidBaseClass() =>
        TestIsDerivingFrom(typeof(string), typeof(object), true);

    [Fact]
    public static void ValueTypeValidBaseClass() =>
        TestIsDerivingFrom(typeof(int), typeof(object), true);

    [Fact]
    public static void SameNonGenericType()
    {
        TestIsDerivingFrom(typeof(double), typeof(double), false);
        TestIsDerivingFrom(typeof(string), typeof(string), false);
    }

    [Fact]
    public static void ValueTypeNonObjectBaseClass() =>
        TestIsDerivingFrom(typeof(double), typeof(ValueType), true);

    [Fact]
    public static void EnumTypeValidBaseClass()
    {
        TestIsDerivingFrom(typeof(ConsoleColor), typeof(Enum), true);
        TestIsDerivingFrom(typeof(Enum), typeof(ConsoleColor), false);
    }

    [Fact]
    public static void ClosedBoundGenericTypeWithValidGenericTypeDefinition()
    {
        TestIsDerivingFrom(typeof(StringDictionary), typeof(Dictionary<,>), true);
        TestIsDerivingFrom(typeof(Dictionary<,>), typeof(StringDictionary), false);
    }

    [Fact]
    public static void ClosedBoundGenericTypeWithObject()
    {
        TestIsDerivingFrom(typeof(Dictionary<int, object>), typeof(object), true);
        TestIsDerivingFrom(typeof(object), typeof(Dictionary<int, object>), false);
    }

    [Fact]
    public static void SameGenericType()
    {
        TestIsDerivingFrom(typeof(Dictionary<string, object>), typeof(Dictionary<string, object>), false);
        TestIsDerivingFrom(typeof(Dictionary<,>), typeof(Dictionary<string, object>), false);
    }

    [Fact]
    public static void GenericTypeDefinitionOfSameType() =>
        TestIsDerivingFrom(typeof(Dictionary<int, object>), typeof(Dictionary<,>), false);

    [Fact]
    public static void WrongBaseType() =>
        TestIsDerivingFrom(typeof(Exception), typeof(ArgumentException), false);

    private static void TestIsDerivingFrom(Type type, Type baseClass, bool expected) =>
        type.DerivesFrom(baseClass).Should().Be(expected);

    [Fact]
    public static void TypeNull()
    {
        var type = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => type.DerivesFrom(typeof(object));

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(type));
    }

    [Fact]
    public static void BaseClassNull()
    {
        var baseClass = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => typeof(string).DerivesFrom(baseClass);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(baseClass));
    }

    public sealed class StringDictionary : Dictionary<string, object> { }
}