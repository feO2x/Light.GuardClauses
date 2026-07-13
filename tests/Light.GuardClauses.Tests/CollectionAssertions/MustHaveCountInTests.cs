using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class MustHaveCountInTests
{
    [Fact]
    public static void InclusiveBoundariesAreAccepted()
    {
        var collection = new[] { 1, 2, 3 };

        collection.MustHaveCountIn(Range.InclusiveBetween(3, 3)).Should().BeSameAs(collection);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public static void ExclusiveBoundariesArePreserved(bool lowerInclusive, bool upperInclusive)
    {
        var collection = new[] { 1, 2, 3 };
        var range = new Range<int>(3, 4, lowerInclusive, upperInclusive);

        var act = () => collection.MustHaveCountIn(range);

        if (lowerInclusive)
        {
            act.Should().NotThrow();
        }
        else
        {
            act.Should().Throw<InvalidCollectionCountException>();
        }
    }

    [Fact]
    public static void DefaultFailureReportsActualCountRangeAndExpression()
    {
        var collection = new[] { 1, 2, 3 };
        var range = Range.InclusiveBetween(4, 6);

        var act = () => collection.MustHaveCountIn(range);

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(collection))
           .WithMessage("*actually has count 3*");
    }

    [Fact]
    public static void ThrowHelperCapturesCollectionExpression()
    {
        var collection = new[] { 1, 2, 3 };

        var act = () => Throw.CollectionCountNotInRange(
            collection,
            collection.Length,
            Range.InclusiveBetween(4, 6)
        );

        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(collection));
    }

    [Fact]
    public static void NullUsesExistingNullBehavior()
    {
        List<int> collection = null;

        // ReSharper disable once ExpressionIsAlwaysNull - required for the test
        var act = () => collection.MustHaveCountIn(Range.InclusiveBetween(0, 1));

        act.Should().Throw<ArgumentNullException>().WithParameterName(nameof(collection));
    }

    [Fact]
    public static void LazyEnumerableIsEnumeratedOnceOnSuccessAndFailure()
    {
        var enumerationCount = 0;
        IEnumerable<int> Values()
        {
            enumerationCount++;
            yield return 1;
            yield return 2;
        }

        Values().MustHaveCountIn(Range.InclusiveBetween(2, 2));
        enumerationCount.Should().Be(1);

        var act = () => Values().MustHaveCountIn(Range.InclusiveBetween(3, 4));
        act.Should().Throw<InvalidCollectionCountException>();
        enumerationCount.Should().Be(2);
    }

    [Fact]
    public static void FactoryOverloadEnumeratesLazyEnumerableOnceOnSuccessAndFailure()
    {
        var enumerationCount = 0;
        IEnumerable<int> Values()
        {
            enumerationCount++;
            yield return 1;
            yield return 2;
        }

        Values().MustHaveCountIn(
            Range.InclusiveBetween(2, 2),
            (_, _) => new InvalidOperationException()
        );
        enumerationCount.Should().Be(1);

        var act = () => Values().MustHaveCountIn(
            Range.InclusiveBetween(3, 4),
            (_, _) => new InvalidOperationException()
        );
        act.Should().Throw<InvalidOperationException>();
        enumerationCount.Should().Be(2);
    }

    [Fact]
    public static void CustomFactoryReceivesCollectionAndRange()
    {
        var collection = new List<int> { 1 };
        var range = Range.InclusiveBetween(2, 3);

        Test.CustomException(collection, range, (value, assertedRange, factory) =>
            value.MustHaveCountIn(assertedRange, factory));
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<InvalidCollectionCountException>(message =>
            new[] { 1 }.MustHaveCountIn(Range.InclusiveBetween(2, 3), message: message));
}
