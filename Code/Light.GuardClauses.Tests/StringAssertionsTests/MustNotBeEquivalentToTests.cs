using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustNotBeEquivalentToTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeEquivalentTo must throw a StringException when the two strings are equivalent.")]
        [InlineData("Foo", "foo", StringComparison.CurrentCultureIgnoreCase)]
        [InlineData("Foo", "Foo", StringComparison.CurrentCulture)]
        public void StringsEquivalent(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustNotBeEquivalentTo(second, comparisonType, nameof(first));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"\"{first}\" must not be equivalent to \"{second}\" (using {comparisonType}), but it is.");
        }

        [Theory(DisplayName = "MustBeEquivalentTo must not throw an exception when the two string are not equivalent.")]
        [InlineData("Foo", "Bar")]
        [InlineData("Foo", "Baz")]
        public void StringsNotEquivalent(string first, string second)
        {
            var result = first.MustNotBeEquivalentTo(second);

            result.Should().BeSameAs(first);
        }

        [Theory(DisplayName = "MustNotBeEquivalentTo must throw an ArgumentNullException when parameter or other is null.")]
        [InlineData("Foo", null)]
        [InlineData(null, "Foo")]
        public void ArgumentsNull(string first, string second)
        {
            Action act = () => first.MustNotBeEquivalentTo(second);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "Foo".MustNotBeEquivalentTo("foo", exception: exception))
                    .AddMessageTest<StringException>(message => "Foo".MustNotBeEquivalentTo("Foo", message: message));
        }
    }
}