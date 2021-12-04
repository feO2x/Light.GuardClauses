using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class SpanMustBeLongerThanTests
{
    [Theory]
    [InlineData(4, 3)]
    [InlineData(8, 4)]
    [InlineData(1, 0)]
    public static void SpanLonger(int spanLength, int expectedLength)
    {
        var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var span = new Span<int>(array, 0, spanLength);

        var returnValue = span.MustBeLongerThan(expectedLength);

        (returnValue == span).Should().BeTrue("the assertion returns a copy of the original span.");
    }

    [Theory]
    [InlineData(3, 4)]
    [InlineData(0, 1)]
    [InlineData(7, 12)]
    public static void SpanShorterOrEqual(int spanLength, int expectedLength)
    {
        var act = () =>
        {
            var array = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            var span = new Span<char>(array, 0, spanLength);
            span.MustBeLongerThan(expectedLength, nameof(span));
        };

        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Contain($"span must be longer than {expectedLength}, but it actually has length {spanLength}.");
    }

    [Fact]
    public static void CustomException()
    {
        var exception = new Exception();
        var act = () =>
        {
            var span = new Span<byte>();
            span.MustBeLongerThan(2, (_, _) => exception);
        };

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void NoCustomException()
    {
        var array = new int[4];
        var span = new Span<int>(array);

        var returnValue = span.MustBeLongerThan(3, null!);

        (returnValue == span).Should().BeTrue("the assertion returns a copy of the original span.");
    }

    [Fact]
    public static void CustomMessage()
    {
        var act = () =>
        {
            var span = new Span<byte>();
            span.MustBeLongerThan(5, null, "Custom exception message");
        };

        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Be("Custom exception message");
    }

    [Fact]
    public static void CallerArgumentExpression()
    {
        var act = () =>
        {
            var mySpan = new Span<byte>();
            mySpan.MustBeLongerThan(3);
        };

        act.Should().Throw<InvalidCollectionCountException>()
           .And.ParamName.Should().Be("mySpan");
    }
}