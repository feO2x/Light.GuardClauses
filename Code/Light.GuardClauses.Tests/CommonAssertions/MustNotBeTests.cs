using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeTests
    {
        [Theory]
        [InlineData(Metasyntactic.Qux)]
        [InlineData(155)]
        [InlineData(false)]
        [InlineData('/')]
        public static void ValuesEqual<T>(T value)
        {
            Action act = () => value.MustNotBe(value, nameof(value));

            act.Should().Throw<ValuesEqualException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be equal to {value.ToStringOrNull()}, but it actually is {value.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar)]
        [InlineData(23, -699)]
        [InlineData(true, false)]
        [InlineData('c', 'a')]
        public static void ValuesNotEqual<T>(T x, T y) => x.MustNotBe(y).Should().Be(x);


        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Foo,
                                 (x, y, exceptionFactory) => x.MustNotBe(y, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValuesEqualException>(message => false.MustNotBe(false, message: message));

        [Fact]
        public static void ValuesNotEqualCustomEqualityComparer()
        {
            Action act = () => Metasyntactic.Foo.MustNotBe(Metasyntactic.Foo, new EqualityComparerStub<string>(true));

            act.Should().Throw<ValuesEqualException>()
               .And.Message.Should().Contain($"The value must not be equal to {Metasyntactic.Foo.ToStringOrNull()}, but it actually is {Metasyntactic.Foo.ToStringOrNull()}.");
        }

        [Fact]
        public static void ValuesEqualCustomEqualityComparer() => 42.MustNotBe(47, new EqualityComparerStub<int>(false)).Should().Be(42);

        [Fact]
        public static void CustomExceptionEqualityComparer() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustNotBe(y, new EqualityComparerStub<string>(true), exceptionFactory));

        [Fact]
        public static void CustomMessageEqualityComparer() =>
            Test.CustomMessage<ValuesEqualException>(message => 50m.MustNotBe(50m, new EqualityComparerStub<decimal>(true), message: message));
    }
}