using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.FrameworkExtensions
{
    public static class ToStringRepresentationTests
    {
        [Theory]
        [InlineData(42)]
        [InlineData(30)]
        [InlineData(-1502)]
        public static void UnquotedValue(int value) => value.ToStringRepresentation().Should().Be(value.ToString());

        [Theory]
        [DefaultVariablesData]
        public static void QuotedValues(string value) => value.ToStringRepresentation().Should().Be($"\"{value}\"");
    }
}