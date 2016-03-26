using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveLengthTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveLength must throw a StringException when the specified length is different from the string's length.")]
        [InlineData("Hello", 4)]
        [InlineData("", 1)]
        [InlineData("Turns out, far too much has been written about great men and not nearly enough about morons. Doesn't seem right.", 42)]
        public void LenthDifferent(string @string, int length)
        {
            Action act = () => @string.MustHaveLength(length, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must have a length of {length}, but it actually has a length of {@string.Length}.");
        }

        [Theory(DisplayName = "MustHaveLength must not throw an exception when the specified length is equal to the string's length.")]
        [InlineData("World")]
        [InlineData("")]
        [InlineData("I suppose I'll have to kill the Mountain myself.")]
        public void LengthEqual(string @string)
        {
            Action act = () => @string.MustHaveLength(@string.Length);

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustHaveLength must throw an ArgumentOutOfRangeException when length is less than zero.")]
        public void LengthLessThanZero()
        {
            Action act = () => "someText".MustHaveLength(-1);

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be("length");
        }

        [Fact(DisplayName = "MustHaveLength must throw an ArgumentNullException when the string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustHaveLength(42);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "someText".MustHaveLength(13, exception: exception)));

            testData.Add(new CustomMessageTest<StringException>(message => "foo".MustHaveLength(2, message: message)));
        }
    }
}