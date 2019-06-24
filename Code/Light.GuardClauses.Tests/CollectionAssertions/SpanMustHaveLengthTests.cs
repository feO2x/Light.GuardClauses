#if (NETCOREAPP2_2 || NET47)
using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class SpanMustHaveLengthTests
    {
        [Theory]
        [InlineData(3, 2)]
        [InlineData(5, 6)]
        [InlineData(0, 1)]
        [InlineData(10, 11)]
        public static void InvalidLength(int spanLength, int expectedLength)
        {
            Action act = () =>
            {
                var array = new int[10];
                var span = new Span<int>(array, 0, spanLength);
                span.MustHaveLength(expectedLength, nameof(span));
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"span must have length {expectedLength}, but it actually has length {spanLength}.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(0)]
        public static void ValidLength(int spanLength)
        {
            var array = new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
            var span = new Span<string>(array, 0, spanLength);
            var result = span.MustHaveLength(spanLength);
            (result == span).Should().BeTrue("The returned span must be a copy of the passed-in instance.");
        }

        [Fact]
        public static void CustomException()
        {
            var exception = new Exception();

            Action act = () =>
            {
                var array = new char[5];
                var span = new Span<char>(array, 0, 3);
                span.MustHaveLength(4, (s, l) => exception);
            };

            act.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public static void NoCustomException()
        {
            var array = new int[4];
            var span = new Span<int>(array);

            var result = span.MustHaveLength(4, (s, l) => null);

            (result == span).Should().BeTrue("The returned span must be a copy of the passed-in instance.");
        }

        [Fact]
        public static void CustomMessage()
        {
            Action act = () =>
            {
                var span = new Span<string>();
                span.MustHaveLength(1, message: "Foo");
            };

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain("Foo");
        }
    }
}
#endif