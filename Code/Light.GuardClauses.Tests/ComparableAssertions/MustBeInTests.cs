using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBeInTests
{
    [Theory]
    [InlineData(-1, 1, 5)]
    [InlineData(5, 1, 5)]
    [InlineData(6, 1, 5)]
    public static void ParameterOutOfInclusiveLowerAndExclusiveUpperBoundary(int value, int lowerBoundary, int upperBoundary)
    {
        Action act = () => value.MustBeIn(Range<int>.FromInclusive(lowerBoundary).ToExclusive(upperBoundary), nameof(value));

        var assertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        assertion.Message.Should().Contain($"{nameof(value)} must be between {lowerBoundary} (inclusive) and {upperBoundary} (exclusive), but it actually is {value}.");
        assertion.ParamName.Should().Be(nameof(value));
    }

    [Theory]
    [InlineData(0, 1, 5)]
    [InlineData(1, 1, 5)]
    [InlineData(6, 1, 5)]
    [InlineData('a', 'b', 'f')]
    [InlineData('b', 'b', 'f')]
    [InlineData('g', 'b', 'f')]
    public static void ParameterOutOfExclusiveLowerAndInclusiveUpperBoundary(char value, char lowerBoundary, char upperBoundary)
    {
        Action act = () => value.MustBeIn(Range<char>.FromExclusive(lowerBoundary).ToInclusive(upperBoundary), nameof(value));

        var assertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
        assertion.Message.Should().Contain($"{nameof(value)} must be between {lowerBoundary} (exclusive) and {upperBoundary} (inclusive), but it actually is {value}.");
        assertion.ParamName.Should().Be(nameof(value));
    }

    [Theory]
    [InlineData(10, 10, 20, true, true)]
    [InlineData(20, 10, 20, true, true)]
    [InlineData(11, 10, 20, true, false)]
    [InlineData(11, 10, 20, false, false)]
    [InlineData(19, 10, 20, false, false)]
    public static void ParameterWithinRange(int value, int lowerBoundary, int upperBoundary, bool isLowerBoundaryInclusive, bool isUpperBoundaryInclusive)
    {
        var range = new Range<int>(lowerBoundary, upperBoundary, isLowerBoundaryInclusive, isUpperBoundaryInclusive);
        var result = value.MustBeIn(range, nameof(value));

        result.Should().Be(value);
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException(50, new Range<int>(15, 25), (x, range, exceptionFactory) => x.MustBeIn(range, exceptionFactory));

    [Fact]
    public static void CustomExceptionParameterNull() =>
        Test.CustomException((string) null,
                             new Range<string>("a", "f"),
                             (s, range, exceptionFactory) => s.MustBeIn(range, exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown() =>
        5.6m.MustBeIn(Range<decimal>.FromInclusive(0m).ToExclusive(100m), (_, _) => null).Should().Be(5.6m);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(message => 'A'.MustBeIn(Range<char>.FromInclusive('a').ToInclusive('z'), message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeIn(Range<string>.FromInclusive("A").ToExclusive("Z"), message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var twenty = 20;

        Action act = () => twenty.MustBeIn(Range.FromInclusive(10).ToInclusive(15));

        act.Should().Throw<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be(nameof(twenty));
    }
}