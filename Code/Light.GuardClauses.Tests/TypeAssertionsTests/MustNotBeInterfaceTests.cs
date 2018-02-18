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
            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must not be an interface, but it is.");
        }

        [Fact(DisplayName = "MustNotBeInterface must not throw an exception when the specified type is no interface.")]
        public void IsNotInterface()
        {
            typeof(string).MustNotBeInterface().Should().Be(typeof(string));
            typeof(Assembly).GetTypeInfo().MustNotBeInterface().Should().Be(typeof(Assembly).GetTypeInfo());
        }

        [Fact(DisplayName = "MustNotBeInterface must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustNotBeInterface()).Should().Throw<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustNotBeInterface()).Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(IContainer).MustNotBeInterface(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(IChangeTracking).MustNotBeInterface(message: message));

            testData.AddExceptionTest(exception => typeof(IEqualityComparer<>).GetTypeInfo().MustNotBeInterface(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(ICommand).GetTypeInfo().MustNotBeInterface(message: message));
        }
    }
}