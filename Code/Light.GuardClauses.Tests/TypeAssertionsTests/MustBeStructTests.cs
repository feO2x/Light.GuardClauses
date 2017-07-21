using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeStructTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeStruct must throw a TypeException when the specified type is no struct.")]
        public void IsNotStruct()
        {
            TestIsNotStruct(() => typeof(object).MustBeStruct(), typeof(object));
            TestIsNotStruct(() => typeof(Action<string>).GetTypeInfo().MustBeStruct(), typeof(Action<string>));
        }

        private static void TestIsNotStruct(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be a struct, but it is not.");
        }

        [Fact(DisplayName = "MustBeStruct must not throw an exception when the specified type is a struct.")]
        public void IsStruct()
        {
            TestIsStruct(() => typeof(int).MustBeStruct());
            TestIsStruct(() => typeof(float).MustBeStruct());
        }

        private static void TestIsStruct(Action act)
        {
            act.ShouldNotThrow();
        }

        [Fact (DisplayName = "MustBeStruct must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeStruct()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeStruct()).ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(IDisposable).MustBeStruct(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(ConsoleKey).MustBeStruct(message: message));

            testData.AddExceptionTest(exception => typeof(Action).GetTypeInfo().MustBeStruct(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(BindingFlags).GetTypeInfo().MustBeStruct(message: message));
        }
    }
}