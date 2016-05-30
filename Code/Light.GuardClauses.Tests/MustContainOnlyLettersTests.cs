using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
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

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(invalidString)} must contain only letters, but you specified \"{invalidString}\".");
        }

        [Theory(DisplayName = "MustContainOnlyLetters must not throw an exception when the specified string contains only letters.")]
        [InlineData("Frank")]
        [InlineData("abcd")]
        public void OnlyLetters(string validString)
        {
            Action act = () => validString.MustContainOnlyLetters();

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustContainOnlyLetters must throw an ArgumentNullException when the specified string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustContainOnlyLetters();

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustContainOnlyLetters must throw an EmptyStringException when the specified string is empty.")]
        public void StringEmpty()
        {
            Action act = () => string.Empty.MustContainOnlyLetters();

            act.ShouldThrow<EmptyStringException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "!".MustContainOnlyLetters(exception: exception)));

            testData.Add(new CustomMessageTest<StringException>(message => "$".MustContainOnlyLetters(message: message)));
        }
    }
}