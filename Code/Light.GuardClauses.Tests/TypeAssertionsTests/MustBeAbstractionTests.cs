using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustBeAbstractionTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeAbstract must not throw an exception when the specified type is an interface.")]
        public void Interface()
        {
            Action act = () => typeof(IProgress<>).MustBeAbstraction();

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustBeAbstract must not throw an exception when the specified type is an abstract class.")]
        public void AbstractClass()
        {
            var type = typeof(CollectionBase);

            var result = type.MustBeAbstraction();

            result.Should().BeSameAs(type);
        }

        [Theory(DisplayName = "MustBeAbstract must throw a TypeException when the specified type is an instantiatable class.")]
        [InlineData(typeof(int))]
        [InlineData(typeof(Action))]
        [InlineData(typeof(List<>))]
        [InlineData(typeof(Console))]
        public void InstantiatableTypes(Type type)
        {
            Action act = () => type.MustBeAbstraction(nameof(type));

            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"{nameof(type)} \"{type}\" must be an abstract base class or interface, but it is not.");
        }

        [Fact(DisplayName = "MustBeAbstract must throw an ArgumentNullException when the specified type is null.")]
        public void ArgumentNull()
        {
            Action act = () => ((Type) null).MustBeAbstraction();

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(double).MustBeAbstraction(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Version).MustBeAbstraction(message: message));
        }
    }
}