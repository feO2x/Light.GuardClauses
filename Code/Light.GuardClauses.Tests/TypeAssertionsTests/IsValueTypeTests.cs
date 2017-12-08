using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class IsValueTypeTests
    {
        [Fact(DisplayName = "IsValueType must return false if the specified type is a class.")]
        public void ClassesAreReferenceTypes()
        {
            CheckIsValueType(typeof(object), false);
            CheckIsValueType(typeof(string), false);
            CheckIsValueType(typeof(DictionaryBase), false);
            CheckIsValueType(typeof(List<string>), false);
            CheckIsValueType(typeof(Dictionary<,>), false);
        }

        [Fact(DisplayName = "IsValueType must return false when the specified type is an interface.")]
        public void InterfacesAreReferenceTypes()
        {
            CheckIsValueType(typeof(IDisposable), false);
            CheckIsValueType(typeof(IQueryable<>), false);
            CheckIsValueType(typeof(IDictionary<string, object>), false);
        }

        [Fact(DisplayName = "IsValueType must return false when the specified type is a delegate.")]
        public void DelegatesAreReferenceTypes()
        {
            CheckIsValueType(typeof(Action), false);
            CheckIsValueType(typeof(Func<string, object>), false);
            CheckIsValueType(typeof(Func<>), false);
        }

        [Fact(DisplayName = "IsValueType must return true when the specified type is a struct.")]
        public void StructsAreNotReferenceTypes()
        {
            CheckIsValueType(typeof(double), true);
            CheckIsValueType(typeof(int?), true);
            CheckIsValueType(typeof(ArraySegment<>), true);
        }

        [Fact(DisplayName = "IsValueType must return true when the specified type is an enum.")]
        public void EnumsAreNotReferenceTypes()
        {
            CheckIsValueType(typeof(ConsoleColor), true);
            CheckIsValueType(typeof(DateTimeKind), true);
        }

        [Fact(DisplayName = "IsValueType must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsValueType();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        private static void CheckIsValueType(Type type, bool expected)
        {
            type.IsValueType().Should().Be(expected);
        }
    }
}