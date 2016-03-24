using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class StringMustContainTests : ICustomMessageAndExceptionTestDataProvider
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

        [Fact(DisplayName = "MustContain must throw an exception when the specified text is null.")]
        public void ContainedTextNull()
        {
            Action act = () => "someText".MustContain(null);

            act.ShouldThrow<ArgumentNullException>()
               .And.Message.Should().Contain("You called MustContain wrongly by specifying null for text.");
        }

        [Fact(DisplayName = "MustContain must throw an exception when the specified text is an empty string.")]
        public void ContainedTextEmpty()
        {
            Action act = () => "someText".MustContain(string.Empty);

            act.ShouldThrow<EmptyStringException>()
               .And.Message.Should().Contain("You called MustContain wrongly by specifying an empty string for text.");
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Hello there!".MustContain("world", exception: exception)));
            testData.Add(new CustomExceptionTest(exception =>
                                                 {
                                                     string @string = null;
                                                     // ReSharper disable once ExpressionIsAlwaysNull
                                                     @string.MustContain(string.Empty, exception: exception);
                                                 }));

            testData.Add(new CustomMessageTest<StringException>(message => "42".MustContain("b", message: message)));
            testData.Add(new CustomMessageTest<StringException>(message => string.Empty.MustContain("a", message: message)));
            testData.Add(new CustomMessageTest<ArgumentNullException>(message =>
                                                                      {
                                                                          string @string = null;
                                                                          // ReSharper disable once ExpressionIsAlwaysNull
                                                                          @string.MustContain(string.Empty, message: message);
                                                                      }));
        }
    }
}