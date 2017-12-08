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
    public sealed class MustBeDelegateTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeDelegate must throw a TypeException when the specified type is no delegate.")]
        public void IsNotDelegate()
        {
            TestIsNotDelegate(() => typeof(int).MustBeDelegate(), typeof(int));
            TestIsNotDelegate(() => typeof(string).GetTypeInfo().MustBeDelegate(), typeof(string));
        }

        private static void TestIsNotDelegate(Action act, Type type)
        {
            act.ShouldThrow<TypeException>()
               .And.Message.Should().Contain($"The type \"{type}\" must be a delegate, but it is not.");
        }

        [Fact(DisplayName = "MustBeDelegate must not throw an exception when the specified type is a delegate.")]
        public void IsDelegate()
        {
            typeof(Action).MustBeDelegate().Should().Be(typeof(Action));
            typeof(Predicate<string>).GetTypeInfo().MustBeDelegate().Should().Be(typeof(Predicate<string>).GetTypeInfo());
        }

        [Fact(DisplayName = "MustBeDelegate must throw an ArgumentNullException when parameter is null.")]
        public void ParameterNull()
        {
            new Action(() => ((Type) null).MustBeDelegate()).ShouldThrow<ArgumentNullException>();
            new Action(() => ((TypeInfo) null).MustBeDelegate()).ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => typeof(IList).MustBeDelegate(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(BindingFlags).MustBeDelegate(message: message));

            testData.AddExceptionTest(exception => typeof(string).GetTypeInfo().MustBeDelegate(exception: exception))
                    .AddMessageTest<TypeException>(message => typeof(IDisposable).GetTypeInfo().MustBeDelegate(message: message));
        }
    }
}