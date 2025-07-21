using System;
using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustHaveLengthInTests
{
    [Theory]
    [MemberData(nameof(LengthInRangeData))]
    public static void LengthInRange(ImmutableArray<int> array, Range<int> range) =>
        array.MustHaveLengthIn(range).Should().Equal(array);

    public static readonly TheoryData<ImmutableArray<int>, Range<int>> LengthInRangeData =
        new ()
        {
            { [1, 2, 3], Range.FromInclusive(0).ToExclusive(10) },
            { [1, 2, 3, 4, 5], Range.FromInclusive(3).ToInclusive(5) },
            { ImmutableArray<int>.Empty, Range.FromInclusive(0).ToExclusive(100) },
            { [1, 2, 3, 4, 5, 6, 7, 8, 9, 10], Range.FromExclusive(5).ToInclusive(10) },
        };

    [Theory]
    [MemberData(nameof(LengthNotInRangeData))]
    public static void LengthNotInRange(ImmutableArray<int> array, Range<int> range)
    {
        var act = () => array.MustHaveLengthIn(range, nameof(array));

        act.Should().Throw<ArgumentOutOfRangeException>()
           .And.Message.Should().Contain($"must have its length in between {range.CreateRangeDescriptionText("and")}")
           .And.Contain($"but it actually has length {array.Length}");
    }

    public static readonly TheoryData<ImmutableArray<int>, Range<int>> LengthNotInRangeData =
        new ()
        {
            { [1, 2, 3], Range.FromInclusive(10).ToInclusive(20) },
            { [1, 2, 3, 4], Range.FromExclusive(4).ToExclusive(10) },
            { ImmutableArray<int>.Empty, Range.FromInclusive(1).ToExclusive(50) },
            { [1, 2], Range.FromInclusive(100).ToExclusive(256) },
        };

    [Fact]
    public static void CustomException() =>
        Test.CustomException(
            ImmutableArray.Create(1, 2, 3),
            Range.FromInclusive(5).ToInclusive(10),
            (array, r, exceptionFactory) => array.MustHaveLengthIn(r, exceptionFactory)
        );

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => ImmutableArray.Create(1, 2, 3).MustHaveLengthIn(
                Range.FromInclusive(42).ToInclusive(50),
                message: message
            )
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        var testArray = ImmutableArray.Create(1, 2, 3, 4, 5);

        var act = () => testArray.MustHaveLengthIn(Range.FromInclusive(10).ToExclusive(20));

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(testArray));
    }

    [Fact]
    public static void DefaultImmutableArrayInRange()
    {
        var defaultArray = default(ImmutableArray<int>);

        var result = defaultArray.MustHaveLengthIn(Range.FromInclusive(0).ToInclusive(5));

        result.IsDefault.Should().BeTrue();
    }

    [Fact]
    public static void DefaultImmutableArrayNotInRange()
    {
        var defaultArray = default(ImmutableArray<int>);

        var act = () => defaultArray.MustHaveLengthIn(Range.FromInclusive(1).ToInclusive(5), nameof(defaultArray));

        act.Should().Throw<ArgumentOutOfRangeException>()
           .And.Message.Should().Contain("must have its length in between 1 (inclusive) and 5 (inclusive)")
           .And.Contain("has no length because it is the default instance");
    }

    [Fact]
    public static void DefaultImmutableArrayCustomException()
    {
        var defaultArray = default(ImmutableArray<int>);

        Test.CustomException(
            defaultArray,
            Range.FromInclusive(1).ToInclusive(5),
            (array, r, exceptionFactory) => array.MustHaveLengthIn(r, exceptionFactory)
        );
    }

    [Fact]
    public static void DefaultImmutableArrayCustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => default(ImmutableArray<int>).MustHaveLengthIn(
                Range.FromInclusive(1).ToInclusive(5),
                message: message
            )
        );
}
