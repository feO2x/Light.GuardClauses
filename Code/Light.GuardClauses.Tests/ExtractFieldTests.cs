using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class ExtractFieldTests
    {
        [Fact(DisplayName = "ExtractField must return the field info of a valid expression \"o => o.Field\".")]
        public void ExtractPropertyInfo()
        {
            Expression<Func<Foo, int>> expression = foo => foo.Baz;

            var fieldInfo = expression.ExtractField();

            fieldInfo.Should().BeSameAs(typeof(Foo).GetField(nameof(Foo.Baz)));
        }

        [Fact(DisplayName = "ExtractField must throw an ArgumentException when the specified expression is not valid.")]
        public void InvalidExpression()
        {
            Expression<Func<Foo, string>> expression = foo => foo.Bar;

            Action act = () => expression.ExtractField();

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain("The specified expression is not valid. Please use an expression like the following one: o => o.Field");
        }

        [Fact(DisplayName = "ExtractField must throw an ArgumentNullException when the specified expression is null.")]
        public void ExpressionNull()
        {
            Action act = () => ((Expression<Func<object, object>>) null).ExtractField();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("expression");
        }
    }
}