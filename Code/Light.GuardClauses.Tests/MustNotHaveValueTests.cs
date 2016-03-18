using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotHaveValueTests
    {
        [Fact(DisplayName = "MustNotHaveValue must throw an exception when the specified Nullable<T> has a value.")]
        public void HasValue()
        {
            DateTime? value = DateTime.Today;

            Action act = () => value.MustNotHaveValue(nameof(value));

            act.ShouldThrow<NullableHasValueException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustNotHaveValue must not throw an exception when the specified Nullable<T> is null.")]
        public void HasNoValue()
        {
            double? value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustNotHaveValue(nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotHaveValue must inject instead of the default one.")]
        public void CustomMessage()
        {
            double? value = 42.0;
            const string message = "Thou shall not have a value!";

            Action act = () => value.MustNotHaveValue(message: message);

            act.ShouldThrow<NullableHasValueException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotHaveValue must raise instead of the default one.")]
        public void CustomException()
        {
            int? value = 42;
            var exception = new Exception();

            Action act = () => value.MustNotHaveValue(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}