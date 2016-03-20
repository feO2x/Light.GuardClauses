using FluentAssertions;
using Light.GuardClauses.Exceptions;
using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotContainTests
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

        [Theory(DisplayName = "The caller can specify a custom message that MustNotContain must inject instead of the default one.")]
        [MemberData(nameof(CustomTestData))]
        public void CustomMessage(string invalidString, string containedText)
        {
            const string message = "Thou shall not contain the other!";

            Action act = () => invalidString.MustNotContain(containedText, message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Be(message);
        }

        [Theory(DisplayName = "The caller can specify a custom exception that MustNotContain must raise instead of the default one.")]
        [MemberData(nameof(CustomTestData))]
        public void CustomException(string invalidString, string containedText)
        {
            var exception = new Exception();

            Action act = () => invalidString.MustNotContain(containedText, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        public static readonly TestData CustomTestData =
            new[]
            {
                new object[] { "I am here", "am"},
                new object[] {null, "foo"}, 
                new object[] { "When you play the game of thrones you win, or you die. There is no middle ground.", "game of thrones"}
            };
    }
}
