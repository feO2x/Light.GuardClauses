#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustNotContainNullTests
{
    [Fact]
    public static void EmptyArraySucceedsAndPreservesShape()
    {
        var values = Array.Empty<string?>();

        var result = values.MustNotContainNull();

        result.Should().BeSameAs(values);
    }

    [Fact]
    public static void ValidListSucceedsAndPreservesShape()
    {
        var values = new List<string?> { "Alpha", "Beta" };

        var result = values.MustNotContainNull();

        result.Should().BeSameAs(values);
    }

    [Fact]
    public static void NullReferenceItemThrowsAtItsPositionWithoutRenderingTheCollection()
    {
        var values = new[] { "Alpha", "Beta", null, "Gamma" };

        Action act = () => values.MustNotContainNull();

        var exception = act.Should().Throw<ExistingItemException>().Which;
        exception.ParamName.Should().Be(nameof(values));
        exception.Message.Should().Contain("must not contain null items");
        exception.Message.Should().Contain("position 2");
        exception.Message.Should().NotContain("Alpha");
    }

    [Fact]
    public static void EmptyNullableValueTypeIsObservedAsNullThroughNonGenericEnumeration()
    {
        var values = new List<int?> { 1, null, 3 };

        Action act = () => values.MustNotContainNull();

        act.Should().Throw<ExistingItemException>()
           .WithMessage("*position 1*");
    }

    [Fact]
    public static void PopulatedNullableValueTypesSucceed()
    {
        var values = new List<int?> { 1, 2, 3 };

        values.MustNotContainNull().Should().BeSameAs(values);
    }

    [Fact]
    public static void NullReceiverThrowsArgumentNullExceptionWithCallerExpression()
    {
        List<object?>? values = null;

        Action act = () => values.MustNotContainNull();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(values));
    }

    [Fact]
    public static void CustomMessageReplacesDefaultMessage() =>
        Test.CustomMessage<ExistingItemException>(
            message => new object?[] { null }.MustNotContainNull(message: message)
        );

    [Fact]
    public static void NullReceiverCustomMessageIsPassedToArgumentNullException() =>
        Test.CustomMessage<ArgumentNullException>(
            message => ((List<object?>?) null).MustNotContainNull(message: message)
        );

    [Fact]
    public static void CustomFactoryReceivesInvalidCollection() =>
        Test.CustomException<List<string?>?>(
            ["Alpha", null],
            (values, factory) => values.MustNotContainNull(factory)
        );

    [Fact]
    public static void CustomFactoryReceivesNullCollection() =>
        Test.CustomException(
            (List<string?>?) null,
            (values, factory) => values.MustNotContainNull(factory)
        );

    [Fact]
    public static void ValidLazyEnumerableIsEnumeratedOnceAndDisposed()
    {
        var values = new TrackingEnumerable<string?>(["Alpha", "Beta"]);

        values.MustNotContainNull().Should().BeSameAs(values);

        values.EnumeratorCount.Should().Be(1);
        values.MoveNextCount.Should().Be(3);
        values.DisposeCount.Should().Be(1);
    }

    [Fact]
    public static void InvalidSingleUseEnumerableStopsEarlyAndIsDisposed()
    {
        var values = new TrackingEnumerable<string?>(["Alpha", null, "unobserved"]);

        Action act = () => values.MustNotContainNull();

        act.Should().Throw<ExistingItemException>();
        values.EnumeratorCount.Should().Be(1);
        values.MoveNextCount.Should().Be(2);
        values.DisposeCount.Should().Be(1);
    }

    [Fact]
    public static void IndexableReceiversDoNotRequestAnEnumerator()
    {
        var validValues = new IndexableOnlyList<string?>(["Alpha", "Beta"]);
        var invalidValues = new IndexableOnlyList<string?>(["Alpha", null, "Gamma"]);

        validValues.MustNotContainNull().Should().BeSameAs(validValues);
        validValues.MustNotContainNull(_ => new InvalidOperationException()).Should().BeSameAs(validValues);
        Action act = () => invalidValues.MustNotContainNull();

        act.Should().Throw<ExistingItemException>()
           .WithMessage("*position 1*");
    }

    [Fact]
    public static void EmptyImmutableArraySucceeds()
    {
        var values = ImmutableArray<string?>.Empty;

        var result = values.MustNotContainNull();

        result.Should().Equal(values);
    }

    [Fact]
    public static void DefaultImmutableArraySucceeds()
    {
        var values = default(ImmutableArray<string?>);

        var result = values.MustNotContainNull();

        result.IsDefault.Should().BeTrue();
    }

    [Fact]
    public static void InvalidImmutableArrayThrowsAtItsPosition()
    {
        var values = ImmutableArray.Create("Alpha", null, "Gamma");

        Action act = () => values.MustNotContainNull();

        act.Should().Throw<ExistingItemException>()
           .WithParameterName(nameof(values))
           .WithMessage("*position 1*");
    }

    [Fact]
    public static void ImmutableArrayCustomFactoryReceivesOriginalShape() =>
        Test.CustomException(
            ImmutableArray.CreateRange(new string?[] { null }),
            (values, factory) => values.MustNotContainNull(factory)
        );

    [Fact]
    public static void DefaultImmutableArrayDoesNotInvokeCustomFactory()
    {
        var values = default(ImmutableArray<string?>);

        var result = values.MustNotContainNull(_ => new InvalidOperationException());

        result.IsDefault.Should().BeTrue();
    }
}
