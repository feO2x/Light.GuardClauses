using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainInstancesOfDifferentTypesTests : ICustomMessageAndExceptionTestDataProvider
    {
        private static readonly List<IInterface> ValidTestData = new List<IInterface>
                                                                 {
                                                                     new A(), new B(), new C()
                                                                 };

        private static readonly List<IInterface> InvalidTestData = new List<IInterface>
                                                                   {
                                                                       new A(), new B(), new A(), new C()
                                                                   };

        [Fact(DisplayName = "MustContainInstancesOfDifferentTypes must throw a CollectionException when the specified collection contains instances of the same type.")]
        public void ListWithInstancesOfSameType()
        {
            Action act = () => InvalidTestData.MustContainInstancesOfDifferentTypes(nameof(InvalidTestData));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(InvalidTestData)} must contain instances of different subtypes, but \"{InvalidTestData[0]}\" and \"{InvalidTestData[2]}\" (at positions {0} and {2}) have the same type: \"{typeof(A)}\".");
        }

        [Fact(DisplayName = "MustContainInstancesOfDifferentTypes must not throw an exception when the specified collection contains instances of different subtypes.")]
        public void ListWithDifferentInstances()
        {
            Action act = () => ValidTestData.MustContainInstancesOfDifferentTypes();

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustContainInstancesOfDifferentTypes must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            Action act = () => ((IEnumerable<object>) null).MustContainInstancesOfDifferentTypes();

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustContainInstancesOfDifferentTypes must throw a CollectionException when the specified collection contains null.")]
        public void CollectionContainsNull()
        {
            var collectionWithNull = new IInterface[] { new A(), null, new C() };

            Action act = () => collectionWithNull.MustContainInstancesOfDifferentTypes();

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain("The value must be a collection not containing null, but you specified null at index 1.");
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => InvalidTestData.MustContainInstancesOfDifferentTypes(exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => InvalidTestData.MustContainInstancesOfDifferentTypes(message: message)));
        }

        private interface IInterface { }

        private class A : IInterface { }

        private class B : IInterface { }

        private class C : A { }
    }
}