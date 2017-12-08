using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsOrImplementsTests
    {
        [Fact(DisplayName = "IsOrImplements must return true when the specified type is equal to or implements the other type.")]
        public void BasicFunctionality()
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

        [Fact(DisplayName = "IsOrImplements must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsOrImplements(typeof(object));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact(DisplayName = "IsOrImplements must throw an ArgumentNullException when otherType is null.")]
        public void OtherTypeNull()
        {
            var otherType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(object).IsOrImplements(otherType);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(otherType));
        }

        private static void CheckIsOrImplements(Type type, Type otherType, bool expected)
        {
            type.IsOrImplements(otherType).Should().Be(expected);
        }
    }
}