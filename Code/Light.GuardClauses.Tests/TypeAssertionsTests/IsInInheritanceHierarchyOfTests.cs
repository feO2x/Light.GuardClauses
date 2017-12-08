using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class IsInInheritanceHierarchyOfTests
    {
        [Fact(DisplayName = "IsPartOfInheritanceHierarchyOf must return true if the specified type is equivalent to other type, or if it implements or derives from it.")]
        public void BasicFunctionality()
        {
            CheckIsInInheritanceHierarchyOf(typeof(string), typeof(string), true);
            CheckIsInInheritanceHierarchyOf(typeof(int), typeof(ValueType), true);
            CheckIsInInheritanceHierarchyOf(typeof(double), typeof(decimal), false);
            CheckIsInInheritanceHierarchyOf(typeof(Exception), typeof(object), true);
            CheckIsInInheritanceHierarchyOf(typeof(char), typeof(IConvertible), true);
            CheckIsInInheritanceHierarchyOf(typeof(bool), typeof(IDisposable), false);

            CheckIsInInheritanceHierarchyOf(typeof(IList<string>), typeof(ICollection<string>), true);
            CheckIsInInheritanceHierarchyOf(typeof(IList<string>), typeof(ICollection<>), true);
            CheckIsInInheritanceHierarchyOf(typeof(Dictionary<string, object>), typeof(Dictionary<int, object>), false);
        }

        [Fact(DisplayName = "IsPartOfInheritanceHierarchyOf must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsInInheritanceHierarchyOf(typeof(object));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact(DisplayName = "IsPartOfInheritanceHierarchyOf must throw an ArgumentNullException when otherType is null.")]
        public void OtherTypeNull()
        {
            var otherType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(object).IsInInheritanceHierarchyOf(otherType);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(otherType));
        }

        private static void CheckIsInInheritanceHierarchyOf(Type type, Type otherType, bool expected)
        {
            type.IsInInheritanceHierarchyOf(otherType).Should().Be(expected);
        }
    }
}