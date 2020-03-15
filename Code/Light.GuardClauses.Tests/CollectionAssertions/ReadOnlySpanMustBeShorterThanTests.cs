using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class ReadOnlySpanMustBeShorterThanTests
    {
        [Theory]
        [InlineData(1, 4)]
        [InlineData(0, 1)]
        [InlineData(9, 14)]
        public static void SpanShorter(int spanLength, int expectedLength)
        {
            var array = new int[10];
            var span = new ReadOnlySpan<int>(array, 0, spanLength);

            var returnValue = span.MustBeShorterThan(expectedLength);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Theory]
        [InlineData(11, 6)]
        [InlineData(1, 0)]
        [InlineData(7, 7)]
        public static void SpanLongerOrEqual(int spanLength, int expectedLength)
        {
            Action act = () =>
            {
                var array = new char[15];
                var span = new ReadOnlySpan<char>(array, 0, spanLength);
                span.MustBeShorterThan(expectedLength, nameof(span));
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"span must be shorter than {expectedLength}, but it actually has length {spanLength}.");
        }

        [Fact]
        public static void CustomException()
        {
            var exception = new Exception();

            Action act = () =>
            {
                var span = new ReadOnlySpan<byte>();
                span.MustBeShorterThan(0, (s, l) => exception);
            };

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public static void NoCustomException()
        {
            var span = new ReadOnlySpan<char>();

            var returnValue = span.MustBeShorterThan(5, null);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Fact]
        public static void CustomMessage()
        {
            Action act = () =>
            {
                var span = new ReadOnlySpan<byte>();
                span.MustBeShorterThan(0, message: "Custom exception message");
            };

            act.Should().Throw<InvalidCollectionCountException>().And.Message.Should().Be("Custom exception message");
        }
    }
}