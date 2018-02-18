using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustNotEndWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWith for strings must throw a StringException when the string ends with the specified text (case-sensitivity respected).")]
        [InlineData("Foo", "Foo")]
        [InlineData("Most men would rather deny a hard truth than face it", "face it")]
        public void StringEndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText, nameof(@string));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not end with \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotEndWith for strings must not throw an exception when the string does not end with the specified text (case-sensitivity respected).")]
        [InlineData("Foo", "Bar")]
        [InlineData("Foo", "foo")]
        [InlineData("The things we love destroy us every time, lad. Remember that.", "Consider that.")]
        public void StringEndsDiffer(string @string, string endText)
        {
            var result = @string.MustNotEndWith(endText);

            result.Should().BeSameAs(@string);
        }

        [Theory(DisplayName = "MustNotEndWith for strings must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotEndWith("Foo", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Bar".MustNotEndWith("Bar", message: message)));
        }
    }
}