using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustContainTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 5)]
    [InlineData(new[] { -5491, 6199 }, 42)]
    public static void ItemNotPartOf(int[] collection, int item)
    {
        Action act = () => collection.MustContain(item, nameof(collection));

        var assertion = act.Should().Throw<MissingItemException>().Which;
        assertion.Message.Should().Contain($"{nameof(collection)} must contain {item}, but it actually does not.");
    }

    [Theory]
    [InlineData(new[] { "Foo", "Bar" }, "Foo")]
    [InlineData(new[] { "Foo", "Bar", "Foo" }, "Foo")]
    [InlineData(new[] { "Qux" }, "Qux")]
    [InlineData(new[] { "Qux", null }, null)]
    public static void ItemPartOf(string[] collection, string item) =>
        collection.MustContain(item).Should().BeSameAs(collection);

    [Fact]
    public static void CollectionNull()
    {
        Action act = () => ((string[]) null).MustContain("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(new[] { long.MinValue, long.MaxValue }, 42L)]
    [InlineData(null, 42L)]
    public static void CustomException(long[] array, long item) =>
        Test.CustomException(array,
                             item,
                             (collection, i, exceptionFactory) => collection.MustContain(i, exceptionFactory));

    [Fact]
    public static void CustomExceptionNotThrown()
    {
        var collection = new List<int> { 1, 2, 3 };
        collection.MustContain(2, (_, _) => new Exception()).Should().BeSameAs(collection);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<MissingItemException>(message => new List<string>().MustContain("Foo", message: message));

    [Fact]
    public static void CustomMessageCollectionNull() => 
        Test.CustomMessage<ArgumentNullException>(message => ((ObservableCollection<string>) null).MustContain("Foo", message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var array = new [] { "Foo", "Bar" };

        var act = () => array.MustContain("Baz");

        act.Should().Throw<MissingItemException>()
           .And.ParamName.Should().Be(nameof(array));
    }
}