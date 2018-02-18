using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustContainOnlyLettersAndDigitsTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainOnlyLettersAndDigits must throw a StringException when the specified string contains characters that are no letters or digits.")]
        [InlineData("12!ab")]
        [InlineData("W|zzard")]
        [InlineData("This string contains whitespace.")]
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
        public void WithSpecialCharacters(string invalidString)
        {
            Action act = () => invalidString.MustContainOnlyLettersAndDigits(nameof(invalidString));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(invalidString)} must contain only letters or digits, but you specified \"{invalidString}\".");
        }

        [Theory(DisplayName = "MustContainOnlyLettersAndDigits must not throw an exception when the specified string contains only letters and digits.")]
        [InlineData("feO2x")]
        [InlineData("Tyrion")]
        [InlineData("1234")]
        public void WithoutSpecialCharacters(string validString)
        {
            var result = validString.MustContainOnlyLettersAndDigits();

            result.Should().BeSameAs(validString);
        }

        [Fact(DisplayName = "MustContainOnlyLettersAndDigits must throw an ArgumentNullException when the specified string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustContainOnlyLettersAndDigits();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustContainOnlyLettersAndDigits must throw an EmptyStringException when the specified string is empty.")]
        public void StringEmpty()
        {
            Action act = () => string.Empty.MustContainOnlyLettersAndDigits();

            act.Should().Throw<EmptyStringException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "200.00$".MustContainOnlyLettersAndDigits(exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "!".MustContainOnlyLettersAndDigits(message: message)));
        }
    }
}