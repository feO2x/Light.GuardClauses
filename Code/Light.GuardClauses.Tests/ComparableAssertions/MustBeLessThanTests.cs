using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.ComparableAssertions
{
    public static class MustBeLessThanTests
    {
        [Theory]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(short.MaxValue, short.MinValue)]
        [InlineData('Z', 'Z')]
        public static void ParameterNotLess(int first, int second)
        {
            Action act = () => first.MustBeLessThan(second, nameof(first));

            var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
            exceptionAssertion.Message.Should().Contain($"{nameof(first)} must be less than {second}, but it actually is {first}.");
            exceptionAssertion.ParamName.Should().BeSameAs(nameof(first));
        }

        [Theory]
        [InlineData(1L, 2L)]
        [InlineData(-444L, -410L)]
        [InlineData(long.MinValue, long.MinValue + 14)]
        [InlineData('A', 'a')]
        public static void ParameterLess(long first, long second) => first.MustBeLessThan(second).Should().Be(first);

        [Fact]
        public static void CustomException() =>
            Test.CustomException("Z", "A", (z, a, exceptionFactory) => z.MustBeLessThan(a, exceptionFactory));

        [Fact]
        public static void CustomExceptionParameterNull() => 
            Test.CustomException((string) null,
                                 Metasyntactic.Foo,
                                 (x, y, exceptionFactory) => x.MustBeLessThan(y, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() => 5m.MustBeLessThan(5.1m, (v, b) => null).Should().Be(5m);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 40.MustBeLessThan(10, message: message));

        [Fact]
        public static void CustomMessageParameterNull() => 
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustBeLessThan(Metasyntactic.Bar, message: message));
    }
}