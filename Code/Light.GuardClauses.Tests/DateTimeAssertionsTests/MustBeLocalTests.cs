using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DateTimeAssertionsTests
{
    public sealed class MustBeLocalTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeLocal must throw an exception when the specified date time's kind is DateTimeKind.Utc or DateTimeKind.Unspecified.")]
        public void NotLocal()
        {
            var invalidDateTime = DateTime.UtcNow;

            Action act = () => invalidDateTime.MustBeLocal(nameof(invalidDateTime));

            act.Should().Throw<InvalidDateTimeException>()
               .And.Message.Should().Contain($"The specified date time \"{invalidDateTime:O}\" must be of kind {DateTimeKind.Local}, but actually is {invalidDateTime.Kind}.");
        }

        [Fact(DisplayName = "MustBeLocal must not throw an exception when the specified date time's kind is DateTimeKind.Local.")]
        public void Local()
        {
            var dateTime = DateTime.Now;

            var result = dateTime.MustBeLocal();

            result.Should().Be(dateTime);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.UtcNow.MustBeLocal(exception: exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeLocal(message: message)));
        }
    }
}