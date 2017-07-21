using System;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotBeEnumTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeEnum must throw a TypeException when the specified type is an enum.")]
        public void IsEnum()
        {
            TestIsEnum(() => typeof(ConsoleColor).MustNotBeEnum(), typeof(ConsoleColor));
            TestIsEnum(() => typeof(BindingFlags).GetTypeInfo().MustNotBeEnum(), typeof(BindingFlags));
        }

        private static void TestIsEnum(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must not be an enum, but it is.");
        }

        [Fact(DisplayName = "MustNotBeEnum must not throw an exception when the specified type is no enum.")]
        public void IsNotEnum()
        {
            TestIsNotEnum(() => typeof(int).MustNotBeEnum());
            TestIsNotEnum(() => typeof(IEquatable<string>).GetTypeInfo().MustNotBeEnum());
        }

        private static void TestIsNotEnum(Action act)
        {
            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustNotBeEnum must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustNotBeEnum()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustNotBeEnum()).ShouldThrow<ArgumentNullException>();
        }


        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(ConsoleColor).MustNotBeEnum(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(ConsoleColor).MustNotBeEnum(message: message));

            testData.AddExceptionTest(exception => typeof(GCCollectionMode).GetTypeInfo().MustNotBeEnum(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(AttributeTargets).GetTypeInfo().MustNotBeEnum(message: message));
        }
    }
}