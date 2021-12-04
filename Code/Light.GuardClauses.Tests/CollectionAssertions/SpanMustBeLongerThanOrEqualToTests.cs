using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions;

public static class SpanMustBeLongerThanOrEqualToTests
{
    [Theory]
    [InlineData(30, 20)]
    [InlineData(5, 5)]
    [InlineData(0, 0)]
    [InlineData(128, 127)]
    public static void LongerThanOrEqualTo(int spanLength, int expectedLength)
    {
        var array = new byte[128];
        var span = new Span<byte>(array, 0, spanLength);

        var returnValue = span.MustBeLongerThanOrEqualTo(expectedLength);

        (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
    }

    [Theory]
    [InlineData(8, 9)]
    [InlineData(2, 3)]
    [InlineData(0, 6)]
    public static void ShorterThan(int spanLength, int expectedLength)
    {
        var act = () =>
        {
            var array = new int[10];
            var span = new Span<int>(array, 0, spanLength);
            span.MustBeLongerThanOrEqualTo(expectedLength, nameof(span));
        };

        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Contain($"span must be longer than or equal to {expectedLength}, but it actually has length {spanLength}.");
    }

    [Fact]
    public static void CustomException()
    {
        var exception = new Exception();

        var act = () =>
        {
            var array = new char[5];
            var span = new Span<char>(array, 1, 3);
            span.MustBeLongerThanOrEqualTo(10, (_, _) => exception);
        };

        act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public static void NoCustomException()
    {
        var span = new Span<byte>();

        var returnValue = span.MustBeLongerThanOrEqualTo(0, null!);

        (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
    }

    [Fact]
    public static void CustomMessage()
    {
        var act = () =>
        {
            var span = new Span<int>();

            span.MustBeLongerThanOrEqualTo(15, null, "Custom exception message");
        };


        act.Should().Throw<InvalidCollectionCountException>()
           .And.Message.Should().Be("Custom exception message");
    }

    [Fact]
    public static void CallerArgumentExpression()
    {
        var act = () =>
        {
            var mySpan = new Span<char>();
            mySpan.MustBeLongerThanOrEqualTo(15);
        };

        act.Should().Throw<InvalidCollectionCountException>()
           .And.ParamName.Should().Be("mySpan");
    }
}