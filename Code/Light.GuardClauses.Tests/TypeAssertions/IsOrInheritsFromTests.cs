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
            CheckIsOrInheritsFrom(typeof(string), typeof(string), true);
            CheckIsOrInheritsFrom(typeof(int), typeof(ValueType), true);
            CheckIsOrInheritsFrom(typeof(double), typeof(decimal), false);
            CheckIsOrInheritsFrom(typeof(Exception), typeof(object), true);
            CheckIsOrInheritsFrom(typeof(char), typeof(IConvertible), true);
            CheckIsOrInheritsFrom(typeof(bool), typeof(IDisposable), false);

            CheckIsOrInheritsFrom(typeof(IList<string>), typeof(ICollection<string>), true);
            CheckIsOrInheritsFrom(typeof(IList<string>), typeof(ICollection<>), true);
            CheckIsOrInheritsFrom(typeof(Dictionary<string, object>), typeof(Dictionary<int, object>), false);
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

        private static void CheckIsOrInheritsFrom(Type type, Type otherType, bool expected) => type.IsOrInheritsFrom(otherType).Should().Be(expected);
    }
}