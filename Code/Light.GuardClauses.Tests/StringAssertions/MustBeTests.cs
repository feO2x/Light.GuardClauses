using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar, StringComparison.OrdinalIgnoreCase)]
        [InlineData("Foo", "foo", StringComparison.Ordinal)]
        [InlineData(Metasyntactic.Baz, null, StringComparison.CurrentCulture)]
        [InlineData(null, Metasyntactic.Qux, StringComparison.OrdinalIgnoreCase)]
        public static void StringsNotEqual(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustBe(second, comparisonType, nameof(first));

            act.Should().Throw<ValuesNotEqualException>()
               .And.Message.Should().Contain($"{nameof(first)} must be equal to {second.ToStringOrNull()}, but it actually is {first.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Foo, StringComparison.Ordinal)]
        [InlineData("Foo", "foo", StringComparison.OrdinalIgnoreCase)]
        [InlineData(null, null, StringComparison.CurrentCulture)]
        public static void StringsEqual(string first, string second, StringComparison comparisonType) => first.MustBe(second, comparisonType).Should().Be(first);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustBe(y, StringComparison.Ordinal, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValuesNotEqualException>(message => Metasyntactic.Foo.MustBe(Metasyntactic.Bar, StringComparison.Ordinal, message: message));
    }
}