using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertionsTests
{
    public sealed class RangeTests
    {
        [Theory(DisplayName = "IsValueWithinRange must return true when the given value is between the specified range.")]
        [InlineData(0, 5, 0, true, true)]
        [InlineData(0, 5, 5, false, true)]
        [InlineData(0, 5, 4, false, false)]
        [InlineData(0, 5, 1, false, false)]
        [InlineData(-4, 4, 0, false, false)]
        [InlineData(42, 80, 42, true, false)]
        public void ValueInRange(long from, long to, long value, bool isFromInclusive, bool isToInclusive)
        {
            var testTarget = new Range<long>(from, to, isFromInclusive, isToInclusive);

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
        public void ValueOutOfRange(int from, int to, int value, bool isFromInclusive, bool isToInclusive)
        {
            var testTarget = new Range<int>(from, to, isFromInclusive, isToInclusive);

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

            act.Should().Throw<ArgumentOutOfRangeException>().And
               .Message.Should().Contain($"{nameof(to)} must not be less than {from}, but it actually is {to}.");
        }
    }
}