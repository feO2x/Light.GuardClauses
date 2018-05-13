using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions
{
    public static class MustNotBeEquivalentToTests
    {
        [Theory]
        [InlineData("Foo", "foo", StringComparison.CurrentCultureIgnoreCase)]
        [InlineData("Foo", "Foo", StringComparison.CurrentCulture)]
        [InlineData(null, null, StringComparison.OrdinalIgnoreCase)]
        public static void StringsEquivalent(string first, string second, StringComparison comparisonType)
        {
            Action act = () => first.MustNotBeEquivalentTo(second, comparisonType, nameof(first));

            act.Should().Throw<StringException>()
               .And.Message.Should().Contain($"{nameof(first)} must not be equivalent to {second.ToStringOrNull()} (using {comparisonType}), but it actually is {first.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData("Foo", "Bar")]
        [InlineData("Foo", "Baz")]
        [InlineData("Qux", null)]
        [InlineData(null, "Quux")]
        public static void StringsNotEquivalent(string first, string second) => first.MustNotBeEquivalentTo(second).Should().BeSameAs(first);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Foo,
                                 (x, y, exceptionFactory) => x.MustNotBeEquivalentTo(y, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<StringException>(message => Metasyntactic.Bar.MustNotBeEquivalentTo(Metasyntactic.Bar, message: message));
    }
}