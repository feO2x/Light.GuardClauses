using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustHaveMaximumLengthTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveMaximumLength must throw a StringException when the specified string is longer than the maximum length.")]
        [InlineData("Foo", 2)]
        [InlineData("I am the Imp; I have certain standards to maintain.", 20)]
        public void Longer(string @string, int maximumLength)
        {
            Action act = () => @string.MustHaveMaximumLength(maximumLength, nameof(@string));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} \"{@string}\" must have a maximum length of {maximumLength}, but it actually is {@string.Length} characters long.");
        }

        [Theory(DisplayName = "MustHaveMaximumLength must not throw an exception when the specified string has at least the same length as minimumLength.")]
        [InlineData("Foo", 3)]
        [InlineData("Bar", 4)]
        [InlineData("", 0)]
        [InlineData("Can I drink myself to death on the road to Meereen?", 82)]
        public void SameLengthOrShorter(string @string, int maximumLength)
        {
            var result = @string.MustHaveMaximumLength(maximumLength);

            result.Should().BeSameAs(@string);
        }

        [Theory(DisplayName = "MustHaveMaximumLength must throw an ArgumentOutOfRangeException when the specified maximum length is negative.")]
        [InlineData(-2)]
        [InlineData(-1)]
        [InlineData(-67612)]
        public void MinimumLengthNegative(int maximumLength)
        {
            Action act = () => "Foo".MustHaveMaximumLength(maximumLength);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be(nameof(maximumLength));
        }

        [Fact(DisplayName = "MustHaveMaximumLength must throw an ArgumentNullException when the specified string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustHaveMaximumLength(42);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "Foo".MustHaveMaximumLength(2, exception: exception))
                    .AddMessageTest<StringException>(message => "Bar".MustHaveMaximumLength(1, message: message));
        }
    }
}