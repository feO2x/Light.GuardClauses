using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class EqualsTests
    {
        [Theory]
        [InlineData("Foo", "   Foo", true)]
        [InlineData("Bar", "Bar  ", true)]
        [InlineData("\t Baz\r\n", "Baz", true)]
        [InlineData("Qux", "Qux", true)]
        [InlineData("{\r\n  \"foo\": 42\r\n}\r\n", "{\"foo\":42}", true)]
        [InlineData("Foo", "Bar", false)]
        [InlineData("  Baz", "Qux  ", false)]
        [InlineData("Foo", "foo", false)]
        [InlineData("Bar\t", "bar ", false)]
        [InlineData(null, null, true)]
        [InlineData(null, "Foo ", false)]
        [InlineData("Foo", null, false)]
        [InlineData("", "", true)]
        public static void OrdinalIgnoreWhiteSpace(string x, string y, bool expected) =>
            x.Equals(y, StringComparisonType.OrdinalIgnoreWhiteSpace).Should().Be(expected);

        [Theory]
        [InlineData("Foo", "FOO", true)]
        [InlineData(" Bar", "bar ", true)]
        [InlineData("B a z", "bAZ", true)]
        [InlineData("Qu\tx", "  qux", true)]
        [InlineData("{\r\n  \"foo\": \"Bar\"\r\n}", "{\"FOO\": \"BAR\"}", true)]
        [InlineData("Foo", "Bar", false)]
        [InlineData("", "some content", false)]
        [InlineData("", "", true)]
        [InlineData(null, null, true)]
        [InlineData(null, "Foo", false)]
        [InlineData("Bar", null, false)]
        public static void OrdinalIgnoreCaseIgnoreWhiteSpace(string x, string y, bool expected) => 
            x.Equals(y, StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace).Should().Be(expected);

        [Theory]
        [InlineData("Foo", "Foo", true)]
        [InlineData("Bar", "bar", false)]
        public static void FallbackToOrdinal(string x, string y, bool expected) => 
            x.Equals(y, StringComparisonType.Ordinal).Should().Be(expected);

        [Theory]
        [InlineData("Foo", "foo", true)]
        [InlineData("Foo", "Bar", false)]
        public static void FallbackToOrdinalIgnoreCase(string x, string y, bool expected) => 
            x.Equals(y, StringComparisonType.OrdinalIgnoreCase).Should().Be(expected);

        [Theory]
        [InlineData("Foo", "Foo", true)]
        [InlineData("Foo", "foo", false)]
        public static void FallbackToCurrentCulture(string x, string y, bool expected) =>
            x.Equals(y, StringComparisonType.CurrentCulture).Should().Be(expected);

        [Theory]
        [InlineData("Foo", "foo", true)]
        [InlineData("Foo", "Bar", false)]
        public static void FallbackToCurrentCultureIgnoreCase(string x, string y, bool expected) =>
            x.Equals(y, StringComparisonType.CurrentCultureIgnoreCase).Should().Be(expected);

#if !NETCOREAPP1_1
        [Theory]
        [InlineData("Foo", "Foo", true)]
        [InlineData("Foo", "foo", false)]
        public static void FallbackToInvariantCulture(string x, string y, bool expected) =>
            x.Equals(y, StringComparisonType.InvariantCulture).Should().Be(expected);

        [Theory]
        [InlineData("Foo", "foo", true)]
        [InlineData("Foo", "Bar", false)]
        public static void FallbackToInvariantCultureIgnoreCase(string x, string y, bool expected) =>
            x.Equals(y, StringComparisonType.InvariantCultureIgnoreCase).Should().Be(expected);
#endif
    }
}