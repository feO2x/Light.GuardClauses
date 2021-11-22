using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class SpanMustBeShorterThanTests
    {
        [Theory]
        [InlineData(2, 4)]
        [InlineData(0, 1)]
        [InlineData(10, 18)]
        public static void SpanShorter(int spanLength, int expectedLength)
        {
            var array = new int[10];
            var span = new Span<int>(array, 0, spanLength);

            var returnValue = span.MustBeShorterThan(expectedLength);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Theory]
        [InlineData(10, 7)]
        [InlineData(1, 0)]
        [InlineData(5, 5)]
        public static void SpanLongerOrEqual(int spanLength, int expectedLength)
        {
            var act = () =>
            {
                var array = new char[15];
                var span = new Span<char>(array, 0, spanLength);
                span.MustBeShorterThan(expectedLength, nameof(span));
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"span must be shorter than {expectedLength}, but it actually has length {spanLength}.");
        }

        [Fact]
        public static void CustomException()
        {
            var exception = new Exception();

            var act = () =>
            {
                var span = new Span<byte>();
                span.MustBeShorterThan(0, (_, _) => exception);
            };

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public static void NoCustomException()
        {
            var span = new Span<char>();

            var returnValue = span.MustBeShorterThan(5, null!);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Fact]
        public static void CustomMessage()
        {
            var act = () =>
            {
                var span = new Span<byte>();
                span.MustBeShorterThan(0, null, "Custom exception message");
            };

            act.Should().Throw<InvalidCollectionCountException>().And.Message.Should().Be("Custom exception message");
        }

        [Fact]
        public static void CallerArgumentExpression()
        {
            var act = () =>
            {
                var span = new Span<byte>(new byte[3]);
                span.MustBeShorterThan(2);
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.ParamName.Should().Be("span");
        }
    }
}