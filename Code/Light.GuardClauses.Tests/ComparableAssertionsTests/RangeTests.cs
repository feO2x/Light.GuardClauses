using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class RangeTests
    {
        [Theory(DisplayName = "IsValueWithinRange must return true when the given value is between the specified range.")]
        [InlineData(0, 5, 0, true, true)]
        [InlineData(0, 5, 5, false, true)]
        [InlineData(0, 5, 4, false, false)]
        [InlineData(0, 5, 1, false, false)]
        [InlineData(-4, 4, 0, false, false)]
        [InlineData(42, 80, 42, true, false)]
        [InlineData(42L, 43L, 43L, true, true)]
        public void ValueInRange<T>(T from, T to, T value, bool isFromInclusive, bool isToInclusive) where T : IComparable<T>
        {
            var testTarget = new Range<T>(from, to, isFromInclusive, isToInclusive);

            var result = testTarget.IsValueWithinRange(value);

            result.Should().BeTrue();
        }

        [Theory(DisplayName = "IsValueWithinRange must return false when the given value is out of the specified range.")]
        [InlineData(0, 5, -1, true, true)]
        [InlineData(0, 5, 6, false, true)]
        [InlineData(0, 5, 5, false, false)]
        [InlineData(0, 5, 0, false, false)]
        [InlineData(-4, 4, -80, false, false)]
        [InlineData(42, 80, 42, false, false)]
        [InlineData(42L, 43L, 43L, false, false)]
        public void ValueOutOfRange<T>(T from, T to, T value, bool isFromInclusive, bool isToInclusive) where T : IComparable<T>
        {
            var testTarget = new Range<T>(from, to, isFromInclusive, isToInclusive);

            var result = testTarget.IsValueWithinRange(value);

            result.Should().BeFalse();
        }

        [Theory(DisplayName = "Constructor must throw exception when to is smaller than from.")]
        [InlineData(1, 0)]
        [InlineData(42, -1)]
        [InlineData(-87, -88)]
        public void ConstructorException(int from, int to)
        {
            Action act = () => Range<int>.FromInclusive(from).ToExclusive(to);

            act.ShouldThrow<ArgumentOutOfRangeException>().And
               .Message.Should().Contain($"{nameof(to)} must not be less than {from}, but you specified {to}.");
        }
    }
}
