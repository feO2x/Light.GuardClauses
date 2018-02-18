using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustEndWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustEndWithEquivalentOf for strings must throw a StringException when the string does not end with the specified text (ignoring case-sensitivity).")]
        [InlineData("Foo", "Bar")]
        [InlineData("This should end", "begin")]
        [InlineData("I'm the captain. If this ship goes down, I go down with it.", "This Ship goes down")]
        public void StringEndsDiffers(string @string, string endText)
        {
            Action act = () => @string.MustEndWithEquivalentOf(endText, nameof(@string));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must end with the equivalent of \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for strings must not throw an exception when the string ends with the specified text (ignoring case-sensitivity).")]
        [InlineData("This is the end", "is the end")]
        [InlineData("Hello", "LO")]
        [InlineData("Why are all the gods such vicious cunts? Where is the god of tits and wine?", "Tits and Wine?")]
        public void StringEndsEqual(string @string, string endText)
        {
            var result = @string.MustEndWithEquivalentOf(endText);

            result.Should().BeSameAs(@string);
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for strings must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustEndWithEquivalentOf(endText);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "The Lord of Light wants his enemies burnt".MustEndWithEquivalentOf("One should treat others as one would like others to treat oneself", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "The Drowned God wants his enemies drowned".MustEndWithEquivalentOf("altruism", message: message)));
        }
    }
}