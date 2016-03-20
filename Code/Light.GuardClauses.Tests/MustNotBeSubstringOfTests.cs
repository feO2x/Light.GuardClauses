using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeSubstringOfTests
    {
        [Theory(DisplayName = "MustNotBeSubstringOf must throw a StringException when the specified string is a substring of text.")]
        [InlineData("abc", "abc")]
        [InlineData("123", "01234")]
        [InlineData("123", "01234")]
        [InlineData("wear this", " I shall wear this as a badge of honor.")]
        public void IsSubstring(string invalidString, string text)
        {
            Action act = () => invalidString.MustNotBeSubstringOf(text, nameof(invalidString));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(invalidString)} must not be a substring of \"{text}\", but you specified \"{invalidString}\".");
        }

        [Theory(DisplayName = "MustNotBeSubstringOf must not throw an exception when the specified string is no substring of text.")]
        [InlineData("abc", "123")]
        [InlineData("AB", "ab")]
        [InlineData("12", "23")]
        [InlineData("Confess!", "Look at me! Look at my face!")]
        public void IsNotSubstring(string validString, string text)
        {
            Action act = () => validString.MustNotBeSubstringOf(text);

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustBeSubstringOf must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall not be part of the other!";

            Action act = () => "someText".MustNotBeSubstringOf("someText", message: message);

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeSubstringOf must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => "someText".MustNotBeSubstringOf("someText", exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        [Theory(DisplayName = "MustNotBeSubstringOf must throw an exception when the specified string is a substring of text, ignoring case-sensitivity.")]
        [InlineData("AB", "ab")]
        [InlineData("I AM", "I am here to serve you")]
        public void IgnoreCaseSensitivity(string invalidString, string text)
        {
            Action act = () => invalidString.MustNotBeSubstringOf(text, ignoreCaseSensitivity: true);

            act.ShouldThrow<StringException>();
        }
    }
}