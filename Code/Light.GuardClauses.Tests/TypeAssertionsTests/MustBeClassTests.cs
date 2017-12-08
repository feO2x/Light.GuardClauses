using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustBeClassTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeClass must throw a TypeException when the specified type is not a class.")]
        public void IsNotClass()
        {
            TestIsNotClass(() => typeof(IList<>).MustBeClass(), typeof(IList<>));
            TestIsNotClass(() => typeof(ConsoleColor).GetTypeInfo().MustBeClass(), typeof(ConsoleColor));
        }

        private static void TestIsNotClass(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be a class, but it is not.");
        }

        [Fact(DisplayName = "MustBeClass must not throw an exception when the specified type is a class.")]
        public void IsClass()
        {
            typeof(string).MustBeClass().Should().Be(typeof(string));
            typeof(object).GetTypeInfo().MustBeClass().Should().Be(typeof(object).GetTypeInfo());
        }

        [Fact(DisplayName = "MustBeClass must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeClass()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeClass()).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(IComparable).MustBeClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Func<object>).MustBeClass(message: message));

            testData.AddExceptionTest(exception => typeof(double).GetTypeInfo().MustBeClass(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(Action).GetTypeInfo().MustBeClass(message: message));
        }
    }
}