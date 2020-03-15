using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class ReadOnlySpanMustBeShorterThanOrEqualToTests
    {
        [Theory]
        [InlineData(45, 68)]
        [InlineData(0, 1)]
        [InlineData(0, 0)]
        [InlineData(87, 87)]
        public static void ShorterThanOrEqualTo(int spanLength, int expectedLength)
        {
            var array = new byte[128];
            var span = new ReadOnlySpan<byte>(array, 0, spanLength);

            var returnValue = span.MustBeShorterThanOrEqualTo(expectedLength);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Theory]
        [InlineData(7, 5)]
        [InlineData(10, 9)]
        [InlineData(1, 0)]
        public static void LongerThan(int spanLength, int expectedLength)
        {
            Action act = () =>
            {
                var array = new int[10];
                var span = new ReadOnlySpan<int>(array, 0, spanLength);
                span.MustBeShorterThanOrEqualTo(expectedLength, nameof(span));
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"span must be shorter than or equal to {expectedLength}, but it actually has length {spanLength}.");
        }

        [Fact]
        public static void CustomException()
        {
            var exception = new Exception();

            Action act = () =>
            {
                var span = new ReadOnlySpan<byte>(new byte[10]);
                span.MustBeShorterThanOrEqualTo(5, (s, l) => exception);
            };

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public static void NoCustomException()
        {
            var span = new ReadOnlySpan<char>(new char[4]);

            var returnValue = span.MustBeShorterThanOrEqualTo(5, null);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Fact]
        public static void CustomMessage()
        {
            Action act = () =>
            {
                var span = new ReadOnlySpan<int>(new int[10]);
                span.MustBeShorterThanOrEqualTo(5, message: "Custom exception message");
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Be("Custom exception message");
        }
    }
}