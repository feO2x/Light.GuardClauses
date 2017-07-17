using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsPartOfInheritanceHierarchyOfTests
    {
        [Fact(DisplayName = "IsPartOfInheritanceHierarchyOf must return true if the specified type is equivalent to other type, or if it implements or derives from it.")]
        public void BasicFunctionality()
        {
            CheckIsPartOfInheritanceHierarchy(typeof(string), typeof(string), true);
            CheckIsPartOfInheritanceHierarchy(typeof(int), typeof(ValueType), true);
            CheckIsPartOfInheritanceHierarchy(typeof(double), typeof(decimal), false);
            CheckIsPartOfInheritanceHierarchy(typeof(Exception), typeof(object), true);
            CheckIsPartOfInheritanceHierarchy(typeof(char), typeof(IConvertible), true);
            CheckIsPartOfInheritanceHierarchy(typeof(bool), typeof(IDisposable), false);

            CheckIsPartOfInheritanceHierarchy(typeof(IList<string>), typeof(ICollection<string>), true);
            CheckIsPartOfInheritanceHierarchy(typeof(IList<string>), typeof(ICollection<>), true);
            CheckIsPartOfInheritanceHierarchy(typeof(Dictionary<string, object>), typeof(Dictionary<int, object>), false);
        }

        [Fact(DisplayName = "IsPartOfInheritanceHierarchyOf must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsPartOfInheritanceHierarcharyOf(typeof(object));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact(DisplayName = "IsPartOfInheritanceHierarchyOf must throw an ArgumentNullException when otherType is null.")]
        public void OtherTypeNull()
        {
            var otherType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(object).IsPartOfInheritanceHierarcharyOf(otherType);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(otherType));
        }

        private static void CheckIsPartOfInheritanceHierarchy(Type type, Type otherType, bool expected)
        {
            type.IsPartOfInheritanceHierarcharyOf(otherType).Should().Be(expected);
        }
    }
}