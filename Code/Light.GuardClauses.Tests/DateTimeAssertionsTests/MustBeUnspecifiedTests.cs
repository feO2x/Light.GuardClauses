using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DateTimeAssertionsTests
{
    public sealed class MustBeUnspecifiedTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeUnspecified must throw an exception when the specified date time's kind is DateTimeKind.Utc or DateTimeKind.Local.")]
        public void NotUnspecified()
        {
            var invalidDateTime = DateTime.UtcNow;

            Action act = () => invalidDateTime.MustBeUnspecified(nameof(invalidDateTime));

            act.ShouldThrow<InvalidDateTimeException>()
               .And.Message.Should().Contain($"The specified date time \"{invalidDateTime:O}\" must be of kind {DateTimeKind.Unspecified}, but actually is {invalidDateTime.Kind}.");
        }

        [Fact(DisplayName = "MustBeUnspecified must not throw an exception when the specified date time's kind is DateTimeKind.Unspecified.")]
        public void Unspecified()
        {
            var dateTime = DateTime.MinValue;

            var result = dateTime.MustBeUnspecified();

            result.Should().Be(dateTime);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.UtcNow.MustBeUnspecified(exception: exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeUnspecified(message: message)));
        }
    }
}