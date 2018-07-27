using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
#if NETCOREAPP1_1
using System.Reflection;
#endif

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustBeValidEnumValueTests
    {
        [Fact]
        public static void InvalidEnumValue()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;

            Action act = () => invalidValue.MustBeValidEnumValue(nameof(invalidValue));

            var exceptionAssertion = act.Should().Throw<EnumValueNotDefinedException>().Which;
            exceptionAssertion.Message.Should().Contain($"{nameof(invalidValue)} \"{invalidValue}\" must be one of the defined constants of enum \"{invalidValue.GetType()}\", but it actually is not.");
            exceptionAssertion.ParamName.Should().Be(nameof(invalidValue));
        }

        [Fact]
        public static void ValidEnumValue() => ConsoleColor.DarkRed.MustBeValidEnumValue().Should().Be(ConsoleColor.DarkRed);

        [Fact]
        public static void CustomException() =>
            Test.CustomException((ConsoleSpecialKey) 42,
                                 (invalidValue, exceptionFactory) => invalidValue.MustBeValidEnumValue(exceptionFactory));

        [Fact]
        public static void CustomExceptionNoEnumType() =>
            Test.CustomException(100L,
                                 (longValue, exceptionFactory) => longValue.MustBeValidEnumValue(exceptionFactory));

        [Fact]
        public static void CustomExceptionValidEnumValue() => 
            SampleEnum.Second.MustBeValidEnumValue(_ => null).Should().Be(SampleEnum.Second);


        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<EnumValueNotDefinedException>(
                message => ((DateTimeKind) 7).MustBeValidEnumValue(message: message));

        [Fact]
        public static void CustomMessageNoEnumValue() => 
            Test.CustomMessage<TypeIsNoEnumException>(
                message => 42.MustBeValidEnumValue(message: message));

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