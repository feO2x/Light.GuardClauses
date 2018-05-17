using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustNotBeTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Foo, StringComparison.Ordinal)]
        [InlineData("Bar", "bar", StringComparison.OrdinalIgnoreCase)]
        [InlineData(null, null, StringComparison.CurrentCulture)]
        public static void ValuesEqual(string x, string y, StringComparison comparisonType)
        {
            Action act = () => x.MustNotBe(y, comparisonType, nameof(x));

            act.Should().Throw<ValuesEqualException>()
               .And.Message.Should().Contain($"{nameof(x)} must not be equal to {y.ToStringOrNull()}, but it actually is {x.ToStringOrNull()}");
        }

        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar, StringComparison.CurrentCultureIgnoreCase)]
        [InlineData("Baz", "BAZ", StringComparison.CurrentCulture)]
        [InlineData(null, Metasyntactic.Qux, StringComparison.Ordinal)]
        [InlineData(Metasyntactic.Quux, null, StringComparison.OrdinalIgnoreCase)]
        public static void ValuesNotEqual(string x, string y, StringComparison comparisonType) => x.MustNotBe(y, comparisonType).Should().Be(x);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Foo,
                                 (x, y, exceptionFactory) => x.MustNotBe(y, StringComparison.CurrentCulture, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValuesEqualException>(message => Metasyntactic.Foo.MustNotBe(Metasyntactic.Foo, StringComparison.CurrentCulture, message: message));
    }
}