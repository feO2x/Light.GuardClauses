using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class IsContainingInstancesOfDifferentTypesTests
    {
        [Fact(DisplayName = "IsContainingInstancesOfDifferentTypes must return true when all instances of the collection have a unique type.")]
        public void InstancesWithUniqueType()
        {
            var collection = new IInterface[] { new A(), new B(), new C() };

            var result = collection.IsContainingInstancesOfDifferentTypes();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsContainingInstancesOfDifferentTypes must return false when there are any items which have the same type.")]
        public void InstancesWithDuplicateTypes()
        {
            var collection = new IInterface[] { new A(), new B(), new A() };

            var result = collection.IsContainingInstancesOfDifferentTypes();

            result.Should().BeFalse();
        }

        [Fact(DisplayName = "IsContainingInstancesOfDifferentTypes must throw an ArgumentNullException when the specified collection is null.")]
        public void EnumerableNull()
        {
            var enumerable = default(IEnumerable<string>);

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => enumerable.IsContainingInstancesOfDifferentTypes();

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(enumerable));
        }

        [Fact(DisplayName = "IsContainingInstancesOfDifferentTypes must throw a CollectionException when any of the items is null.")]
        public void ItemNull()
        {
            var collection = new IInterface[] { new A(), null, new C() };

            Action act = () => collection.IsContainingInstancesOfDifferentTypes();

            act.Should().Throw<CollectionException>()
               .And.Message.Should().Contain($"The collection contains null at index 1");
        }

        private interface IInterface { }

        private class A : IInterface { }

        private class B : IInterface { }

        private class C : A { }
    }
}