using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustStartWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustStartWith must throw a StringException when the string does not start with the specified text.")]
        [InlineData("Hello", "World")]
        [InlineData("Foo", "Bar")]
        [InlineData("Hey There", "hey")]
        [InlineData("A man with no motive is a man no one suspects. Always keep your foes confused.", "A max")]
        public void StartTextDifferent(string @string, string startText)
        {
            Action act = () => @string.MustStartWith(startText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must start with \"{startText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustStartWith must not throw an exception when the string starts with the specified text.")]
        [InlineData("Hello", "Hell")]
        [InlineData("Foo", "Foo")]
        [InlineData("It's not easy being drunk all the time. Everyone would do it, if it were easy.", "It's not easy")]
        public void StartTextEqual(string @string, string startText)
        {
            Action act = () => @string.MustStartWith(startText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustStartWith must throw an ArgumentNullException when parameter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string startText)
        {
            Action act = () => @string.MustStartWith(startText);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => ((string) null).MustStartWith("foo", exception: exception)));
            testData.Add(new CustomExceptionTest(exception => "Foo".MustStartWith("Bar", exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentNullException>(message => ((string) null).MustStartWith("foo", message: message)));
            testData.Add(new CustomMessageTest<StringException>(message => "Foo".MustStartWith("Bar", message: message)));
        }
    }
}