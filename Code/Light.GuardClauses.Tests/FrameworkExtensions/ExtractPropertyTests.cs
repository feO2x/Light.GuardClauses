using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions;

public static class ExtractPropertyTests
{
    [Fact]
    public static void ExtractPropertyInfo()
    {
        Expression<Func<Foo, string>> expression = foo => foo.Bar;

        var propertyInfo = expression.ExtractProperty();

        propertyInfo.Should().BeSameAs(typeof(Foo).GetTypeInfo().GetProperty(nameof(Foo.Bar)));
    }

    [Fact]
    public static void InvalidExpression()
    {
        Expression<Func<Foo, int>> expression = foo => foo.Baz;

        Action act = () => expression.ExtractProperty();

        act.Should().Throw<ArgumentException>()
           .And.Message.Should().Contain("The specified expression is not valid. Please use an expression like the following one: o => o.Property");
    }


    [Fact]
    public static void ExpressionNull()
    {
        Action act = () => ((Expression<Func<object, object>>) null).ExtractProperty();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("expression");
    }
}

public class Foo
{
    public int Baz;
    public string Bar { get; set; }
}