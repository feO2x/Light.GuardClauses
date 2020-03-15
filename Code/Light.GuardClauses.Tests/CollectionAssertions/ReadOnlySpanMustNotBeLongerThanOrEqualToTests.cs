using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using System;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class ReadOnlySpanMustNotBeLongerThanOrEqualToTests
    {
        [Theory]
        [InlineData(76, 29)]
        [InlineData(10, 10)]
        [InlineData(0, 0)]
        [InlineData(128, 127)]
        public static void LongerThanOrEqualTo(int spanLength, int expectedLength)
        {
            var array = new byte[128];
            var span = new ReadOnlySpan<byte>(array, 0, spanLength);

            var returnValue = span.MustBeLongerThanOrEqualTo(expectedLength);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Theory]
        [InlineData(6, 9)]
        [InlineData(1, 3)]
        [InlineData(0, 4)]
        public static void ShorterThan(int spanLength, int expectedLength)
        {
            Action act = () =>
            {
                var array = new int[10];
                var span = new ReadOnlySpan<int>(array, 0, spanLength);
                span.MustBeLongerThanOrEqualTo(expectedLength, nameof(span));
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"span must be longer than or equal to {expectedLength}, but it actually has length {spanLength}.");
        }

        [Fact]
        public static void CustomException()
        {
            var exception = new Exception();

            Action act = () =>
            {
                var array = new char[5];
                var span = new ReadOnlySpan<char>(array, 1, 3);
                span.MustBeLongerThanOrEqualTo(10, (s, l) => exception);
            };

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public static void NoCustomException()
        {
            var span = new ReadOnlySpan<byte>();

            var returnValue = span.MustBeLongerThanOrEqualTo(0, null);

            (returnValue == span).Should().BeTrue("the assertion returns a copy of the passed-in span");
        }

        [Fact]
        public static void CustomMessage()
        {
            Action act = () =>
            {
                var span = new ReadOnlySpan<int>();

                span.MustBeLongerThanOrEqualTo(15, message: "Custom exception message");
            };


            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Be("Custom exception message");
        }
    }
}