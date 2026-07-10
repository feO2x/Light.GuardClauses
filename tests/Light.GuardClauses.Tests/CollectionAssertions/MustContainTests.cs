using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
           .WithParameterName(nameof(array));
    }

    [Theory]
    [InlineData(new[] { 1, 2, 3 }, 5)]
    [InlineData(new[] { -5491, 6199 }, 42)]
    public static void ImmutableArrayItemNotPartOf(int[] source, int item)
    {
        var immutableArray = source.ToImmutableArray();
        Action act = () => immutableArray.MustContain(item, nameof(immutableArray));

        var assertion = act.Should().Throw<MissingItemException>().Which;
        assertion.Message.Should().Contain($"{nameof(immutableArray)} must contain {item}, but it actually does not.");
    }

    [Theory]
    [InlineData(new[] { "Foo", "Bar" }, "Foo")]
    [InlineData(new[] { "Foo", "Bar", "Foo" }, "Foo")]
    [InlineData(new[] { "Qux" }, "Qux")]
    [InlineData(new[] { "Qux", null }, null)]
    public static void ImmutableArrayItemPartOf(string[] source, string item)
    {
        var immutableArray = source.ToImmutableArray();
        immutableArray.MustContain(item).Should().Equal(immutableArray);
    }

    [Fact]
    public static void ImmutableArrayEmptyDoesNotContainItem()
    {
        var immutableArray = ImmutableArray<string>.Empty;
        Action act = () => immutableArray.MustContain("Foo");

        act.Should().Throw<MissingItemException>();
    }

    [Theory]
    [InlineData(new[] { 42L, 100L }, 1337L)]
    public static void ImmutableArrayCustomException(long[] source, long item)
    {
        var immutableArray = source.ToImmutableArray();
        Test.CustomException(
            immutableArray,
            item,
            (array, i, exceptionFactory) => array.MustContain(i, exceptionFactory)
        );
    }

    [Fact]
    public static void ImmutableArrayCustomExceptionNotThrown()
    {
        var immutableArray = ImmutableArray.Create(1, 2, 3);
        immutableArray.MustContain(2, (_, _) => new Exception()).Should().Equal(immutableArray);
    }

    [Fact]
    public static void ImmutableArrayCustomMessage() =>
        Test.CustomMessage<MissingItemException>(
            message => ImmutableArray<string>.Empty.MustContain("Foo", message: message)
        );

    [Fact]
    public static void ImmutableArrayCallerArgumentExpression()
    {
        var immutableArray = ImmutableArray.Create("Foo", "Bar");

        var act = () => immutableArray.MustContain("Baz");

        act.Should().Throw<MissingItemException>()
           .WithParameterName(nameof(immutableArray));
    }
}