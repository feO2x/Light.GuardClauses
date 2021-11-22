using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.DateTimeAssertions
{
    public static class MustBeLocalTests
    {
        [Fact]
        public static void NotLocal()
        {
            var invalidDateTime = DateTime.UtcNow;

            Action act = () => invalidDateTime.MustBeLocal(nameof(invalidDateTime));

            act.Should().Throw<InvalidDateTimeException>()
               .And.Message.Should().Contain($"{nameof(invalidDateTime)} must use kind \"{DateTimeKind.Local}\", but it actually uses \"{invalidDateTime.Kind}\" and is \"{invalidDateTime:O}\".");
        }

        [Fact]
        public static void Local()
        {
            var dateTime = DateTime.Now;

            dateTime.MustBeLocal().Should().Be(dateTime);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new DateTime(2018, 2, 18, 16, 57, 00, DateTimeKind.Utc),
                                 (value, exceptionFactory) => value.MustBeLocal(exceptionFactory));

        [Fact]
        public static void NoCustomException()
        {
            var value = new DateTime(1989, 2, 5, 3, 32, 21, DateTimeKind.Local);

            var result = value.MustBeLocal(_ => null);

            result.Should().Be(value);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeLocal(message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var invalidDateTime = DateTime.UtcNow;

            Action act = () => invalidDateTime.MustBeLocal();

            act.Should().Throw<InvalidDateTimeException>()
               .And.ParamName.Should().Be(nameof(invalidDateTime));
        }
    }
}