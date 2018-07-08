using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DateTimeAssertions
{
    public static class MustBeUtcTests
    {
        [Fact]
        public static void NotUtc()
        {
            var invalidDateTime = DateTime.Now;

            Action act = () => invalidDateTime.MustBeUtc(nameof(invalidDateTime));

            act.Should().Throw<InvalidDateTimeException>()
               .And.Message.Should().Contain($"{nameof(invalidDateTime)} must use kind \"{DateTimeKind.Utc}\", but it actually uses \"{invalidDateTime.Kind}\" and is \"{invalidDateTime:O}\".");
        }

        [Fact]
        public static void Utc()
        {
            var utcDateTime = DateTime.UtcNow;

            var result = utcDateTime.MustBeUtc();

            result.Should().Be(utcDateTime);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new DateTime(2018, 2, 18, 16, 51, 00, DateTimeKind.Local),
                                 (value, exceptionFactory) => value.MustBeUtc(exceptionFactory));

        [Fact]
        public static void NoCustomException()
        {
            var value = new DateTime(2016, 5, 31, 10, 0, 0, DateTimeKind.Utc);

            var result = value.MustBeUtc(dt => null);

            result.Should().Be(value);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidDateTimeException>(message => DateTime.Now.MustBeUtc(message:message));
    }
}