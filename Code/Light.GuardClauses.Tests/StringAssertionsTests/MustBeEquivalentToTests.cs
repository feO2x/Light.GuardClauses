using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustBeEquivalentToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeEquivalentTo must throw a StringException when the two strings are not equivalent.")]
        [InlineData("Foo", "foo", StringComparison.CurrentCulture)]
        [InlineData("Foo", "Bar", StringComparison.OrdinalIgnoreCase)]
        public void StringsNotEquivalent(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustBeEquivalentTo(second, comparisonType, nameof(first));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"\"{first}\" must be equivalent to \"{second}\" (using {comparisonType}), but it is not");
        }

        [Theory(DisplayName = "MustBeEquivalentTo must not throw an exception when the two string are equivalent.")]
        [InlineData("Foo", "Foo")]
        [InlineData("Foo", "foo")]
        [InlineData("bar", "bAR")]
        public void StringsEquivalent(string first, string second)
        {
            var result = first.MustBeEquivalentTo(second);

            result.Should().BeSameAs(first);
        }

        [Theory(DisplayName = "MustBeEquivalentTo must throw an ArgumentNullException when parameter or other is null.")]
        [InlineData("Foo", null)]
        [InlineData(null, "Foo")]
        public void ArgumentsNull(string first, string second)
        {
            Action act = () => first.MustBeEquivalentTo(second);

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "Foo".MustBeEquivalentTo("Bar", exception: exception))
                    .AddMessageTest<StringException>(message => "Foo".MustBeEquivalentTo("Bar", message: message));
        }
    }
}