using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustNotBeLessThanTests
    {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(-87, 2)]
        public static void ParameterBelowBoundary(int value, int boundary)
        {
            Action act = () => value.MustNotBeLessThan(boundary, nameof(value));

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but it actually is {value}.");
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(42, 42)]
        [InlineData(-87, -88)]
        public static void ParameterAtOrAboveBoundary(short value, short boundary) => value.MustNotBeLessThan(boundary, nameof(value)).Should().Be(value);

        [Fact]
        public static void ParameterNull()
        {
            Action act = () => ((string) null).MustNotBeLessThan(Metasyntactic.Foo, Metasyntactic.Bar);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().BeSameAs(Metasyntactic.Bar);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(99, 100, (x, y, exceptionFactory) => x.MustNotBeLessThan(y, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 'a'.MustNotBeLessThan('b', message: message));

    }
}