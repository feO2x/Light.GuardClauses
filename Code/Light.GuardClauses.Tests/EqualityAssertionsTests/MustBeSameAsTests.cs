using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EqualityAssertionsTests
{
    public sealed class MustBeSameAsTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeSameAs must throw an ArgumentException when the two specified references do not point to the same instance.")]
        [InlineData("Hello", "World")]
        [InlineData("1", "2")]
        public void ReferencesDifferent(string first, string second)
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
            var result = reference.MustBeSameAs(reference);

            result.Should().BeSameAs(reference);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "foo".MustBeSameAs("bar", exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => "foo".MustBeSameAs("bar", message: message)));
        }
    }
}