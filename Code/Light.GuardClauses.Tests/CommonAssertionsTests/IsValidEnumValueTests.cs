using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsValidEnumValueTests
    {
        [Theory(DisplayName = "IsValidEnumValue must return true when the enum value is valid.")]
        [InlineData(ConsoleColor.Black)]
        [InlineData(UriKind.Absolute)]
        [InlineData(DateTimeKind.Utc)]
        public void EnumValueValid<T>(T enumValue)
        {
            var result = enumValue.IsValidEnumValue();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsValidEnumValue must return false when the enum value is invalid.")]
        [InlineData(2000)]
        [InlineData(-5)]
        public void EnumValueInvalid(int invalidDateTimeKindValue)
        {
            var result = ((DateTimeKind) invalidDateTimeKindValue).IsValidEnumValue();

            result.Should().BeFalse();
        }
    }
}