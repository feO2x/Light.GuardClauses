using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions
{
    public static class ImplementsTests
    {
        [Fact]
        public static void InterfaceTypes()
        {
            CheckImplements(typeof(ArrayList), typeof(IList), true);
            CheckImplements(typeof(List<string>), typeof(IEnumerable<string>), true);
            CheckImplements(typeof(List<object>), typeof(IList<>), true);
            CheckImplements(typeof(ObservableCollection<>), typeof(ICollection<>), true);
            CheckImplements(typeof(ICollection), typeof(IEnumerable), true);
            CheckImplements(typeof(IList<object>), typeof(ICollection<>), true);
            CheckImplements(typeof(IList<>), typeof(ICollection<>), true);
            CheckImplements(typeof(Exception), typeof(IObservable<>), false);
            CheckImplements(typeof(IList<string>), typeof(IList<string>), false);
            CheckImplements(typeof(IList<string>), typeof(IList<>), false);
            CheckImplements(typeof(IList<>), typeof(IList<>), false);
        }

        [Fact]
        public static void OtherTypes()
        {
            CheckImplements(typeof(Array), typeof(MulticastDelegate), false);
            CheckImplements(typeof(string), typeof(object), false);
            CheckImplements(typeof(IsEquivalentTypeToTests.GenericType<>), typeof(int), false);
        }

        [Fact]
        public static void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.Implements(typeof(IComparable));

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact]
        public static void InterfaceTypeNull()
        {
            var interfaceType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(List<object>).Implements(interfaceType);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(interfaceType));
        }

        private static void CheckImplements(Type type, Type interfaceType, bool expected) => 
            type.Implements(interfaceType).Should().Be(expected);
    }
}
