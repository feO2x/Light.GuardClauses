using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustNotBeNullOrEmptyTests
{
    [Fact]
    public static void CollectionNull()
    {
        // ReSharper disable once ExplicitCallerInfoArgument
        Action act = () => ((object[]) null).MustNotBeNullOrEmpty("Foo");

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be("Foo");
    }

    [Fact]
    public static void CollectionEmpty()
    {
        var empty = new List<double>();

        Action act = () => empty.MustNotBeNullOrEmpty(nameof(empty));

        var assertion = act.Should().Throw<EmptyCollectionException>().Which;
        assertion.Message.Should().Contain($"{nameof(empty)} must not be an empty collection, but it actually is.");
        assertion.ParamName.Should().BeSameAs(nameof(empty));
    }

    [Fact]
    public static void CollectionNotEmpty()
    {
        var collection = new HashSet<string> { "Baz", "Qux", "Corge" };

        collection.MustNotBeNullOrEmpty().Should().BeSameAs(collection);
    }

    [Theory]
    [InlineData(new int[] {})]
    [InlineData(null)]
    public static void CustomException(int[] collection) =>
        Test.CustomException(collection, 
                             (_, exceptionFactory) => collection.MustNotBeNullOrEmpty(exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown()
    {
        var collection = new List<int>{42};
        collection.MustNotBeNullOrEmpty(_ => new Exception()).Should().BeSameAs(collection);
    }

    [Fact]
    public static void CustomMessage() => 
        Test.CustomMessage<EmptyCollectionException>(message => new HashSet<string>().MustNotBeNullOrEmpty(message: message));

    [Fact]
    public static void CustomMessageCollectionNull() => 
        Test.CustomMessage<ArgumentNullException>(message => ((List<int>) null).MustNotBeNullOrEmpty(message: message));

    [Fact]
    public static void CallerArgumentExpressionForEmptyCollection()
    {
        var emptyArray = Array.Empty<int>();

        Action act = () => emptyArray.MustNotBeNullOrEmpty();

        act.Should().Throw<EmptyCollectionException>()
           .And.ParamName.Should().Be(nameof(emptyArray));
    }

    [Fact]
    public static void CallerArgumentExpressionForNull()
    {
        var nullArray = default(int[]);

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => nullArray.MustNotBeNullOrEmpty();

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(nullArray));
    }
}