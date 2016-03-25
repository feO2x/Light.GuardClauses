using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class StringMustNotContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContain must throw an exception when the specified string contains the given text.")]
        [InlineData("abc", "b")]
        [InlineData("Say herro to my littre friend", "herro")]
        public void TextContained(string value, string containedText)
        {
            Action act = () => value.MustNotContain(containedText, nameof(value));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(value)} must not contain the text \"{containedText}\", but you specified \"{value}\".");
        }

        [Theory(DisplayName = "MustNotContain must not throw an exception when the specified string does not contain the given text.")]
        [InlineData("1, 2, 3", ".")]
        [InlineData("Say herro to my littre friend", "hello")]
        public void TextNotContained(string value, string containedText)
        {
            Action act = () => value.MustNotContain(containedText, nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotContain must throw an exception when the two strings have the same content with different capital letters.")]
        [InlineData("I AM YOUR MASTER", "am your")]
        [InlineData("Where is the LIGHT?", "light")]
        [InlineData("PwnD", "pwnd")]
        public void CompareCaseInsensitive(string @string, string comparedText)
        {
            Action act = () => @string.MustNotContain(comparedText, ignoreCaseSensitivity: true);

            act.ShouldThrow<StringException>();
        }

        [Fact(DisplayName = "MustNotContain must throw an exception when the specified text is null.")]
        public void ContainedTextNull()
        {
            Action act = () => "someText".MustNotContain(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("text");
        }

        [Fact(DisplayName = "MustNotContain must throw an exception when the specified text is an empty string.")]
        public void ContainedTextEmpty()
        {
            Action act = () => "someText".MustNotContain(string.Empty);

            act.ShouldThrow<EmptyStringException>()
               .And.ParamName.Should().Be("text");
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "I am here".MustNotContain("am", exception: exception)));
            testData.Add(new CustomExceptionTest(exception => ((string) null).MustNotContain("Foo", exception: exception)));
            testData.Add(new CustomExceptionTest(exception =>
                                                 "When you play the game of thrones you win, or you die. There is no middle ground.".MustNotContain("game of thrones", exception: exception)));

            testData.Add(new CustomMessageTest<StringException>(message => "I am here".MustNotContain("am", message: message)));
            testData.Add(new CustomMessageTest<ArgumentNullException>(message => ((string) null).MustNotContain("Foo", message: message)));
        }
    }
}