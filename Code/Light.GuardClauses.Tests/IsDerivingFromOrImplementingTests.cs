using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsDerivingFromOrImplementingTests
    {
        [Fact(DisplayName = "IsDerivingFromOrImplementing must return true when the specified type derives from the given base class.")]
        public void BaseClasses()
        {
            CheckIsDerivingFromOrImplementing(typeof(ArgumentException), typeof(Exception), true);
            CheckIsDerivingFromOrImplementing(typeof(ArgumentNullException), typeof(Exception), true);
            CheckIsDerivingFromOrImplementing(typeof(List<>), typeof(Encoding), false);
            CheckIsDerivingFromOrImplementing(typeof(ObservableCollection<object>), typeof(Collection<>), true);
            CheckIsDerivingFromOrImplementing(typeof(double), typeof(ValueType), true);
        }

        [Fact(DisplayName = "IsDerivingFromOrImplementing must return true when the specified type implements the given interface.")]
        public void Interfaces()
        {
            CheckIsDerivingFromOrImplementing(typeof(string), typeof(IComparable), true);
            CheckIsDerivingFromOrImplementing(typeof(string), typeof(IEnumerable<char>), true);
            CheckIsDerivingFromOrImplementing(typeof(string), typeof(IEnumerable<>), true);
            CheckIsDerivingFromOrImplementing(typeof(string), typeof(IEqualityComparer<>), false);

            CheckIsDerivingFromOrImplementing(typeof(int), typeof(IEquatable<int>), true);
            CheckIsDerivingFromOrImplementing(typeof(int), typeof(IServiceProvider), false);

            CheckIsDerivingFromOrImplementing(typeof(IList<object>), typeof(ICollection<object>), true);
            CheckIsDerivingFromOrImplementing(typeof(IList<object>), typeof(ICollection<>), true);
            CheckIsDerivingFromOrImplementing(typeof(IList<>), typeof(IDictionary<,>), false);
        }

        [Fact(DisplayName = "IsDerivingFromOrImplementing must always return false when the base type is not a class or interface.")]
        public void OtherTypes()
        {
            CheckIsDerivingFromOrImplementing(typeof(int), typeof(ConsoleColor), false);
            CheckIsDerivingFromOrImplementing(typeof(string), typeof(double), false);
            CheckIsDerivingFromOrImplementing(typeof(UnicodeEncoding), typeof(Func<>), false);
        }

        [Fact(DisplayName = "IsDerivingFromOrImplementing must throw an ArgumentNullException when the specified type is null.")]
        public void TypeNull()
        {
            var type = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => type.IsDerivingFromOrImplementing(typeof(object));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(type));
        }

        [Fact]
        public void BaseTypeNull()
        {
            var baseClassOrInterfaceType = default(Type);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => typeof(string).IsDerivingFromOrImplementing(baseClassOrInterfaceType);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(baseClassOrInterfaceType));
        }

        private static void CheckIsDerivingFromOrImplementing(Type type, Type baseClassOrInterfaceType, bool expected)
        {
            type.IsDerivingFromOrImplementing(baseClassOrInterfaceType).Should().Be(expected);
        }
    }
}