using System;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public sealed class OrdinalIgnoreWhiteSpaceComparerTests
    {
        private static readonly OrdinalIgnoreWhiteSpaceComparer Comparer = new OrdinalIgnoreWhiteSpaceComparer();
        private readonly ITestOutputHelper _output;

        public OrdinalIgnoreWhiteSpaceComparerTests(ITestOutputHelper output) => _output = output.MustNotBeNull(nameof(output));

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
        [InlineData("", "", true)]
        public static void CheckEquals(string x, string y, bool expected) =>
            Comparer.Equals(x, y).Should().Be(expected);

        [Theory]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void EqualsStringNull(string x, string y)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => Comparer.Equals(x, y);

            act.Should().Throw<ArgumentNullException>().WriteExceptionTo(_output);
        }

        [Theory]
        [InlineData("Foo", "Foo")]
        [InlineData("  Bar", "Bar  ")]
        [InlineData("{\r\n\t\"foo\": 42\r\n}\r\n", "{\"foo\": 42}")]
        public static void CheckGetHashCode(string x, string y) => 
            Comparer.GetHashCode(x).Should().Be(Comparer.GetHashCode(y));

        [Fact]
        public void GetHashCodeStringNull()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => Comparer.GetHashCode(null);

            act.Should().Throw<ArgumentNullException>().WriteExceptionTo(_output);
        }
    }
}