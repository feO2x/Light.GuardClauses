using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DateTimeAssertionsTests
{
    public sealed class MustBeUtcTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeUtc must throw an exception when the specified DateTime.Kind is not DateTimeKind.UTC")]
        public void NotUtc()
        {
            var invalidDateTime = DateTime.Now;

            Action act = () => invalidDateTime.MustBeUtc(nameof(invalidDateTime));

            act.Should().Throw<InvalidDateTimeException>()
               .And.Message.Should().Contain($"The specified date time \"{invalidDateTime:O}\" must be of kind {DateTimeKind.Utc}, but actually is {invalidDateTime.Kind}.");
        }

        [Fact(DisplayName = "MustBeUtc must not throw an exception when the specified DateTime is of kind UTC.")]
        public void Utc()
        {
            var utcDateTime = DateTime.UtcNow;

            var result = utcDateTime.MustBeUtc();

            result.Should().Be(utcDateTime);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.Now.MustBeUtc(exception: exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.Now.MustBeUtc(message: message)));
        }
    }
}