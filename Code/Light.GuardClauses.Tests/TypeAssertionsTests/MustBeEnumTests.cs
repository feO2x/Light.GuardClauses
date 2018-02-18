using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class MustBeEnumTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeEnum must throw a TypeException when the specified type is no enum.")]
        public void IsNotEnum()
        {
            TestIsNotEnum(() => typeof(Action).MustBeEnum(), typeof(Action));
            TestIsNotEnum(() => typeof(double).GetTypeInfo().MustBeEnum(), typeof(double));
        }

        private static void TestIsNotEnum(Action act, Type type)
        {
            act.Should().Throw<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be an enum, but it is not.");
        }

        [Fact(DisplayName = "MustBeEnum must not throw an exception when the specified type is an enum.")]
        public void IsEnum()
        {
            typeof(ConsoleKey).MustBeEnum().Should().Be(typeof(ConsoleKey));
            typeof(LockRecursionPolicy).GetTypeInfo().MustBeEnum().Should().Be(typeof(LockRecursionPolicy).GetTypeInfo());
        }

        [Fact(DisplayName = "MustBeEnum must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeEnum()).Should().Throw<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeEnum()).Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(string).MustBeEnum(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(IComparable).MustBeEnum(message: message));

            testData.AddExceptionTest(exception => typeof(object).GetTypeInfo().MustBeEnum(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(IList).GetTypeInfo().MustBeEnum(message: message));
        }
    }
}