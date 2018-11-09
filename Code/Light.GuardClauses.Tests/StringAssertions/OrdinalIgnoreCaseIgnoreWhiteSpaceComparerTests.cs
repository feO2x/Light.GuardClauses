using System;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public sealed class OrdinalIgnoreCaseIgnoreWhiteSpaceComparerTests
    {
        private static readonly OrdinalIgnoreCaseIgnoreWhiteSpaceComparer Comparer = new OrdinalIgnoreCaseIgnoreWhiteSpaceComparer();
        private readonly ITestOutputHelper _output;

        public OrdinalIgnoreCaseIgnoreWhiteSpaceComparerTests(ITestOutputHelper output) => _output = output.MustNotBeNull(nameof(output));

        [Theory]
        [InlineData("Foo", "FOO", true)]
        [InlineData(" Bar", "bar ", true)]
        [InlineData("B a z", "bAZ", true)]
        [InlineData("Qu\tx", "  qux", true)]
        [InlineData("{\r\n  \"foo\": \"Bar\"\r\n}", "{\"FOO\": \"BAR\"}", true)]
        [InlineData("Foo", "Bar", false)]
        [InlineData("", "some content", false)]
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
        [InlineData("Foo", "foo")]
        [InlineData("B a\tr", "Bar")]
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