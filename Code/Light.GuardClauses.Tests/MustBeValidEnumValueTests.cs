using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeValidEnumValueTests : ICustomMessageAndExceptionTestDataProvider
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

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => ((ConsoleSpecialKey) 15).MustBeValidEnumValue(exception: exception)));

            testData.Add(new CustomMessageTest<EnumValueNotDefinedException>(message => ((ConsoleSpecialKey) 15).MustBeValidEnumValue(message: message)));
        }
    }
}