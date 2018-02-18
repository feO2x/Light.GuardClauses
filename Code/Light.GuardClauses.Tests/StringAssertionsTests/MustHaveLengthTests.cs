using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
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

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must have a length of {length}, but it actually has a length of {@string.Length}.");
        }

        [Theory(DisplayName = "MustHaveLength must not throw an exception when the specified length is equal to the string's length.")]
        [InlineData("World")]
        [InlineData("")]
        [InlineData("I suppose I'll have to kill the Mountain myself.")]
        public void LengthEqual(string @string)
        {
            var result = @string.MustHaveLength(@string.Length);

            result.Should().BeSameAs(@string);
        }

        [Fact(DisplayName = "MustHaveLength must throw an ArgumentOutOfRangeException when length is less than zero.")]
        public void LengthLessThanZero()
        {
            Action act = () => "someText".MustHaveLength(-1);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be("length");
        }

        [Fact(DisplayName = "MustHaveLength must throw an ArgumentNullException when the string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustHaveLength(42);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "someText".MustHaveLength(13, exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "foo".MustHaveLength(2, message: message)));
        }
    }
}