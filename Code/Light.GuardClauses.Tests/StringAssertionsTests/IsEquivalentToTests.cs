using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class IsEquivalentToTests
    {
        [Theory(DisplayName = "IsEquivalentTo must return true when the two strings are null or equal according to StringComparison.OrdinalIgnoreCase.")]
        [InlineData("foo", "foo", true)]
        [InlineData("foo", "Foo", true)]
        [InlineData("foo", "bar", false)]
        [InlineData("foo", null, false)]
        [InlineData(null, "foo", false)]
        [InlineData(null, null, true)]
        [InlineData("", "", true)]
        public void Equivalency(string first, string second, bool expected)
        {
            first.IsEquivalentTo(second).Should().Be(expected);
        }

        [Theory(DisplayName = "IsEquivalentTo must apply the passed in comparison type when it is specified by the caller.")]
        [InlineData("foo", "Foo", StringComparison.Ordinal, false)]
        [InlineData("Day", "day", StringComparison.CurrentCulture, false)]
        public void CustomComparisonType(string first, string second, StringComparison comparisonType, bool expected)
        {
            first.IsEquivalentTo(second, comparisonType).Should().Be(expected);
        }

        [Fact(DisplayName = "IsEquivalentTo must throw an EnumValueNotDefinedException when an invalid comparison type is passed in.")]
        public void InvalidComparisonType()
        {
            const StringComparison comparisonType = (StringComparison) 42;

            Action act = () => "foo".IsEquivalentTo("bar", comparisonType);

            act.ShouldThrow<EnumValueNotDefinedException>()
               .And.ParamName.Should().Be(nameof(comparisonType));
        }
    }
}