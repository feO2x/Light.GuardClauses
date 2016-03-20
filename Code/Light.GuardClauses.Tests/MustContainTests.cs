using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainTests
    {
        [Theory(DisplayName = "MustContain must throw a StringException when the specified text is not part of the given string.")]
        [InlineData("abc", "d")]
        [InlineData("Hello, World!", "You")]
        [InlineData("1, 2, 3", ". ")]
        public void StringDoesNotContainText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, nameof(value));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(value)} must contain the text \"{containedText}\", but you specified \"{value}\".");
        }

        [Theory(DisplayName = "MustContain must not throw an exception when the text is contained.")]
        [InlineData("abc", "a")]
        [InlineData("Hello, World!", "orl")]
        [InlineData("1, 2, 3", ", ")]
        public void StringContainsText(string value, string containedText)
        {
            Action act = () => value.MustContain(containedText, nameof(value));

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "The caller can specify a custom message that MustContain must inject instead of the default one.")]
        [InlineData(null, "")]
        [InlineData("", "a")]
        [InlineData("42", "b")]
        public void CustomMessage(string invalidString, string containedText)
        {
            const string message = "Thou shall have the contained text!";

            Action act = () => invalidString.MustContain(containedText, message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain(message);
        }

        [Theory(DisplayName = "The caller can specify a custom exception that MustContain must raise instead of the default one.")]
        [InlineData(null, "")]
        [InlineData("Hello there!", "world")]
        public void CustomException(string invalidString, string containedText)
        {
            var exception = new Exception();

            Action act = () => invalidString.MustContain(containedText, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact(DisplayName = "MustContain must throw an exception when the specified containedText is null.")]
        public void ContainedTextNull()
        {
            Action act = () => "someText".MustContain(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain("You called MustContain wrongly by specifying null for containedText.");
        }

        [Fact(DisplayName = "MustContain must throw an exception when the specified containedText is an empty string.")]
        public void ContainedTextEmpty()
        {
            Action act = () => "someText".MustContain(string.Empty);

            act.ShouldThrow<EmptyStringException>()
               .And.Message.Should().Contain("You called MustContain wrongly by specifying an empty string for containedText.");
        }
    }
}