using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions
{
    public static class IsOrInheritsFromTests
    {
        [Fact]
        public static void BasicFunctionality()
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

        [Fact]
        public static void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsOrInheritsFrom(typeof(object));

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact]
        public static void OtherTypeNull()
        {
            var otherType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(object).IsOrInheritsFrom(otherType);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(otherType));
        }

        private static void CheckIsInInheritanceHierarchyOf(Type type, Type otherType, bool expected)
        {
            type.IsOrInheritsFrom(otherType).Should().Be(expected);
        }
    }
}