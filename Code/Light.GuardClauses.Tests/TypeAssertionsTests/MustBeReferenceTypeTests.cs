using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeReferenceTypeTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeReferenceType must throw a TypeException when the specified type is a value type.")]
        public void IsValueType()
        {
            TestIsValueType(() => typeof(int).MustBeReferenceType(), typeof(int));
            TestIsValueType(() => typeof(ConsoleColor).GetTypeInfo().MustBeReferenceType(), typeof(ConsoleColor));
        }

        private static void TestIsValueType(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be a reference type, but it is a value type");
        }

        [Fact(DisplayName = "MustBeReferenceType must not throw an exception when the specified type is a reference type.")]
        public void IsReferenceType()
        {
            typeof(string).MustBeReferenceType().Should().Be(typeof(string));
            typeof(Action).GetTypeInfo().MustBeReferenceType().Should().Be(typeof(Action).GetTypeInfo());
        }

        [Fact(DisplayName = "MustBeReferenceType must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeReferenceType()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeReferenceType()).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(int).MustBeReferenceType(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(float).MustBeReferenceType(message: message));

            testData.AddExceptionTest(exception => typeof(decimal).GetTypeInfo().MustBeReferenceType(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(BindingFlags).GetTypeInfo().MustBeReferenceType(message: message));
        }
    }
}