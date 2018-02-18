using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class MustBeValidEnumValueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustBeValidEnumValue must throw an exception when the specified value is not within the defined values of the enum.")]
        public static void InvalidEnumValue()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey) 15;

            Action act = () => invalidValue.MustBeValidEnumValue(nameof(invalidValue));

            act.Should().Throw<EnumValueNotDefinedException>()
               .And.ParamName.Should().Be(nameof(invalidValue));
        }

        [Fact(DisplayName = "MustBeValidEnumValue must not throw an exception when the specified value is within the defined values of the enum.")]
        public static void ValidEnumValue()
        {
            const ConsoleColor validValue = ConsoleColor.DarkRed;

            var result = validValue.MustBeValidEnumValue(nameof(validValue));

            result.Should().Be(validValue);
        }

        [Fact(DisplayName = "MustBeValidEnumValue must throw the custom parameterized exception when the specified value is not within the defined values of the enumeration.")]
        public static void InvalidEnumValueWithCustomParameterizedException()
        {
            const ConsoleSpecialKey invalidValue = (ConsoleSpecialKey)15;
            var observedValue = default(ConsoleSpecialKey);
            var exception = new Exception();

            Action act = () => invalidValue.MustBeValidEnumValue(v =>
                                                                 {
                                                                     observedValue = v;
                                                                     return exception;
                                                                 });

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
            observedValue.Should().Be(invalidValue);
        }

        [Fact(DisplayName = "MustBeValidEnumValue must not throw the custom parameterized exception when the specified value is defined in the target enum.")]
        public static void ValidEnumValueWithCustomParameterizedException()
        {
            const ConsoleKey enumValue = ConsoleKey.B;

            var result = enumValue.MustBeValidEnumValue(v => new Exception());

            result.Should().Be(enumValue);
        }

        [Fact(DisplayName = "MustBeValidEnumValue must throw an ArgumentException when the type of the specified value is no enum.")]
        public void NotEnum()
        {
            Action act = () => 42.MustBeValidEnumValue();

            act.Should().Throw<ArgumentException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => ((ConsoleSpecialKey) 15).MustBeValidEnumValue(exception)))
                    .Add(new CustomMessageTest<EnumValueNotDefinedException>(message => ((ConsoleSpecialKey) 15).MustBeValidEnumValue(message: message)));
        }
    }
}