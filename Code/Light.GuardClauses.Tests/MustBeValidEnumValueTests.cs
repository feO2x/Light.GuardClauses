using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeValidEnumValueTests
    {
        [Fact(DisplayName = "MustBeValidEnumValue must throw an exception when the specified value is not within the defined values of the enumeration.")]
        public void InvalidEnumValue()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;

            Action act = () => invalidValue.MustBeValidEnumValue(nameof(invalidValue));

            act.ShouldThrow<EnumValueNotDefinedException>()
               .And.ParamName.Should().Be(nameof(invalidValue));
        }

        [Fact(DisplayName = "MustBeValidEnumValue must not throw an exception when the specified value is within the defined values of the enumeration.")]
        public void ValidEnumValue()
        {
            const ConsoleColor validValue = ConsoleColor.DarkRed;

            Action act = () => validValue.MustBeValidEnumValue(nameof(validValue));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustBeValidEnumValue must inject instead of the default one.")]
        public void CustomMessage()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;
            const string message = "Though shall be a defined enum value!";

            Action act = () => invalidValue.MustBeValidEnumValue(message: message);

            act.ShouldThrow<EnumValueNotDefinedException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustBeValidEnumValue must raise instead of the default one.")]
        public void CustomException()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;
            var exception = new Exception();

            Action act = () => invalidValue.MustBeValidEnumValue(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}