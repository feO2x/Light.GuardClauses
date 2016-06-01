using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotEndWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWith must throw a StringException when the string ends with the specified text (case-sensitivity respected).")]
        [InlineData("Foo", "Foo")]
        [InlineData("Most men would rather deny a hard truth than face it", "face it")]
        public void EndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not end with \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotEndWith must not throw an exception when the string does not end with the specified text (case-sensitivity respected).")]
        [InlineData("Foo", "Bar")]
        [InlineData("Foo", "foo")]
        [InlineData("The things we love destroy us every time, lad. Remember that.", "Consider that.")]
        public void EndsDiffer(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotEndWith must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotEndWith("Foo", exception: exception)));

            testData.Add(new CustomMessageTest<StringException>(message => "Bar".MustNotEndWith("Bar", message: message)));
        }
    }
}