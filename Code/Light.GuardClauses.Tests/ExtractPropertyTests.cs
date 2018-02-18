using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class ExtractPropertyTests
    {
        [Fact(DisplayName = "ExtractProperty must return the property info of a valid expression \"o => o.Property\".")]
        public void ExtractPropertyInfo()
        {
            Expression<Func<Foo, string>> expression = foo => foo.Bar;

            var propertyInfo = expression.ExtractProperty();

            propertyInfo.Should().BeSameAs(typeof(Foo).GetProperty(nameof(Foo.Bar)));
        }

        [Fact(DisplayName = "ExtractProperty must throw an ArgumentException when the specified expression is not valid.")]
        public void InvalidExpression()
        {
            Expression<Func<Foo, int>> expression = foo => foo.Baz;

            Action act = () => expression.ExtractProperty();

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Contain("The specified expression is not valid. Please use an expression like the following one: o => o.Property");
        }


        [Fact(DisplayName = "ExtractProperty must throw an ArgumentNullException when the specified expression is null.")]
        public void ExpressionNull()
        {
            Action act = () => ((Expression<Func<object, object>>) null).ExtractProperty();

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("expression");
        }
    }

    public class Foo
    {
        public int Baz;
        public string Bar { get; set; }
    }
}