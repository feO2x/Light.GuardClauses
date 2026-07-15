#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustNotContainNullOrWhiteSpaceTests
{
    [Fact]
    public static void EmptyArraySucceedsAndPreservesShape()
    {
        var values = Array.Empty<string?>();

        var result = values.MustNotContainNullOrWhiteSpace();

        result.Should().BeSameAs(values);
    }

    [Fact]
    public static void ValidListSucceedsAndPreservesShape()
    {
        var values = new List<string?> { "Alpha", "Beta" };

        var result = values.MustNotContainNullOrWhiteSpace();

        result.Should().BeSameAs(values);
    }

    [Theory]
    [InlineData(null, "a null string")]
    [InlineData("", "an empty string")]
    [InlineData(" \t\r\n", "a white-space-only string")]
    [InlineData("\u2003", "a white-space-only string")]
    public static void InvalidStringThrowsWithCategoryAndPosition(string? invalidValue, string category)
    {
        var values = new[] { "Alpha", invalidValue, "Gamma" };

        Action act = () => values.MustNotContainNullOrWhiteSpace();

        var exception = act.Should().Throw<ExistingItemException>().Which;
        exception.ParamName.Should().Be(nameof(values));
        exception.Message.Should().Contain(category);
        exception.Message.Should().Contain("position 1");
        exception.Message.Should().NotContain("Alpha");
    }

    [Fact]
    public static void NullReceiverThrowsArgumentNullExceptionWithCallerExpression()
    {
        List<string?>? values = null;

        Action act = () => values.MustNotContainNullOrWhiteSpace();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(values));
    }

    [Fact]
    public static void CustomMessageReplacesDefaultMessage() =>
        Test.CustomMessage<ExistingItemException>(
            message => new[] { "" }.MustNotContainNullOrWhiteSpace(message: message)
        );

    [Fact]
    public static void NullReceiverCustomMessageIsPassedToArgumentNullException() =>
        Test.CustomMessage<ArgumentNullException>(
            message => ((List<string?>?) null).MustNotContainNullOrWhiteSpace(message: message)
        );

    [Fact]
    public static void CustomFactoryReceivesInvalidCollection() =>
        Test.CustomException<List<string?>?>(
            ["Alpha", " "],
            (values, factory) => values.MustNotContainNullOrWhiteSpace(factory)
        );

    [Fact]
    public static void CustomFactoryReceivesNullCollection() =>
        Test.CustomException(
            (List<string?>?) null,
            (values, factory) => values.MustNotContainNullOrWhiteSpace(factory)
        );

    [Fact]
    public static void ValidLazyEnumerableIsEnumeratedOnceAndDisposed()
    {
        var values = new TrackingEnumerable<string?>(["Alpha", "Beta"]);

        values.MustNotContainNullOrWhiteSpace().Should().BeSameAs(values);

        values.EnumeratorCount.Should().Be(1);
        values.MoveNextCount.Should().Be(3);
        values.DisposeCount.Should().Be(1);
    }

    [Fact]
    public static void InvalidSingleUseEnumerableStopsEarlyAndIsDisposed()
    {
        var values = new TrackingEnumerable<string?>(["Alpha", "\u2003", "unobserved"]);

        Action act = () => values.MustNotContainNullOrWhiteSpace();

        act.Should().Throw<ExistingItemException>();
        values.EnumeratorCount.Should().Be(1);
        values.MoveNextCount.Should().Be(2);
        values.DisposeCount.Should().Be(1);
    }

    [Fact]
    public static void IndexableReceiversDoNotRequestAnEnumerator()
    {
        var validValues = new IndexableOnlyList<string?>(["Alpha", "Beta"]);
        var invalidValues = new IndexableOnlyList<string?>(["Alpha", " ", "Gamma"]);
        var readOnlyValues = new ReadOnlyIndexableOnlyList<string?>(["Alpha", "Beta"]);

        validValues.MustNotContainNullOrWhiteSpace().Should().BeSameAs(validValues);
        validValues.MustNotContainNullOrWhiteSpace(_ => new InvalidOperationException()).Should().BeSameAs(validValues);
        readOnlyValues.MustNotContainNullOrWhiteSpace().Should().BeSameAs(readOnlyValues);
        Action act = () => invalidValues.MustNotContainNullOrWhiteSpace();

        act.Should().Throw<ExistingItemException>()
           .WithMessage("*position 1*");
    }

    [Fact]
    public static void EmptyImmutableArraySucceeds()
    {
        var values = ImmutableArray<string?>.Empty;

        var result = values.MustNotContainNullOrWhiteSpace();

        result.Should().Equal(values);
    }

    [Fact]
    public static void DefaultImmutableArraySucceeds()
    {
        var values = default(ImmutableArray<string?>);

        var result = values.MustNotContainNullOrWhiteSpace();

        result.IsDefault.Should().BeTrue();
    }

    [Fact]
    public static void InvalidImmutableArrayThrowsAtItsPosition()
    {
        var values = ImmutableArray.Create<string?>("Alpha", "", "Gamma");

        Action act = () => values.MustNotContainNullOrWhiteSpace();

        act.Should().Throw<ExistingItemException>()
           .WithParameterName(nameof(values))
           .WithMessage("*position 1*");
    }

    [Fact]
    public static void InvalidReadOnlyIndexableReceiverThrowsAtItsPosition()
    {
        var values = new ReadOnlyIndexableOnlyList<string?>(["Alpha", " ", "Gamma"]);

        Action act = () => values.MustNotContainNullOrWhiteSpace();

        act.Should().Throw<ExistingItemException>()
           .WithParameterName(nameof(values))
           .WithMessage("*position 1*");
    }

    [Fact]
    public static void ImmutableArrayCustomFactoryReceivesOriginalShape() =>
        Test.CustomException(
            ImmutableArray.Create<string?>(" "),
            (values, factory) => values.MustNotContainNullOrWhiteSpace(factory)
        );

    [Fact]
    public static void ValidImmutableArrayDoesNotInvokeCustomFactory()
    {
        var values = ImmutableArray.Create<string?>("Alpha", "Beta");

        var result = values.MustNotContainNullOrWhiteSpace(
            _ => new InvalidOperationException("The factory must not be invoked.")
        );

        result.Should().Equal(values);
    }

    [Fact]
    public static void DefaultImmutableArrayDoesNotInvokeCustomFactory()
    {
        var values = default(ImmutableArray<string?>);

        var result = values.MustNotContainNullOrWhiteSpace(_ => new InvalidOperationException());

        result.IsDefault.Should().BeTrue();
    }
}
