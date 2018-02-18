using System;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class IsLengthInTests
    {
        [Theory(DisplayName = "IsLengthIn must return true when the length of the string is within the boundaries of the specified string, else false.")]
        [MemberData(nameof(IsLenghtInRangeData))]
        public void IsLenghtInRange(string @string, Range<int> range, bool expected)
        {
            @string.IsLengthIn(range).Should().Be(expected);
        }

        public static readonly TestData IsLenghtInRangeData =
            new[]
            {
                new object[] { "Foo", Range<int>.FromInclusive(2).ToExclusive(10), true },
                new object[] { "Foo", Range<int>.FromInclusive(4).ToExclusive(10), false },
                new object[] { "", Range<int>.FromInclusive(0).ToInclusive(5), true },
                new object[] { "", Range<int>.FromExclusive(0).ToInclusive(5), false },
                new object[] { "If I wanted to kill you, do you think I'd let a wooden door stop me?", Range<int>.FromExclusive(14).ToInclusive(100), true }
            };

        [Fact(DisplayName = "IsLengthIn must throw an ArgumentNullException when the specified string is null.")]
        public void StringNull()
        {
            Action act = () => ((string) null).IsLengthIn(Range<int>.FromInclusive(42).ToExclusive(87));

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("string");
        }
    }
}