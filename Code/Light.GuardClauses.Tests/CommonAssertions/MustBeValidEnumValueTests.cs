using System;
#if NETCOREAPP1_1
using System.Reflection;
#endif
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustBeValidEnumValueTests
    {
        [Fact]
        public static void InvalidEnumValue()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;

            Action act = () => invalidValue.MustBeValidEnumValue(nameof(invalidValue));

            act.Should().Throw<EnumValueNotDefinedException>()
               .And.Message.Should().Contain($"{nameof(invalidValue)} \"{invalidValue}\" must be one of the defined constants of enum \"{invalidValue.GetType()}\", but it actually is not.");
        }

        [Fact]
        public static void ValidEnumValue() => ConsoleColor.DarkRed.MustBeValidEnumValue().Should().Be(ConsoleColor.DarkRed);

        [Fact]
        public static void CustomException() =>
            CustomExceptions.TestCustomException((ConsoleSpecialKey) 42,
                                                 (invalidValue, exceptionFactory) => invalidValue.MustBeValidEnumValue(exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            CustomMessages.TestCustomMessage<EnumValueNotDefinedException>(
                message => ((DateTimeKind) 7).MustBeValidEnumValue(message: message));

        [Fact]
        public static void NoEnumType()
        {
            Action act = () => 42.0.MustBeValidEnumValue();

            act.Should().Throw<TypeIsNoEnumException>()
               .And.Message.Should().Contain($"The type \"{typeof(double)}\" must be an enum type, but it actually is not.");
        }

        [Fact]
        public static void GetAllEnumValues()
        {
#if !NETCOREAPP1_1
            var enumFields = typeof(SampleEnum).GetFields();
#else
            var enumFields = typeof(SampleEnum).GetTypeInfo().GetFields();
#endif
            var actualValues = new SampleEnum[enumFields.Length - 1];
            for (var i = 1; i < enumFields.Length; ++i)
            {
                actualValues[i - 1] = (SampleEnum) enumFields[i].GetValue(null);
            }

            var expectedValues = (SampleEnum[]) Enum.GetValues(typeof(SampleEnum));

            actualValues.Should().Equal(expectedValues);
        }
    }

    public enum SampleEnum
    {
        First,
        Second,
        Third = 42
    }
}