using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class AdditionalSpanAndMemoryGuardsTests
{
    [Fact]
    public static void NonEmptyGuardsReturnEveryOriginalShape()
    {
        var array = new[] { 1, 2 };
        var span = array.AsSpan();
        ReadOnlySpan<int> readOnlySpan = array;
        var memory = array.AsMemory();
        ReadOnlyMemory<int> readOnlyMemory = array;

        (span.MustNotBeEmpty() == span).Should().BeTrue();
        (readOnlySpan.MustNotBeEmpty() == readOnlySpan).Should().BeTrue();
        memory.MustNotBeEmpty().Should().Be(memory);
        readOnlyMemory.MustNotBeEmpty().Should().Be(readOnlyMemory);
    }

    [Fact]
    public static void EmptyGuardsCaptureCallerExpressions()
    {
        var spanAct = () =>
        {
            var emptySpan = Span<int>.Empty;
            emptySpan.MustNotBeEmpty();
        };
        var readOnlySpanAct = () =>
        {
            var emptyReadOnlySpan = ReadOnlySpan<int>.Empty;
            emptyReadOnlySpan.MustNotBeEmpty();
        };
        var emptyMemory = Memory<int>.Empty;
        var emptyReadOnlyMemory = ReadOnlyMemory<int>.Empty;

        spanAct.Should().Throw<EmptyCollectionException>().WithParameterName("emptySpan");
        readOnlySpanAct.Should().Throw<EmptyCollectionException>().WithParameterName("emptyReadOnlySpan");
        ((Action) (() => emptyMemory.MustNotBeEmpty())).Should().Throw<EmptyCollectionException>()
                                                      .WithParameterName(nameof(emptyMemory));
        ((Action) (() => emptyReadOnlyMemory.MustNotBeEmpty())).Should().Throw<EmptyCollectionException>()
                                                              .WithParameterName(nameof(emptyReadOnlyMemory));
    }

    [Fact]
    public static void EmptyGuardFactoriesReceiveEveryShape()
    {
        Test.CustomSpanException(Span<int>.Empty, (value, factory) => value.MustNotBeEmpty(factory));
        Test.CustomSpanException(ReadOnlySpan<int>.Empty, (value, factory) => value.MustNotBeEmpty(factory));
        Test.CustomMemoryException(Memory<int>.Empty, (value, factory) => value.MustNotBeEmpty(factory));
        Test.CustomMemoryException(ReadOnlyMemory<int>.Empty, (value, factory) => value.MustNotBeEmpty(factory));
    }

    [Fact]
    public static void RangedLengthGuardsPreserveBoundariesAndShapes()
    {
        var array = new[] { 1, 2, 3 };
        var inclusive = Range.InclusiveBetween(3, 3);
        var exclusive = Range.FromExclusive(3).ToInclusive(4);
        var span = array.AsSpan();
        ReadOnlySpan<int> readOnlySpan = array;
        var memory = array.AsMemory();
        ReadOnlyMemory<int> readOnlyMemory = array;

        (span.MustHaveLengthIn(inclusive) == span).Should().BeTrue();
        (readOnlySpan.MustHaveLengthIn(inclusive) == readOnlySpan).Should().BeTrue();
        memory.MustHaveLengthIn(inclusive).Should().Be(memory);
        readOnlyMemory.MustHaveLengthIn(inclusive).Should().Be(readOnlyMemory);

        var act = () => memory.MustHaveLengthIn(exclusive);
        act.Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(memory))
           .WithMessage("*actually has length 3*");
    }

    [Fact]
    public static void RangedLengthFactoriesReceiveEveryShape()
    {
        var invalidRange = Range.InclusiveBetween(2, 3);
        Test.CustomSpanException(Span<int>.Empty, invalidRange,
            (value, range, factory) => value.MustHaveLengthIn(range, factory));
        Test.CustomSpanException(ReadOnlySpan<int>.Empty, invalidRange,
            (value, range, factory) => value.MustHaveLengthIn(range, factory));
        Test.CustomMemoryException(Memory<int>.Empty, invalidRange,
            (value, range, factory) => value.MustHaveLengthIn(range, factory));
        Test.CustomMemoryException(ReadOnlyMemory<int>.Empty, invalidRange,
            (value, range, factory) => value.MustHaveLengthIn(range, factory));
    }

    [Fact]
    public static void MemoryExactLengthGuardsReturnOriginalShapes()
    {
        var memory = new[] { 1, 2 }.AsMemory();
        ReadOnlyMemory<int> readOnlyMemory = memory;

        memory.MustHaveLength(2).Should().Be(memory);
        readOnlyMemory.MustHaveLength(2).Should().Be(readOnlyMemory);
    }

    [Fact]
    public static void MemoryExactLengthFailuresSupportMessagesExpressionsAndFactories()
    {
        var memory = Memory<int>.Empty;
        ReadOnlyMemory<int> readOnlyMemory = memory;

        ((Action) (() => memory.MustHaveLength(1, message: "custom")))
           .Should().Throw<InvalidCollectionCountException>()
           .WithParameterName(nameof(memory))
           .WithMessage("*custom*");
        Test.CustomMemoryException(memory, 1, (value, length, factory) => value.MustHaveLength(length, factory));
        Test.CustomMemoryException(readOnlyMemory, 1,
            (value, length, factory) => value.MustHaveLength(length, factory));
    }

    [Fact]
    public static void NonWhitespaceGuardsReturnEveryOriginalShape()
    {
        var characters = " a".ToCharArray();
        var span = characters.AsSpan();
        ReadOnlySpan<char> readOnlySpan = characters;
        var memory = characters.AsMemory();
        ReadOnlyMemory<char> readOnlyMemory = characters;

        (span.MustNotBeEmptyOrWhiteSpace() == span).Should().BeTrue();
        (readOnlySpan.MustNotBeEmptyOrWhiteSpace() == readOnlySpan).Should().BeTrue();
        memory.MustNotBeEmptyOrWhiteSpace().Should().Be(memory);
        readOnlyMemory.MustNotBeEmptyOrWhiteSpace().Should().Be(readOnlyMemory);
    }

    [Fact]
    public static void EmptyAndWhitespaceFailuresUseExistingStringExceptions()
    {
        var emptyAct = () =>
        {
            var emptyCharacters = ReadOnlySpan<char>.Empty;
            emptyCharacters.MustNotBeEmptyOrWhiteSpace();
        };
        var whiteSpaceMemory = " \t".ToCharArray().AsMemory();

        emptyAct.Should().Throw<EmptyStringException>().WithParameterName("emptyCharacters");
        ((Action) (() => whiteSpaceMemory.MustNotBeEmptyOrWhiteSpace()))
           .Should().Throw<WhiteSpaceStringException>()
           .WithParameterName(nameof(whiteSpaceMemory))
           .WithMessage("*actually has length 2*");
        ((Action) (() => whiteSpaceMemory.MustNotBeEmptyOrWhiteSpace(message: "custom")))
           .Should().Throw<WhiteSpaceStringException>()
           .WithParameterName(nameof(whiteSpaceMemory))
           .WithMessage("*custom*");
    }

    [Fact]
    public static void WhitespaceFactoriesReceiveEveryShape()
    {
        Test.CustomSpanException(" ".ToCharArray().AsSpan(),
            (value, factory) => value.MustNotBeEmptyOrWhiteSpace(factory));
        Test.CustomSpanException((ReadOnlySpan<char>) " ".ToCharArray(),
            (value, factory) => value.MustNotBeEmptyOrWhiteSpace(factory));
        Test.CustomMemoryException(" ".ToCharArray().AsMemory(),
            (value, factory) => value.MustNotBeEmptyOrWhiteSpace(factory));
        Test.CustomMemoryException((ReadOnlyMemory<char>) " ".ToCharArray(),
            (value, factory) => value.MustNotBeEmptyOrWhiteSpace(factory));
    }
}
