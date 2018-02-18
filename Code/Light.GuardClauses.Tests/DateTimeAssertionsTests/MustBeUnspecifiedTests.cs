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

            act.Should().Throw<InvalidDateTimeException>()
               .And.Message.Should().Contain($"{nameof(invalidDateTime)} \"{invalidDateTime:O}\" must use kind {DateTimeKind.Unspecified}, but it actually uses {invalidDateTime.Kind}.");
        }

        [Fact(DisplayName = "MustBeUnspecified must not throw an exception when the specified date time's kind is DateTimeKind.Unspecified.")]
        public void Unspecified()
        {
            var dateTime = DateTime.MinValue;

            var result = dateTime.MustBeUnspecified();

            result.Should().Be(dateTime);
        }

        [Fact(DisplayName = "MustBeUnspecified must throw the custom exception with single parameter when the specified date time does not use DateTimeKind.Unspecified.")]
        public static void CustomExceptionWithDateTime()
        {
            var value = new DateTime(2018, 2, 18, 16, 57, 00, DateTimeKind.Utc);
            var exception = new Exception();
            var recordedValue = default(DateTime);

            Action act = () => value.MustBeUnspecified(dt =>
            {
                recordedValue = dt;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(value);
        }

        [Fact(DisplayName = "MustBeUnspecified must not throw a custom exception with a single parameter when the specified date time uses DateTimeKind.Unspecified.")]
        public static void NoCustomException()
        {
            var value = new DateTime(2000, 9, 10, 15, 32, 47, DateTimeKind.Unspecified);

            var result = value.MustBeUnspecified(dt => null);

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.UtcNow.MustBeUnspecified(exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeUnspecified(message: message)));
        }
    }
}