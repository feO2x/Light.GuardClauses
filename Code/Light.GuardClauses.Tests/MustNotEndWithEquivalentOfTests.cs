using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotEndWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWithEquivalentOf must throw a StringException when the string ends with the specified text (case-sensitivity ignored).")]
        [InlineData("Foo", "Foo")]
        [InlineData("Bar", "bAR")]
        [InlineData("Death is so terribly final, while life is full of possibilities", "Full of Possibilities")]
        public void EndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWithEquivalentOf(endText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not end with equivalent of \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf must not throw an exception when the string does not end with the specified text (case-sensitivity ignored).")]
        [InlineData("Foo", "Bar")]
        [InlineData("And I have a tender spot in my heart for cripples and bastards and broken things", "broken glass")]
        public void EndsDiffer(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWithEquivalentOf(endText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWithEquivalentOf(endText);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotEndWithEquivalentOf("foo", exception: exception)));

            testData.Add(new CustomMessageTest<StringException>(message => "Bar".MustNotEndWithEquivalentOf("Bar", message: message)));
        }
    }
}