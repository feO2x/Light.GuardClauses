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
        public static void NotUtc()
        {
            var invalidDateTime = DateTime.Now;

            Action act = () => invalidDateTime.MustBeUtc(nameof(invalidDateTime));

            act.Should().Throw<InvalidDateTimeException>()
               .And.Message.Should().Contain($"{nameof(invalidDateTime)} \"{invalidDateTime:O}\" must use kind {DateTimeKind.Utc}, but it actually uses {invalidDateTime.Kind}.");
        }

        [Fact(DisplayName = "MustBeUtc must not throw an exception when the specified DateTime is of kind UTC.")]
        public static void Utc()
        {
            var utcDateTime = DateTime.UtcNow;

            var result = utcDateTime.MustBeUtc();

            result.Should().Be(utcDateTime);
        }

        [Fact(DisplayName = "MustBeUtc must throw the custom exception with single parameter when the specified date time does not use UTC.")]
        public static void CustomExceptionWithDateTime()
        {
            var value = new DateTime(2018, 2, 18, 16, 51, 00, DateTimeKind.Local);
            var exception = new Exception();
            var recordedValue = default(DateTime);

            Action act = () => value.MustBeUtc(dt =>
            {
                recordedValue = dt;
                return exception;
            });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            recordedValue.Should().Be(value);
        }

        [Fact(DisplayName = "MustBeUtc must not throw a custom exception with a single parameter when the specified date time uses UTC.")]
        public static void NoCustomException()
        {
            var value = new DateTime(2016, 5, 31, 10, 0, 0, DateTimeKind.Utc);

            var result = value.MustBeUtc(dt => null);

            result.Should().Be(value);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.Now.MustBeUtc(exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.Now.MustBeUtc(message: message)));
        }
    }
}