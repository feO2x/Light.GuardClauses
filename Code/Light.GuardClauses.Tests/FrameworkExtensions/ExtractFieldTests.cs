using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

public static class ExtractFieldTests
{
    [Fact]
    public static void ExtractPropertyInfo()
    {
        Expression<Func<Foo, int>> expression = foo => foo.Baz;

        var fieldInfo = expression.ExtractField();

        fieldInfo.Should().BeSameAs(typeof(Foo).GetTypeInfo().GetField(nameof(Foo.Baz)));
    }

    [Fact]
    public static void InvalidExpression()
    {
        Expression<Func<Foo, string>> expression = foo => foo.Bar;

        Action act = () => expression.ExtractField();

        act.Should().Throw<ArgumentException>()
           .And.Message.Should().Contain("The specified expression is not valid. Please use an expression like the following one: o => o.Field");
    }

    [Fact]
    public static void ExpressionNull()
    {
        Action act = () => ((Expression<Func<object, object>>) null).ExtractField();

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("expression");
    }
}