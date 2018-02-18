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
               .And.Message.Should().Contain($"{nameof(invalidDateTime)} \"{invalidDateTime:O}\" must use kind {DateTimeKind.Local}, but it actually uses {invalidDateTime.Kind}.");
        }

        [Fact(DisplayName = "MustBeLocal must not throw an exception when the specified date time's kind is DateTimeKind.Local.")]
        public void Local()
        {
            var dateTime = DateTime.Now;

            var result = dateTime.MustBeLocal();

            result.Should().Be(dateTime);
        }

        [Fact(DisplayName = "MustBeLocal must throw the custom exception with single parameter when the specified date time does not use DateTimeKind.Local.")]
        public static void CustomExceptionWithDateTime()
        {
            var value = new DateTime(2018, 2, 18, 16, 57, 00, DateTimeKind.Utc);
            var exception = new Exception();
            var recordedValue = default(DateTime);

            Action act = () => value.MustBeLocal(dt =>
            {
                recordedValue = dt;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(value);
        }

        [Fact(DisplayName = "MustBeLocal must not throw a custom exception with a single parameter when the specified date time uses DateTimeKind.Local.")]
        public static void NoCustomException()
        {
            var value = new DateTime(1989, 2, 5, 3, 32, 21, DateTimeKind.Local);

            var result = value.MustBeLocal(dt => null);

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.UtcNow.MustBeLocal(exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeLocal(message: message)));
        }
    }
}