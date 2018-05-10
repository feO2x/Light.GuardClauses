using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustHaveValueTests
    {
        [Fact]
        public static void HasNoValue()
        {
            DateTime? nullable = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => nullable.MustHaveValue(nameof(nullable));

            act.Should().Throw<NullableHasNoValueException>()
               .And.Message.Should().Contain($"{nameof(nullable)} must have a value, but it actually is null.");
        }

        [Theory]
        [InlineData(42)]
        [InlineData(20)]
        [InlineData(-187)]
        public static void HasValue(int? value) => value.MustHaveValue(nameof(value)).Should().Be(value);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(exceptionFactory => new double?().MustHaveValue(exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<NullableHasNoValueException>(message => new short?().MustHaveValue(message: message));
    }
}