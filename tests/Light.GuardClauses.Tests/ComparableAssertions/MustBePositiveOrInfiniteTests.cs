using System;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions;

public static class MustBePositiveOrInfiniteTests
{
    [Fact]
    public static void PositiveBoundaryValuesAreAcceptedAndReturned()
    {
        TimeSpan.FromTicks(1).MustBePositiveOrInfinite().Should().Be(TimeSpan.FromTicks(1));
        TimeSpan.MaxValue.MustBePositiveOrInfinite().Should().Be(TimeSpan.MaxValue);
    }

    [Fact]
    public static void InfiniteTimeoutIsAcceptedAndReturned()
    {
        Timeout.InfiniteTimeSpan.MustBePositiveOrInfinite().Should().Be(Timeout.InfiniteTimeSpan);
        TimeSpan.FromMilliseconds(-1).MustBePositiveOrInfinite().Should().Be(Timeout.InfiniteTimeSpan);
    }

    [Theory]
    [MemberData(nameof(InvalidValues))]
    public static void ZeroAndNegativeValuesOtherThanInfiniteTimeoutAreRejected(TimeSpan invalidValue)
    {
        var act = () => invalidValue.MustBePositiveOrInfinite();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidValue))
           .WithMessage(
                $"*must be positive or equal to Timeout.InfiniteTimeSpan, but it actually is {invalidValue}*"
            );
    }

    public static TheoryData<TimeSpan> InvalidValues =>
        new ()
        {
            TimeSpan.Zero,
            TimeSpan.FromTicks(Timeout.InfiniteTimeSpan.Ticks - 1),
            TimeSpan.FromTicks(Timeout.InfiniteTimeSpan.Ticks + 1),
            TimeSpan.FromSeconds(-1),
            TimeSpan.MinValue,
        };

    [Fact]
    public static void DefaultExceptionCapturesExpressionAndValue()
    {
        var invalidTimeout = TimeSpan.FromTicks(-1);

        var act = () => invalidTimeout.MustBePositiveOrInfinite();

        act.Should().Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(invalidTimeout))
           .WithMessage(
                $"*invalidTimeout must be positive or equal to Timeout.InfiniteTimeSpan, but it actually is {invalidTimeout}*"
            );
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentOutOfRangeException>(
            message => TimeSpan.Zero.MustBePositiveOrInfinite(message: message)
        );

    [Fact]
    public static void CustomFactoryReceivesOriginalValue() =>
        Test.CustomException(
            TimeSpan.FromTicks(Timeout.InfiniteTimeSpan.Ticks + 1),
            (value, factory) => value.MustBePositiveOrInfinite(factory)
        );

    [Fact]
    public static void CustomFactoriesAreNotInvokedForAcceptedValues()
    {
        TimeSpan.FromTicks(1)
                .MustBePositiveOrInfinite(_ => FactoryMustNotBeInvoked())
                .Should().Be(TimeSpan.FromTicks(1));
        Timeout.InfiniteTimeSpan
               .MustBePositiveOrInfinite(_ => FactoryMustNotBeInvoked())
               .Should().Be(Timeout.InfiniteTimeSpan);
    }

    [Fact]
    public static void NullFactoryThrowsArgumentNullExceptionForInvalidValue()
    {
        var act = () => TimeSpan.Zero.MustBePositiveOrInfinite(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("exceptionFactory");
    }

    private static Exception FactoryMustNotBeInvoked() =>
        throw new InvalidOperationException("The exception factory must not be invoked for a valid value.");
}
