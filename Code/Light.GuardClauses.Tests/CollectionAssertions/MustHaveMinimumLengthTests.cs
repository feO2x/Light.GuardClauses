using System;
using System.Collections.Immutable;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustHaveMinimumLengthTests
{
    [Theory]
    [InlineData(new[] { 1, 2 }, 3)]
    [InlineData(new[] { 1 }, 2)]
    [InlineData(new int[] { }, 1)]
    public static void ImmutableArrayFewerItems(int[] items, int length)
    {
        var immutableArray = items.ToImmutableArray();
        Action act = () => immutableArray.MustHaveMinimumLength(length, nameof(immutableArray));

        var assertion = act.Should().Throw<InvalidCollectionCountException>().Which;
        assertion.Message.Should().Contain(
            $"{nameof(immutableArray)} must have at least a length of {length}, but it actually has a length of {immutableArray.Length}."
        );
        assertion.ParamName.Should().BeSameAs(nameof(immutableArray));
    }

    [Theory]
    [InlineData(new[] { "Foo" }, 1)]
    [InlineData(new[] { "Bar" }, 0)]
    [InlineData(new[] { "Baz", "Qux", "Quux" }, 2)]
    public static void ImmutableArrayMoreOrEqualItems(string[] items, int length)
    {
        var immutableArray = items.ToImmutableArray();
        var result = immutableArray.MustHaveMinimumLength(length);
        result.Should().Equal(immutableArray);
    }

    [Fact]
    public static void ImmutableArrayEmpty()
    {
        var emptyArray = ImmutableArray<int>.Empty;
        var result = emptyArray.MustHaveMinimumLength(0);
        result.Should().Equal(emptyArray);
    }

    [Theory]
    [InlineData(new[] { 87 }, 3)]
    [InlineData(new[] { 1, 2 }, 5)]
    public static void ImmutableArrayCustomException(int[] items, int minimumLength)
    {
        var immutableArray = items.ToImmutableArray();

        Action act = () => immutableArray.MustHaveMinimumLength(
            minimumLength,
            (array, length) => new ($"Custom exception for array with length {array.Length} and min {length}")
        );

        act.Should().Throw<Exception>()
           .WithMessage($"Custom exception for array with length {immutableArray.Length} and min {minimumLength}");
    }

    [Fact]
    public static void ImmutableArrayNoCustomExceptionThrown()
    {
        var immutableArray = new[] { "Foo", "Bar" }.ToImmutableArray();
        var result = immutableArray.MustHaveMinimumLength(2, (_, _) => new ());
        result.Should().Equal(immutableArray);
    }

    [Fact]
    public static void ImmutableArrayCustomMessage()
    {
        var immutableArray = new[] { 1 }.ToImmutableArray();

        Test.CustomMessage<InvalidCollectionCountException>(
            message => immutableArray.MustHaveMinimumLength(3, message: message)
        );
    }

    [Fact]
    public static void ImmutableArrayCallerArgumentExpression()
    {
        var myImmutableArray = new[] { 1 }.ToImmutableArray();

        var act = () => myImmutableArray.MustHaveMinimumLength(2);

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(myImmutableArray));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public static void
        DefaultImmutableArrayInstanceShouldNotThrowWhenLengthIsLessOrEqualToZero(int validLength) =>
        default(ImmutableArray<int>).MustHaveMinimumLength(validLength).IsDefault.Should().BeTrue();

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(12)]
    public static void DefaultImmutableArrayInstanceShouldThrowWhenLengthIsPositive(int positiveLength)
    {
        var act = () => default(ImmutableArray<int>).MustHaveMinimumLength(positiveLength);

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName("default(ImmutableArray<int>)")
           .WithMessage(
                $"default(ImmutableArray<int>) must have at least a length of {positiveLength}, but it actually has no length because it is the default instance.*"
            );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public static void DefaultImmutableArrayInstanceCustomExceptionShouldNotThrow(int validLength)
    {
        var result = default(ImmutableArray<int>).MustHaveMinimumLength(validLength, (_, _) => new Exception());
        result.IsDefault.Should().BeTrue();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(12)]
    public static void DefaultImmutableArrayInstanceCustomExceptionShouldThrow(int positiveLength)
    {
        var act = () => default(ImmutableArray<int>).MustHaveMinimumLength(
            positiveLength,
            (array, length) => new ArgumentException(
                $"Custom: Array length {(array.IsDefault ? 0 : array.Length)} is below minimum {length}"
            )
        );

        act.Should().Throw<ArgumentException>()
           .WithMessage("Custom: Array length 0 is below minimum *");
    }
}
