using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContain must throw a StringException when the specified text is not part of the given string.")]
        [InlineData("abc", "d")]
        [InlineData("Hello, World!", "You")]
        [InlineData("1, 2, 3", ". ")]
        public void StringDoesNotContainText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, parameterName: nameof(value));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"\"{value}\" must contain \"{containedText}\", but it does not.");
        }

        [Theory(DisplayName = "MustContain must not throw an exception when the text is contained.")]
        [InlineData("abc", "a")]
        [InlineData("Hello, World!", "orl")]
        [InlineData("1, 2, 3", ", ")]
        public void StringContainsText(string value, string containedText)
        {
            var result = value.MustContain(containedText, parameterName: nameof(value));

            result.Should().BeSameAs(value);
        }

        [Fact(DisplayName = "MustContain must not throw an exception when case sensitivity is turned off and the text is contained in the string.")]
        public void IgnoreCase()
        {
            const string text = "I suppose I'll have to kill the Mountain myself. Won't that make for a great song.";

            var result = text.MustContain("KILL", true);

            result.Should().BeSameAs(text);
        }

        [Fact(DisplayName = "MustContain must throw an exception when the specified text is null.")]
        public void ContainedTextNull()
        {
            Action act = () => "someText".MustContain(null);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("text");
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Hello there!".MustContain("world", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "42".MustContain("b", message: message)))
                    .Add(new CustomMessageTest<StringException>(message => string.Empty.MustContain("a", message: message)));
        }
    }
}