using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveValueTests
    {
        [Fact(DisplayName = "MustHaveValue must throw an exception when the specified Nullable<T> has no value.")]
        public void HasNoValue()
        {
            DateTime? value = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustHaveValue(nameof(value));

            act.ShouldThrow<NullableHasNoValueException>()
               .And.ParamName.Should().Be(nameof(value));
        }

        [Fact(DisplayName = "MustHaveValue must not throw an exception when the specified Nullable<T> has a value.")]
        public void HasValue()
        {
            int? value = 42;

            Action act = () => value.MustHaveValue(nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustHaveValue must inject instead of the default one.")]
        public void CustomMessage()
        {
            double? value = null;
            const string message = "Thou shall have a value!";

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustHaveValue(message: message);

            act.ShouldThrow<NullableHasNoValueException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustHaveValue must raise instead of the default one.")]
        public void CustomException()
        {
            double? value = null;
            var exception = new Exception();

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => value.MustHaveValue(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}