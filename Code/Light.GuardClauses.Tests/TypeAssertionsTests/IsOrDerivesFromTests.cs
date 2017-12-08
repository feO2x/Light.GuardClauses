using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class IsOrDerivesFromTests
    {
        [Fact(DisplayName = "IsOrDerivesFrom must return true when the specified type is equal to the other type or if it derives from it.")]
        public void BasicFunctionality()
        {
            CheckIsOrDerivesFrom(typeof(int), typeof(int), true);
            CheckIsOrDerivesFrom(typeof(int), typeof(ValueType), true);
            CheckIsOrDerivesFrom(typeof(int), typeof(double), false);

            CheckIsOrDerivesFrom(typeof(List<>), typeof(IList<>), false);
            CheckIsOrDerivesFrom(typeof(ObservableCollection<string>), typeof(Collection<string>), true);
            CheckIsOrDerivesFrom(typeof(ObservableCollection<string>), typeof(Collection<>), true);
            CheckIsOrDerivesFrom(typeof(List<>), typeof(object), true);
            CheckIsOrDerivesFrom(typeof(List<>), typeof(HashSet<>), false);
        }

        [Fact(DisplayName = "IsOrDerivesFrom must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsOrDerivesFrom(typeof(object));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact(DisplayName = "IsOrDerivesFrom must throw an ArgumentNullException when otherType is null.")]
        public void OtherTypeNull()
        {
            var otherType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(object).IsOrDerivesFrom(otherType);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(otherType));
        }

        private static void CheckIsOrDerivesFrom(Type type, Type otherType, bool expected)
        {
            type.IsOrDerivesFrom(otherType).Should().Be(expected);
        }
    }
}