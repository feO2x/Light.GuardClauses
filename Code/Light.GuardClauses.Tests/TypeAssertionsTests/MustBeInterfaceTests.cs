using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeInterfaceTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeInterface must throw a TypeException when the specified type is no interface.")]
        public void IsNotInterface()
        {
            TestIsNotInterface(() => typeof(string).MustBeInterface(), typeof(string));
            TestIsNotInterface(() => typeof(double).GetTypeInfo().MustBeInterface(), typeof(double));
        }

        private static void TestIsNotInterface(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be an interface, but is not.");
        }

        [Fact(DisplayName = "MustBeInterface must not throw an exception when the specified type is an interface.")]
        public void IsInterface()
        {
            TestIsInterface(() => typeof(IConvertible).MustBeInterface());
            TestIsInterface(() => typeof(IDictionary<,>).GetTypeInfo().MustBeInterface());
        }

        private static void TestIsInterface(Action act)
        {
            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustBeInterface must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeInterface()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeInterface()).ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(ConsoleColor).MustBeInterface(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Action).MustBeInterface(message: message));

            testData.AddExceptionTest(exception => typeof(string).GetTypeInfo().MustBeInterface(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(int).GetTypeInfo().MustBeInterface(message: message));
        }
    }
}