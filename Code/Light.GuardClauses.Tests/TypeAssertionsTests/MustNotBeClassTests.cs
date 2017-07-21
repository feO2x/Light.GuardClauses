using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeClassTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeClass must throw a TypeException when the specified type is a class.")]
        public void IsClass()
        {
            TestIsClass(() => typeof(object).MustNotBeClass(), typeof(object));
            TestIsClass(() => typeof(List<>).GetTypeInfo().MustNotBeClass(), typeof(List<>));
        }

        private static void TestIsClass(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must not be a class, but it is.");
        }

        [Fact(DisplayName = "MustNotBeClass must not throw an exception when the specified type is not a class.")]
        public void IsNotClass()
        {
            TestIsNotClass(() => typeof(Action).MustNotBeClass());
            TestIsNotClass(() => typeof(IEnumerator).GetTypeInfo().MustNotBeClass());
        }

        private static void TestIsNotClass(Action act)
        {
            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustNotBeClass must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustNotBeClass()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustNotBeClass()).ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(string).MustNotBeClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(object).MustNotBeClass(message: message));

            testData.AddExceptionTest(exception => typeof(Math).GetTypeInfo().MustNotBeClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Exception).GetTypeInfo().MustNotBeClass(message: message));
        }
    }
}