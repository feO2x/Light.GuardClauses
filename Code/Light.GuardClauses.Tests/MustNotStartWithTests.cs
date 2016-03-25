using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotStartWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotStartWith must throw a StringException when the string starts with the specified text.")]
        [InlineData("Hello", "Hell")]
        [InlineData("Foo", "Foo")]
        [InlineData("Would it be excessive of me to ask you to save my life twice in a week?", "Would it be excessive")]
        public void StartTextEqual(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWith(startText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not start with \"{startText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotStartWith must not throw an exception when the string does not start with the specified text.")]
        [InlineData("Hello", "World")]
        [InlineData("Foo", "Bar")]
        [InlineData("Those are brave men knocking at our door. Let's go kill them!", "These are brave")]
        public void StartTextDifferent(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWith(startText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotStartWith must throw an ArgumentNullException when parameter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWith(startText);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => ((string) null).MustNotStartWith("foo", exception: exception)));
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotStartWith("Foo", exception: exception)));

            testData.Add(new CustomMessageTest<ArgumentNullException>(message => ((string) null).MustNotStartWith("foo", message: message)));
            testData.Add(new CustomMessageTest<StringException>(message => "Foo".MustNotStartWith("Foo", message: message)));
        }
    }
}