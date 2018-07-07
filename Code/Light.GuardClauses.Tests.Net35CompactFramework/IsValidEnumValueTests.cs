using System;
using System.Globalization;
using System.Reflection;
using NUnit.Framework;

namespace Light.GuardClauses.Tests.Net35CompactFramework
{
    [TestFixture]
    public class IsValidEnumValueTests
    {
        [Test]
        public void ValidEnumValue()
        {
            CheckValidEnumValue(UriKind.Absolute);
            CheckValidEnumValue(DateTimeKind.Utc);
            CheckValidEnumValue(BindingFlags.Static | BindingFlags.Public);
            CheckValidEnumValue(BindingFlags.SetProperty | BindingFlags.SetField);
            CheckValidEnumValue(NumberStyles.Number | NumberStyles.AllowExponent);
            CheckValidEnumValue(UInt64Enum.Low1 | UInt64Enum.Low3);
            CheckValidEnumValue(UInt64Enum.High2 | UInt64Enum.High4);
            CheckValidEnumValue(UInt64Enum.AllLow | UInt64Enum.High1);
            CheckValidEnumValue(UInt64Enum.AllHigh);
            CheckValidEnumValue(UInt64Enum.MaxValue);

            void CheckValidEnumValue<T>(T enumValue) where T : struct, IConvertible, IComparable, IFormattable => 
                Assert.IsTrue(enumValue.IsValidEnumValue());
        }

        [Test]
        public void InvalidEnumValue()
        {
            CheckInvalidEnumValue((DateTimeKind) 2000);
            CheckInvalidEnumValue((UriKind) (-4));
        }

        [Test]
        public void InvalidFlagsEnumValue()
        {
            CheckInvalidEnumValue((NumberStyles) (-1));
            CheckInvalidEnumValue((BindingFlags) (1 << 22));
        }

        private static void CheckInvalidEnumValue<T>(T invalidEnumValue) where T : struct, IConvertible, IComparable, IFormattable =>
            Assert.IsFalse(invalidEnumValue.IsValidEnumValue());

        [Flags]
        public enum UInt64Enum : ulong
        {
            Low1 = 1 << 0,
            Low2 = 1 << 1,
            Low3 = 1 << 2,
            Low4 = 1 << 3,
            AllLow = Low1 | Low2 | Low3 | Low4,
            High1 = 1 << 59,
            High2 = 1 << 60,
            High3 = 1 << 61,
            High4 = 1 << 62,
            AllHigh = High1 | High2 | High3 | High4,
            MaxValue = ulong.MaxValue
        }

        public enum EmptyEnum { }

        [Flags]
        public enum EmptyFlagsEnum { }
    }
}
