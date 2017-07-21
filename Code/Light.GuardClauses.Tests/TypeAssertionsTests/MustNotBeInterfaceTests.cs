using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeInterfaceTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeInterface must throw a TypeException when the specified type is an interface.")]
        public void IsInterface()
        {
            TestIsInterface(() => typeof(IServiceProvider).MustNotBeInterface(), typeof(IServiceProvider));
            TestIsInterface(() => typeof(ICommand).GetTypeInfo().MustNotBeInterface(), typeof(ICommand));
        }

        private static void TestIsInterface(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must not be an interface, but it is.");
        }

        [Fact(DisplayName = "MustNotBeInterface must not throw an exception when the specified type is no interface.")]
        public void IsNotInterface()
        {
            TestIsNotInterface(() => typeof(string).MustNotBeInterface());
            TestIsNotInterface(() => typeof(Assembly).GetTypeInfo().MustNotBeInterface());
        }

        private static void TestIsNotInterface(Action act)
        {
            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustNotBeInterface must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustNotBeInterface()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustNotBeInterface()).ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(IContainer).MustNotBeInterface(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(IChangeTracking).MustNotBeInterface(message: message));

            testData.AddExceptionTest(exception => typeof(IEqualityComparer<>).GetTypeInfo().MustNotBeInterface(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(ICommand).GetTypeInfo().MustNotBeInterface(message: message));
        }
    }
}