using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeSameAsTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeSameAs must throw an ArgumentException when the two specified references do not point to the same instance.")]
        [InlineData("Hello", "World")]
        [InlineData("1", "2")]
        [InlineData(new object[] { }, new object[] { "Foo" })]
        public void ReferencesDifferent<T>(T first, T second) where T : class
        {
            Action act = () => first.MustBeSameAs(second, nameof(first));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(first)} must point to the object instance \"{second}\", but it does not.");
        }

        [Theory(DisplayName = "MustBeSameAs must not throw an exception when the two specified references point to the same instance.")]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void ReferencesEqual(string reference)
        {
            Action act = () => reference.MustBeSameAs(reference);

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "foo".MustBeSameAs("bar", exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentException>(message => "foo".MustBeSameAs("bar", message: message)));
        }
    }
}