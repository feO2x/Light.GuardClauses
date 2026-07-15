using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustHaveSameCountAsTests
{
    [Fact]
    public static void EqualCollectionsWithDifferentConcreteAndElementTypesReturnOriginalReceiver()
    {
        var keys = new List<int> { 1, 2 };
        var values = new[] { "one", "two" };

        var result = keys.MustHaveSameCountAs(values);

        result.Should().BeSameAs(keys);
    }

    [Fact]
    public static void EmptyCollectionsAreAccepted()
    {
        var collection = Array.Empty<int>();

        collection.MustHaveSameCountAs(new List<string>()).Should().BeSameAs(collection);
    }

    [Fact]
    public static void CollectionContentsAreNotCompared()
    {
        var collection = new[] { 1, 2 };

        collection.MustHaveSameCountAs(new[] { "different", "items" }).Should().BeSameAs(collection);
    }

    [Fact]
    public static void UnequalCountsThrowDefaultExceptionWithReceiverDetailsAndBothObservedCounts()
    {
        var keys = new[] { 1, 2, 3 };

        Action act = () => keys.MustHaveSameCountAs(new[] { "one" });

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(keys))
           .WithMessage(
                "keys must have the same count as the comparison collection, but its count is 3 and the comparison collection's count is 1.*"
            );
    }

    [Fact]
    public static void NullReceiverUsesCapturedReceiverExpression()
    {
        int[] values = null;

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => values.MustHaveSameCountAs(Array.Empty<string>());

        act.Should().Throw<ArgumentNullException>().WithParameterName(nameof(values));
    }

    [Fact]
    public static void NullComparisonCollectionUsesSecondaryArgumentNameWithoutObservingReceiver()
    {
        var values = new TrackingEnumerable<int>([1]);
        string[] other = null;

        // ReSharper disable once ExpressionIsAlwaysNull
        Action act = () => values.MustHaveSameCountAs(other);

        act.Should().Throw<ArgumentNullException>().WithParameterName("otherCollection");
        values.EnumeratorCount.Should().Be(0);
    }

    [Fact]
    public static void CustomMessageIsUsedForUnequalCounts()
    {
        Action act = () => new[] { 1 }.MustHaveSameCountAs(Array.Empty<int>(), message: "custom message");

        act.Should().Throw<InvalidCollectionCountException>().WithMessage("custom message*");
    }

    [Fact]
    public static void CustomMessageIsUsedForNullComparisonCollection()
    {
        Action act = () => Array.Empty<int>().MustHaveSameCountAs((string[]) null, message: "custom message");

        act.Should().Throw<ArgumentNullException>().WithMessage("custom message*");
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public static void CustomFactoryReceivesOriginalValuesForEveryFailure(bool receiverIsNull, bool otherIsNull)
    {
        int[] receiver = receiverIsNull ? null : [1, 2];
        string[] other = otherIsNull ? null : ["one"];
        var invocationCount = 0;
        var expectedException = new Exception();

        Action act = () => receiver.MustHaveSameCountAs(
            other,
            (actualReceiver, actualOther) =>
            {
                ++invocationCount;
                actualReceiver.Should().BeSameAs(receiver);
                actualOther.Should().BeSameAs(other);
                return expectedException;
            }
        );

        act.Should().Throw<Exception>().Which.Should().BeSameAs(expectedException);
        invocationCount.Should().Be(1);
    }

    [Fact]
    public static void CustomFactoryIsNotInvokedWhenComparingACollectionWithItself()
    {
        var collection = new TrackingEnumerable<int>([1, 2]);

        collection.MustHaveSameCountAs(
                       collection,
                       (_, _) => throw new InvalidOperationException("The factory must not be invoked.")
                   )
                  .Should().BeSameAs(collection);
        collection.EnumeratorCount.Should().Be(0);
    }

    [Fact]
    public static void CustomFactoryIsNotInvokedForEqualCounts()
    {
        var collection = new List<int> { 1, 2 };

        collection.MustHaveSameCountAs(
                       new[] { "one", "two" },
                       (_, _) => throw new InvalidOperationException("The factory must not be invoked.")
                   )
                  .Should().BeSameAs(collection);
    }

    [Fact]
    public static void NonGenericCollectionCountFastPathDoesNotEnumerateEitherInput()
    {
        var collection = new IndexableOnlyList<int>([1, 2]);
        var other = new IndexableOnlyList<string>(["one", "two"]);

        collection.MustHaveSameCountAs(other).Should().BeSameAs(collection);
    }

    [Fact]
    public static void StringLengthFastPathSupportsACollectionWithAnotherElementType()
    {
        const string collection = "text";

        collection.MustHaveSameCountAs(new[] { 1, 2, 3, 4 }).Should().BeSameAs(collection);
    }

    [Fact]
    public static void SameInstanceReturnsWithoutEnumeration()
    {
        var collection = new TrackingEnumerable<int>([1, 2]);

        collection.MustHaveSameCountAs(collection).Should().BeSameAs(collection);

        collection.EnumeratorCount.Should().Be(0);
    }

    [Fact]
    public static void DistinctLazySingleUseEnumerablesAreEnumeratedOnceAndDisposedOnSuccess()
    {
        var collection = new TrackingEnumerable<int>([1, 2]);
        var other = new TrackingEnumerable<string>(["one", "two"]);

        collection.MustHaveSameCountAs(other).Should().BeSameAs(collection);

        collection.EnumeratorCount.Should().Be(1);
        collection.DisposeCount.Should().Be(1);
        other.EnumeratorCount.Should().Be(1);
        other.DisposeCount.Should().Be(1);
    }

    [Fact]
    public static void FailureMessageDoesNotReenumerateLazySingleUseEnumerables()
    {
        var collection = new TrackingEnumerable<int>([1, 2]);
        var other = new TrackingEnumerable<string>(["one"]);

        Action act = () => collection.MustHaveSameCountAs(other);

        act.Should().Throw<InvalidCollectionCountException>();
        collection.EnumeratorCount.Should().Be(1);
        collection.DisposeCount.Should().Be(1);
        other.EnumeratorCount.Should().Be(1);
        other.DisposeCount.Should().Be(1);
    }
}
