using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustContainOnlyLettersTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainOnlyLetters must throw a StringExceptions when the specified string contains digits or special characters.")]
        [InlineData("abc123")]
        [InlineData("feO2x")]
        [InlineData("Here is a sentence.")]
        [InlineData("!")]
        [InlineData("\"")]
        [InlineData("§")]
        [InlineData("$")]
        [InlineData("%")]
        [InlineData("&")]
        [InlineData("/")]
        [InlineData("(")]
        [InlineData(")")]
        [InlineData("=")]
        [InlineData("?")]
        [InlineData("\\")]
        [InlineData("`")]
        [InlineData("´")]
        [InlineData("°")]
        [InlineData("^")]
        [InlineData("{")]
        [InlineData("}")]
        [InlineData("[")]
        [InlineData("]")]
        [InlineData("*")]
        [InlineData("+")]
        [InlineData("~")]
        [InlineData("'")]
        [InlineData("#")]
        [InlineData("|")]
        public void NotOnlyLetters(string invalidString)
        {
            Action act = () => invalidString.MustContainOnlyLetters(nameof(invalidString));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(invalidString)} must contain only letters, but you specified \"{invalidString}\".");
        }

        [Theory(DisplayName = "MustContainOnlyLetters must not throw an exception when the specified string contains only letters.")]
        [InlineData("Frank")]
        [InlineData("abcd")]
        public void OnlyLetters(string validString)
        {
            var result = validString.MustContainOnlyLetters();

            result.Should().BeSameAs(validString);
        }

        [Fact(DisplayName = "MustContainOnlyLetters must throw an ArgumentNullException when the specified string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustContainOnlyLetters();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustContainOnlyLetters must throw an EmptyStringException when the specified string is empty.")]
        public void StringEmpty()
        {
            Action act = () => string.Empty.MustContainOnlyLetters();

            act.Should().Throw<EmptyStringException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "!".MustContainOnlyLetters(exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "$".MustContainOnlyLetters(message: message)));
        }
    }
}