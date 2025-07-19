using System;
using System.Collections.Immutable;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustNotBeDefaultOrEmptyTests
{
    [Fact]
    public static void ImmutableArrayDefault()
    {
        var defaultArray = default(ImmutableArray<int>);

        Action act = () => defaultArray.MustNotBeDefaultOrEmpty(nameof(defaultArray));

        var assertion = act.Should().Throw<EmptyCollectionException>().Which;
        assertion.Message.Should()
                 .Contain($"{nameof(defaultArray)} must not be an empty collection, but it actually is.");
        assertion.ParamName.Should().BeSameAs(nameof(defaultArray));
    }

    [Fact]
    public static void ImmutableArrayEmpty()
    {
        var emptyArray = ImmutableArray<string>.Empty;

        Action act = () => emptyArray.MustNotBeDefaultOrEmpty(nameof(emptyArray));

        var assertion = act.Should().Throw<EmptyCollectionException>().Which;
        assertion.Message.Should()
                 .Contain($"{nameof(emptyArray)} must not be an empty collection, but it actually is.");
        assertion.ParamName.Should().BeSameAs(nameof(emptyArray));
    }

    [Fact]
    public static void ImmutableArrayNotEmpty()
    {
        var array = ImmutableArray.Create("Foo", "Bar", "Baz");

        array.MustNotBeDefaultOrEmpty().Should().Equal(array);
    }

    [Theory]
    [MemberData(nameof(DefaultOrEmptyArrays))]
    public static void CustomException(ImmutableArray<int> array) =>
        Test.CustomException(
            array,
            (invalidArray, exceptionFactory) => invalidArray.MustNotBeDefaultOrEmpty(exceptionFactory)
        );

    [Fact]
    public static void NoCustomExceptionThrown()
    {
        var array = ImmutableArray.Create(42, 84);
        array.MustNotBeDefaultOrEmpty(_ => new ()).Should().Equal(array);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<EmptyCollectionException>(
            message => ImmutableArray<string>.Empty.MustNotBeDefaultOrEmpty(message: message)
        );

    [Fact]
    public static void CallerArgumentExpressionForEmptyArray()
    {
        var emptyArray = ImmutableArray<int>.Empty;

        Action act = () => emptyArray.MustNotBeDefaultOrEmpty();

        act.Should().Throw<EmptyCollectionException>()
           .WithParameterName(nameof(emptyArray));
    }

    [Fact]
    public static void CallerArgumentExpressionForDefaultArray()
    {
        var defaultArray = default(ImmutableArray<int>);

        Action act = () => defaultArray.MustNotBeDefaultOrEmpty();

        act.Should().Throw<EmptyCollectionException>()
           .WithParameterName(nameof(defaultArray));
    }

    public static TheoryData<ImmutableArray<int>> DefaultOrEmptyArrays() => new ()
    {
        default,
        ImmutableArray<int>.Empty,
    };
}
