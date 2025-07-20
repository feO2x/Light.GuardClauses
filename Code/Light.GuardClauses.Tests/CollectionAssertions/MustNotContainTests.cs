using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustNotContainTests
{
    [Theory]
    [InlineData(new[] { "Foo", "Bar" }, "Foo")]
    [InlineData(new[] { "Baz", "Qux", "Quux" }, "Qux")]
    [InlineData(new[] { "Corge", "Grault", null }, null)]
    public static void ItemExists(string[] collection, string item)
    {
        Action act = () => collection.MustNotContain(item, nameof(collection));

        var assertions = act.Should().Throw<ExistingItemException>().Which;
        assertions.Message.Should().Contain($"{nameof(collection)} must not contain {item.ToStringOrNull()}, but it actually does.");
        assertions.ParamName.Should().BeSameAs(nameof(collection));
    }

    [Theory]
    [InlineData(new[] { 100, 101, 102 }, 42)]
    [InlineData(new[] { 11 }, -5000)]
    [InlineData(new int[] { }, 13)]
    public static void ItemExistsNot(int[] collection, int item) =>
        collection.MustNotContain(item).Should().BeSameAs(collection);

    [Fact]
    public static void CollectionNull()
    {
        Action act = () => ((ObservableCollection<object>)null).MustNotContain(new object());

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomException() =>
        Test.CustomException(new List<string> { "Foo" },
                             "Foo",
                             (collection, value, exceptionFactory) => collection.MustNotContain(value, exceptionFactory));

    [Fact]
    public static void CustomExceptionCollectionNull() =>
        Test.CustomException((Collection<int>)null,
                             42,
                             (collection, i, exceptionFactory) => collection.MustNotContain(i, exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown()
    {
        var collection = new[] { 1, 2 };
        collection.MustNotContain(3, (_, _) => new Exception()).Should().BeSameAs(collection);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ExistingItemException>(message => new HashSet<int> { 42 }.MustNotContain(42, message: message));

    [Fact]
    public static void CustomMessageCollectionNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((List<bool>)null).MustNotContain(false, message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var array = new[] { 1, 2, 3 };

        Action act = () => array.MustNotContain(3);

        act.Should().Throw<ExistingItemException>()
           .WithParameterName(nameof(array));
    }

    [Theory]
    [InlineData(new[] { "Foo", "Bar" }, "Foo")]
    [InlineData(new[] { "Baz", "Qux", "Quux" }, "Qux")]
    [InlineData(new[] { "Corge", "Grault", null }, null)]
    public static void ImmutableArrayItemExists(string[] items, string item)
    {
        var array = ImmutableArray.Create(items);

        Action act = () => array.MustNotContain(item, nameof(array));

        var assertions = act.Should().Throw<ExistingItemException>().Which;
        assertions.Message.Should()
                  .Contain($"{nameof(array)} must not contain {item.ToStringOrNull()}, but it actually does.");
        assertions.ParamName.Should().BeSameAs(nameof(array));
    }

    [Theory]
    [InlineData(new[] { 100, 101, 102 }, 42)]
    [InlineData(new[] { 11 }, -5000)]
    public static void ImmutableArrayItemExistsNot(int[] items, int item)
    {
        var array = ImmutableArray.Create(items);
        array.MustNotContain(item).Should().Equal(array);
    }

    [Fact]
    public static void ImmutableArrayEmptyDoesNotContainItem()
    {
        var emptyArray = ImmutableArray<int>.Empty;
        emptyArray.MustNotContain(42).Should().Equal(emptyArray);
    }

    [Fact]
    public static void ImmutableArrayCustomException() =>
        Test.CustomException(
            ImmutableArray.Create("Foo"),
            "Foo",
            (array, value, exceptionFactory) => array.MustNotContain(value, exceptionFactory)
        );

    [Fact]
    public static void ImmutableArrayNoCustomExceptionThrown()
    {
        var array = ImmutableArray.Create(1, 2);
        array.MustNotContain(3, (_, _) => new ()).Should().Equal(array);
    }

    [Fact]
    public static void ImmutableArrayCustomMessage() =>
        Test.CustomMessage<ExistingItemException>(
            message => ImmutableArray.Create(42).MustNotContain(42, message: message)
        );

    [Fact]
    public static void ImmutableArrayCallerArgumentExpression()
    {
        var array = ImmutableArray.Create(1, 2, 3);

        Action act = () => array.MustNotContain(3);

        act.Should().Throw<ExistingItemException>()
           .WithParameterName(nameof(array));
    }
}