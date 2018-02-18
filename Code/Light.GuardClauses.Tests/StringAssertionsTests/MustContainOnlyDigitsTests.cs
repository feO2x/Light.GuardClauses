using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustContainOnlyDigitsTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainOnlyDigits must throw a StringException when the specified string does not contain only digits.")]
        [InlineData("Foo")]
        [InlineData("123Bar")]
        [InlineData("Baz881")]
        [InlineData("19j557")]
        [InlineData("")]
        public void NotOnlyDigits(string @string)
        {
            Action act = () => @string.MustContainOnlyDigits(nameof(@string));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must contain only digits, but you specified \"{@string}\".");
        }

        [Theory(DisplayName = "MustContainOnlyDigits must return the string when it contains only digits.")]
        [InlineData("123")]
        [InlineData("87345212")]
        [InlineData("54091")]
        [InlineData("0")]
        public void OnlyDigits(string @string)
        {
            var result = @string.MustContainOnlyDigits();

            result.Should().BeSameAs(@string);
        }

        [Fact(DisplayName = "MustContainOnlyDigits must throw an ArgumentNullException when the specified string is null.")]
        public void ArgumentNull()
        {
            Action act = () => ((string) null).MustContainOnlyDigits();

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "abcde".MustContainOnlyDigits(exception: exception))
                    .AddMessageTest<StringException>(message => "Foo".MustContainOnlyDigits(message: message));
        }
    }
}