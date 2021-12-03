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

            var exceptionAssertion = act.Should().Throw<ArgumentOutOfRangeException>().Which;
            exceptionAssertion.Message.Should().Contain($"{nameof(value)} must not be less than {boundary}, but it actually is {value}.");
            exceptionAssertion.ParamName.Should().BeSameAs(nameof(value));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(42, 42)]
        [InlineData(-87, -88)]
        public static void ParameterAtOrAboveBoundary(short value, short boundary) => value.MustNotBeLessThan(boundary, nameof(value)).Should().Be(value);

        [Fact]
        public static void ParameterNull()
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            Action act = () => ((string) null).MustNotBeLessThan("Foo", "Bar");

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().BeSameAs("Bar");
        }

        [Fact]
        public static void OtherStringNull() => "abc".MustNotBeLessThan(null).Should().Be("abc");

        [Fact]
        public static void CustomException() =>
            Test.CustomException(99, 100, (x, y, exceptionFactory) => x.MustNotBeLessThan(y, exceptionFactory));

        [Fact]
        public static void CustomExceptionParameterNull() =>
            Test.CustomException<string, string>(null, "abc", (x, y, exceptionFactory) => x.MustNotBeLessThan(y, exceptionFactory));

        [Fact]
        public static void CustomExceptionParameterValid() =>
            42.MustNotBeLessThan(40, (_, _) => null).Should().Be(42);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentOutOfRangeException>(message => 'a'.MustNotBeLessThan('b', message: message));

        [Fact]
        public static void CustomMessageParameterNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotBeLessThan("a", message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var seventeen = 17;

            Action act = () => seventeen.MustNotBeLessThan(20);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be(nameof(seventeen));
        }
    }
}