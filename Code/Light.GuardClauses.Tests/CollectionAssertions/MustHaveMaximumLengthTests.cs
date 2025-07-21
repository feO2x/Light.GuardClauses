using System;
using System.Collections.Immutable;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustHaveMaximumLengthTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4 }, 3)]
    [InlineData(new[] { 1, 2 }, 1)]
    [InlineData(new[] { 500 }, 0)]
    public static void ImmutableArrayMoreItems(int[] items, int length)
    {
        var immutableArray = items.ToImmutableArray();
        Action act = () => immutableArray.MustHaveMaximumLength(length, nameof(immutableArray));

        var assertion = act.Should().Throw<InvalidCollectionCountException>().Which;
        assertion.Message.Should().Contain(
            $"{nameof(immutableArray)} must have at most count {length}, but it actually has count {immutableArray.Length}."
        );
        assertion.ParamName.Should().BeSameAs(nameof(immutableArray));
    }

    [Theory]
    [InlineData(new[] { "Foo" }, 1)]
    [InlineData(new[] { "Bar" }, 2)]
    [InlineData(new[] { "Baz", "Qux", "Quux" }, 5)]
    public static void ImmutableArrayLessOrEqualItems(string[] items, int length)
    {
        var immutableArray = items.ToImmutableArray();
        var result = immutableArray.MustHaveMaximumLength(length);
        result.Should().Equal(immutableArray);
    }

    [Fact]
    public static void ImmutableArrayEmpty()
    {
        var emptyArray = ImmutableArray<int>.Empty;
        var result = emptyArray.MustHaveMaximumLength(5);
        result.Should().Equal(emptyArray);
    }

    [Theory]
    [InlineData(new[] { 87, 89, 99 }, 1)]
    [InlineData(new[] { 1, 2, 3 }, -30)]
    public static void ImmutableArrayCustomException(int[] items, int maximumLength)
    {
        var immutableArray = items.ToImmutableArray();

        Action act = () => immutableArray.MustHaveMaximumLength(
            maximumLength,
            (array, length) => new ($"Custom exception for array with length {array.Length} and max {length}")
        );

        act.Should().Throw<Exception>()
           .WithMessage($"Custom exception for array with length {immutableArray.Length} and max {maximumLength}");
    }

    [Fact]
    public static void ImmutableArrayNoCustomExceptionThrown()
    {
        var immutableArray = new[] { "Foo", "Bar" }.ToImmutableArray();
        var result = immutableArray.MustHaveMaximumLength(2, (_, _) => new ());
        result.Should().Equal(immutableArray);
    }

    [Fact]
    public static void ImmutableArrayCustomMessage()
    {
        var immutableArray = new[] { 1, 2, 3 }.ToImmutableArray();

        Test.CustomMessage<InvalidCollectionCountException>(
            message => immutableArray.MustHaveMaximumLength(2, message: message)
        );
    }

    [Fact]
    public static void ImmutableArrayCallerArgumentExpression()
    {
        var myImmutableArray = new[] { 1, 2, 3 }.ToImmutableArray();

        var act = () => myImmutableArray.MustHaveMaximumLength(2);

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(myImmutableArray));
    }
}
