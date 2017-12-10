using System;
using System.Globalization;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    public static class IsValidEnumValueTests
    {
        [Theory(DisplayName = "IsValidEnumValue must return true when the enum value is valid.")]
        [InlineData(ConsoleColor.Black)]
        [InlineData(UriKind.Absolute)]
        [InlineData(DateTimeKind.Utc)]
        [InlineData(BindingFlags.Static | BindingFlags.Public)]
        [InlineData(BindingFlags.SetProperty | BindingFlags.SetField)]
        [InlineData(NumberStyles.Number | NumberStyles.AllowExponent)]
        [InlineData(UInt64Enum.Low1 | UInt64Enum.Low3)]
        [InlineData(UInt64Enum.High2 | UInt64Enum.High4)]
        [InlineData(UInt64Enum.AllLow | UInt64Enum.High1)]
        [InlineData(UInt64Enum.MaxValue)]
        public static void EnumValueValid<T>(T enumValue)
        {
            var result = enumValue.IsValidEnumValue();

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsValidEnumValue must return false when the enum value is invalid.")]
        [InlineData(2000)]
        [InlineData(-5)]
        public static void EnumValueInvalid(int invalidDateTimeKindValue)
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
        public static void InvalidNumberStyles(int invalidValue)
        {
            ((NumberStyles) invalidValue).IsValidEnumValue().Should().BeFalse();
        }

        [Flags]
        public enum UInt64Enum : ulong
        {
            Low1= 1 << 0,
            Low2 = 1 << 1,
            Low3= 1 << 2,
            Low4 = 1 << 3,
            AllLow = Low1 | Low2 | Low3 | Low4,
            High1 = 1 << 59,
            High2 = 1 << 60,
            High3 = 1 << 61,
            High4 = 1 << 62,
            AllHigh = High1 | High2 | High3 | High4,
            MaxValue = ulong.MaxValue
        }

        [Fact(DisplayName = "IsValidEnumValue must return false when an invalid value of an empty enum is passed in.")]
        public static void EmptyEnumValue()
        {
            ((EmptyEnum) 42).IsValidEnumValue().Should().BeFalse();
        }

        public enum EmptyEnum { }

        [Fact(DisplayName = "IsValidEnumValue must return false when an invalid value of an empty flags enum is passed in.")]
        public static void EmptyFlagsEnumValue()
        {
            ((EmptyFlagsEnum) 186).IsValidEnumValue().Should().BeFalse();
        }

        [Flags]
        public enum EmptyFlagsEnum { }
    }
}