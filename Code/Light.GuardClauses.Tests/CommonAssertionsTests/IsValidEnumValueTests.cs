using System;
using System.Globalization;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public sealed class IsValidEnumValueTests
    {
        [Theory(DisplayName = "IsValidEnumValue must return true when the enum value is valid.")]
        [InlineData(ConsoleColor.Black)]
        [InlineData(UriKind.Absolute)]
        [InlineData(DateTimeKind.Utc)]
        [InlineData(BindingFlags.Static | BindingFlags.Public)]
        [InlineData(BindingFlags.SetProperty | BindingFlags.SetField)]
        [InlineData(NumberStyles.Number | NumberStyles.AllowExponent)]
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

        [Theory(DisplayName = "IsValidEnumValue must return false when an invalid value is specified for the NumberStyles enum.")]
        [InlineData(-1)]
        [InlineData(-2)]
        [InlineData(-512)]
        [InlineData(int.MinValue)]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(int.MaxValue)]
        public void InvalidNumberStyles(int invalidValue)
        {
            ((NumberStyles) invalidValue).IsValidEnumValue().Should().BeFalse();
        }
    }
}