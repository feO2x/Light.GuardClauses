using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsReferenceTypeTests
    {
        [Fact(DisplayName = "IsReferenceType must return true if the specified type is a class.")]
        public void ClassesAreReferenceTypes()
        {
            CheckIsReferenceType(typeof(object), true);
            CheckIsReferenceType(typeof(string), true);
            CheckIsReferenceType(typeof(DictionaryBase), true);
            CheckIsReferenceType(typeof(List<string>), true);
            CheckIsReferenceType(typeof(Dictionary<,>), true);
        }

        [Fact(DisplayName = "IsReferenceType must return true when the specified type is an interface.")]
        public void InterfacesAreReferenceTypes()
        {
            CheckIsReferenceType(typeof(IDisposable), true);
            CheckIsReferenceType(typeof(IQueryable<>), true);
            CheckIsReferenceType(typeof(IDictionary<string, object>), true);
        }

        [Fact(DisplayName = "IsReferenceType must return true when the specified type is a delegate.")]
        public void DelegatesAreReferenceTypes()
        {
            CheckIsReferenceType(typeof(Action), true);
            CheckIsReferenceType(typeof(Func<string, object>), true);
            CheckIsReferenceType(typeof(Func<>), true);
        }

        [Fact(DisplayName = "IsReferenceType must return false when the specified type is a struct.")]
        public void StructsAreNotReferenceTypes()
        {
            CheckIsReferenceType(typeof(double), false);
            CheckIsReferenceType(typeof(int?), false);
            CheckIsReferenceType(typeof(ArraySegment<>), false);
        }

        [Fact(DisplayName = "IsReferenceType must return false when the specified type is an enum.")]
        public void EnumsAreNotReferenceTypes()
        {
            CheckIsReferenceType(typeof(ConsoleColor), false);
            CheckIsReferenceType(typeof(DateTimeKind), false);
        }

        [Fact(DisplayName = "IsReferenceType must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsReferenceType();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void CheckIsReferenceType(Type type, bool expected)
        {
            type.IsReferenceType().Should().Be(expected);
        }
    }
}