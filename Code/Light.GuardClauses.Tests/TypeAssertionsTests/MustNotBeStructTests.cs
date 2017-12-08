using System;
using System.Collections;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeStructTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeStruct must throw a TypeException when the specified type is a struct.")]
        public void IsStruct()
        {
            TestIsStruct(() => typeof(decimal).MustNotBeStruct(), typeof(decimal));
            TestIsStruct(() => typeof(ConsoleKeyInfo).GetTypeInfo().MustNotBeStruct(), typeof(ConsoleKeyInfo));
        }

        private static void TestIsStruct(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must not be a struct, but it is.");
        }

        [Fact(DisplayName = "MustNotBeStruct must not throw an exception when the specified type is no struct.")]
        public void IsNotStruct()
        {
            typeof(IEnumerable).MustNotBeStruct().Should().Be(typeof(IEnumerable));
            typeof(Action).GetTypeInfo().MustNotBeStruct().Should().Be(typeof(Action).GetTypeInfo());
        }

        [Fact(DisplayName = "MustNotBeStruct must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustNotBeStruct()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustNotBeStruct()).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(int).MustNotBeStruct(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(ConsoleKeyInfo).MustNotBeStruct(message: message));

            testData.AddExceptionTest(exception => typeof(double).GetTypeInfo().MustNotBeStruct(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(long).GetTypeInfo().MustNotBeStruct(message: message));
        }
    }
}