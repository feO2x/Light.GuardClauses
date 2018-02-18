using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustNotBeNullReferenceTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustNotBeNullReference must throw an ArgumentNullException when a string reference is null.")]
        public static void StringNull()
        {
            CheckArgumentNullExceptionIsThrown(default(string));
        }

        [Fact(DisplayName = "MustNotBeNullReference must throw an ArgumentNullException when a delegate reference is null.")]
        public static void DelegateNull()
        {
            CheckArgumentNullExceptionIsThrown(default(Action));
        }

        [Fact(DisplayName = "MustNotBeNullReference must throw an ArgumentNullException when an object reference is null.")]
        public static void ObjectNull()
        {
            CheckArgumentNullExceptionIsThrown(default(object));
        }

        private static void CheckArgumentNullExceptionIsThrown<T>(T nullReference)
        {
            Action act = () => nullReference.MustNotBeNullReference(nameof(nullReference));

            act.Should().Throw<ArgumentNullException>()
               .And.Message.Should().Contain($"{nameof(nullReference)} must not be null.");
        }

        [Theory(DisplayName = "MustNotBeNullReference must not throw an exception when a value type is passed in.")]
        [InlineData(42)]
        [InlineData(true)]
        [InlineData(ConsoleKey.Escape)]
        public static void ValueType<T>(T value)
        {
            var result = value.MustNotBeNullReference();

            result.Should().Be(value);
        }

        [Theory(DisplayName = "MustNotBeNullReference must not throw an exception when a reference type is not null.")]
        [InlineData("Foo")]
        [InlineData(new []{1, 2, 3})]
        public static void ReferenceNotNull<T>(T reference) where T : class
        {
            var result = reference.MustNotBeNullReference(nameof(reference), "You shall not be null");

            result.Should().BeSameAs(reference);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => default(object).MustNotBeNullReference(exception))
                    .AddMessageTest<ArgumentNullException>(message => default(string).MustNotBeNullReference(message: message));
        }
    }
}