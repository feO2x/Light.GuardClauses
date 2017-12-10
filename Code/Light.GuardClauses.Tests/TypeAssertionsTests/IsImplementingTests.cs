using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class IsImplementingTests
    {
        [Fact(DisplayName = "IsImplementing must return true if the specified interface is implemented by the target type.")]
        public void InterfaceTypes()
        {
            CheckIsImplementing(typeof(ArrayList), typeof(IList), true);
            CheckIsImplementing(typeof(List<string>), typeof(IEnumerable<string>), true);
            CheckIsImplementing(typeof(List<object>), typeof(IList<>), true);
            CheckIsImplementing(typeof(ObservableCollection<>), typeof(ICollection<>), true);
            CheckIsImplementing(typeof(ICollection), typeof(IEnumerable), true);
            CheckIsImplementing(typeof(IList<object>), typeof(ICollection<>), true);
            CheckIsImplementing(typeof(IList<>), typeof(ICollection<>), true);
            CheckIsImplementing(typeof(Exception), typeof(IObservable<>), false);
            CheckIsImplementing(typeof(IList<string>), typeof(IList<string>), false);
            CheckIsImplementing(typeof(IList<string>), typeof(IList<>), false);
            CheckIsImplementing(typeof(IList<>), typeof(IList<>), false);
        }

        [Fact(DisplayName = "IsImplementing must always return false when the specified interface type is not an interface.")]
        public void OtherTypes()
        {
            CheckIsImplementing(typeof(Array), typeof(MulticastDelegate), false);
            CheckIsImplementing(typeof(string), typeof(object), false);
            CheckIsImplementing(typeof(IsEquivalentTypeToTests.GenericType<>), typeof(int), false);
        }

        [Fact(DisplayName = "IsImplementing must throw an ArgumentNullException when type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsImplementing(typeof(IComparable));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact(DisplayName = "IsImplementing must throw an ArgumentNullException when interfaceType is null.")]
        public void InterfaceTypeNull()
        {
            var interfaceType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(List<object>).IsImplementing(interfaceType);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(interfaceType));
        }

        private static void CheckIsImplementing(Type type, Type interfaceType, bool expected)
        {
            type.IsImplementing(interfaceType).Should().Be(expected);
        }
    }
}