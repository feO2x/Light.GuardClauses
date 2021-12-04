using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class IsOrImplementsTests
{
    [Fact]
    public static void BasicFunctionality()
    {
        CheckIsOrImplements(typeof(string), typeof(string), true);
        CheckIsOrImplements(typeof(string), typeof(object), false);
        CheckIsOrImplements(typeof(string), typeof(IComparable<string>), true);
        CheckIsOrImplements(typeof(ICollection<string>), typeof(ICollection<string>), true);
        CheckIsOrImplements(typeof(ICollection<string>), typeof(ICollection<>), true);
        CheckIsOrImplements(typeof(ICollection<string>), typeof(IEnumerable<>), true);
        CheckIsOrImplements(typeof(ICollection<string>), typeof(IList<string>), false);
        CheckIsOrImplements(typeof(ICollection<string>), typeof(IList<>), false);
    }

    [Fact]
    public static void TypeNull()
    {
        var type = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => type.IsOrImplements(typeof(object));

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(type));
    }

    [Fact]
    public static void OtherTypeNull()
    {
        var otherType = default(Type);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => typeof(object).IsOrImplements(otherType);

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(otherType));
    }

    private static void CheckIsOrImplements(Type type, Type otherType, bool expected) =>
        type.IsOrImplements(otherType).Should().Be(expected);
}