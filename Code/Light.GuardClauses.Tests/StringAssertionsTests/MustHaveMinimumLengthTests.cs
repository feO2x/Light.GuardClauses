using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveMinimumLengthTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveMinimumLength must throw a StringException when the specified string is shorter than the minimum length.")]
        [InlineData("Foo", 4)]
        [InlineData("I'm all for cheating. This is war. But to slaughter them at a wedding...", 100)]
        public void Shorter(string @string, int minimumLength)
        {
            Action act = () => @string.MustHaveMinimumLength(minimumLength, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} \"{@string}\" must have a minimum length of {minimumLength}, but it actually is only {@string.Length} characters long.");
        }

        [Theory(DisplayName = "MustHaveMinimumLength must not throw an exception when the specified string has at least the same length as minimumLength.")]
        [InlineData("Foo", 3)]
        [InlineData("Bar", 0)]
        [InlineData("It's not easy being drunk all the time. Everyone would do it if it were easy.", 50)]
        public void SameLengthOrLonger(string @string, int minimumLength)
        {
            var result = @string.MustHaveMinimumLength(minimumLength);

            result.Should().BeSameAs(@string);
        }

        [Theory(DisplayName = "MustHaveMinimumLenght must throw an ArgumentOutOfRangeException when the specified minimum length is negative.")]
        [InlineData(-3)]
        [InlineData(-1)]
        [InlineData(-1554)]
        public void MinimumLengthNegative(int minimumLength)
        {
            Action act = () => "Foo".MustHaveMinimumLength(minimumLength);

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be(nameof(minimumLength));
        }

        [Fact(DisplayName = "MustHaveMinimumLength must throw an ArgumentNullException when the specified string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).MustHaveMinimumLength(42);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "Foo".MustHaveMinimumLength(4, exception: exception))
                    .AddMessageTest<StringException>(message => "Bar".MustHaveMinimumLength(42, message: message));
        }
    }
}