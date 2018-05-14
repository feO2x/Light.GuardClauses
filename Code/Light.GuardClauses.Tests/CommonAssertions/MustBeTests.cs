using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustBeTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar)]
        [InlineData(23, -699)]
        [InlineData(true, false)]
        [InlineData('c', 'a')]
        public static void ValuesNotEqual<T>(T x, T y)
        {
            Action act = () => x.MustBe(y, nameof(x));

            act.Should().Throw<ValuesNotEqualException>()
               .And.Message.Should().Contain($"{nameof(x)} must be equal to {y.ToStringOrNull()}, but it actually is {x.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData(Metasyntactic.Baz)]
        [InlineData(42)]
        [InlineData(true)]
        [InlineData('Y')]
        public static void ValuesEqual<T>(T value) => value.MustBe(value).Should().Be(value);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustBe(y, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValuesNotEqualException>(message => false.MustBe(true, message: message));

        [Fact]
        public static void ValuesNotEqualCustomEqualityComparer()
        {
            Action act = () => Metasyntactic.Foo.MustBe(Metasyntactic.Bar, new EqualityComparerStub<string>(false));

            act.Should().Throw<ValuesNotEqualException>()
               .And.Message.Should().Contain($"The value must be equal to {Metasyntactic.Bar.ToStringOrNull()}, but it actually is {Metasyntactic.Foo.ToStringOrNull()}.");
        }

        [Fact]
        public static void ValuesEqualCustomEqualityComparer() => 42.MustBe(42, new EqualityComparerStub<int>(true)).Should().Be(42);

        [Fact]
        public static void CustomExceptionEqualityComparer() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustBe(y, new EqualityComparerStub<string>(false), exceptionFactory));

        [Fact]
        public static void CustomMessageEqualityComparer() =>
            Test.CustomMessage<ValuesNotEqualException>(message => 99m.MustBe(100m, new EqualityComparerStub<decimal>(false), message: message));
    }
}