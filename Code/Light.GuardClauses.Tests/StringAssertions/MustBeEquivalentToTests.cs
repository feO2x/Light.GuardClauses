using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustBeEquivalentToTests
    {
        [Theory]
        [InlineData("Foo", "foo", StringComparison.CurrentCulture)]
        [InlineData("Foo", "Bar", StringComparison.OrdinalIgnoreCase)]
        [InlineData(null, "Baz", StringComparison.OrdinalIgnoreCase)]
        [InlineData("Qux", null, StringComparison.OrdinalIgnoreCase)]
        public static void StringsNotEquivalent(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustBeEquivalentTo(second, comparisonType, nameof(first));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(first)} must be equivalent to {second.ToStringOrNull()} (using {comparisonType}), but it actually is {first.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData("Foo", "Foo")]
        [InlineData("Foo", "foo")]
        [InlineData("bar", "bAR")]
        [InlineData(null, null)]
        public static void StringsEquivalent(string first, string second) => first.MustBeEquivalentTo(second).Should().BeSameAs(first);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustBeEquivalentTo(y, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringException>(message => Metasyntactic.Baz.MustBeEquivalentTo(Metasyntactic.Qux, message: message));
    }
}