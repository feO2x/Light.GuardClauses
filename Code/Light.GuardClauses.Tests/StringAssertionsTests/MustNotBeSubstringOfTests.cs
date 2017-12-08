using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustNotBeSubstringOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotBeSubstringOf must throw a StringException when the specified string is a substring of text.")]
        [InlineData("abc", "abc")]
        [InlineData("123", "01234")]
        [InlineData("wear this", " I shall wear this as a badge of honor.")]
        public void IsSubstring(string invalidString, string text)
        {
            Action act = () => invalidString.MustNotBeSubstringOf(text, parameterName: nameof(invalidString));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"\"{invalidString}\" must not be a substring of \"{text}\", but it is.");
        }

        [Theory(DisplayName = "MustNotBeSubstringOf must not throw an exception when the specified string is no substring of text.")]
        [InlineData("abc", "123")]
        [InlineData("AB", "ab")]
        [InlineData("12", "23")]
        [InlineData("Confess!", "Look at me! Look at my face!")]
        public void IsNotSubstring(string validString, string text)
        {
            var result = validString.MustNotBeSubstringOf(text);

            result.Should().BeSameAs(validString);
        }

        [Theory(DisplayName = "MustNotBeSubstringOf must throw an exception when the specified string is a substring of text, ignoring case-sensitivity.")]
        [InlineData("AB", "ab")]
        [InlineData("I AM", "I am here to serve you")]
        public void IgnoreCaseSensitivity(string invalidString, string text)
        {
            Action act = () => invalidString.MustNotBeSubstringOf(text, true);

            act.ShouldThrow<StringException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "someText".MustNotBeSubstringOf("someText", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "someText".MustNotBeSubstringOf("someText", message: message)));
        }
    }
}