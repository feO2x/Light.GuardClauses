using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustHaveLengthInTests
{
    [Theory]
    [MemberData(nameof(LengthInRangeData))]
    public static void LengthInRange(string @string, Range<int> range) =>
        @string.MustHaveLengthIn(range).Should().BeSameAs(@string);

    public static readonly TheoryData<string, Range<int>> LengthInRangeData =
        new ()
        {
            { "Foo", Range.FromInclusive(0).ToExclusive(10) },
            { "Bar", Range.FromInclusive(3).ToInclusive(5) },
            { "", Range.FromInclusive(0).ToExclusive(100) },
            { "Nothing isn’t better or worse than anything. Nothing is just nothing.", Range.FromExclusive(20).ToInclusive(69) }
        };

    [Theory]
    [MemberData(nameof(LengthNotInRangeData))]
    public static void LengthNotInRange(string @string, Range<int> range)
    {
        var act = () => @string.MustHaveLengthIn(range, nameof(@string));

        act.Should().Throw<StringLengthException>()
           .And.Message.Should().Contain($"string must have its length in between {range.CreateRangeDescriptionText("and")}, but it actually has length {@string.Length}.");
    }

    public static readonly TheoryData<string, Range<int>> LengthNotInRangeData =
        new ()
        {
            { "Baz", Range.FromInclusive(10).ToInclusive(20) },
            { "Qux", Range.FromExclusive(3).ToExclusive(10) },
            { "", Range.FromInclusive(1).ToExclusive(50) },
            { "There is only one god, and his name is Death.", Range.FromInclusive(100).ToExclusive(256) }
        };

    [Fact]
    public static void StringNull()
    {
        var act = () => ((string) null).MustHaveLengthIn(new Range<int>());

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException("Foo",
                             Range.FromInclusive(5).ToInclusive(10),
                             (s, r, exceptionFactory) => s.MustHaveLengthIn(r, exceptionFactory));

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<StringLengthException>(message => "Foo".MustHaveLengthIn(Range.FromInclusive(42).ToInclusive(50), message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string message = "God rest ye merry, gentlemen";

        var act = () => message.MustHaveLengthIn(Range.FromInclusive(3).ToExclusive(10));

        act.Should().Throw<StringLengthException>()
           .WithParameterName(nameof(message));
    }
}