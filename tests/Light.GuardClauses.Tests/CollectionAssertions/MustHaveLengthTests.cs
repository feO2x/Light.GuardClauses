using System;
using System.Collections.Immutable;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustHaveLengthTests
{
    [Theory]
    [InlineData(3, 2)]
    [InlineData(5, 6)]
    [InlineData(0, 1)]
    [InlineData(10, 11)]
    public static void ImmutableArrayInvalidLength(int arrayLength, int expectedLength)
    {
        var array = ImmutableArray.CreateRange(new int[arrayLength]);

        var act = () => array.MustHaveLength(expectedLength, nameof(array));

        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Contain(
                $"{nameof(array)} must have length {expectedLength}, but it actually has length {arrayLength}."
            );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public static void ImmutableArrayValidLength(int arrayLength)
    {
        var array = ImmutableArray.CreateRange(new string[arrayLength]);

        var result = array.MustHaveLength(arrayLength);

        result.Should().Equal(array);
    }

    [Fact]
    public static void ImmutableArrayCustomException()
    {
        var exception = new Exception();
        var array = ImmutableArray.Create("a", "b", "c");

        var act = () => array.MustHaveLength(4, (_, _) => exception);

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void ImmutableArrayNoCustomException()
    {
        var array = ImmutableArray.Create(1, 2, 3, 4);

        var result = array.MustHaveLength(4, (_, _) => null);

        result.Should().Equal(array);
    }

    [Fact]
    public static void ImmutableArrayCustomMessage()
    {
        var array = ImmutableArray<string>.Empty;

        var act = () => array.MustHaveLength(1, message: "Custom error message");

        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Contain("Custom error message");
    }

    [Fact]
    public static void ImmutableArrayCallerArgumentExpression()
    {
        var myArray = ImmutableArray.Create("foo", "bar");

        var act = () => myArray.MustHaveLength(10);

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName("myArray");
    }

    [Fact]
    public static void ImmutableArrayDefaultArray()
    {
        var defaultArray = default(ImmutableArray<int>);

        var act = () => defaultArray.MustHaveLength(5, nameof(defaultArray));

        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Contain($"{nameof(defaultArray)} must have length 5, but it actually has length 0.");
    }
}
