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

        [Theory]
        [InlineData("Foo", "  foo  ", StringComparisonType.OrdinalIgnoreWhiteSpace)]
        [InlineData("Bar", "Baz", StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
        [InlineData("\tFoo", null, StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
        [InlineData(null, "Qux\r\n", StringComparisonType.OrdinalIgnoreWhiteSpace)]
        public static void StringsNotEqualIgnoreWhiteSpace(string first, string second, StringComparisonType comparisonType)
        {
            Action act = () => first.MustBe(second, comparisonType, nameof(first));

            act.Should().Throw<ValuesNotEqualException>()
               .And.Message.Should().Contain($"{nameof(first)} must be equal to {second.ToStringOrNull()}, but it actually is {first.ToStringOrNull()}.");
        }

        [Theory]
        [InlineData("Foo", "\tfoo", StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
        [InlineData("Bar", "  Bar", StringComparisonType.OrdinalIgnoreWhiteSpace)]
        [InlineData(null, null, StringComparisonType.OrdinalIgnoreWhiteSpace)]
        public static void StringsEqualIgnoreWhiteSpace(string x, string y, StringComparisonType comparisonType) => x.MustBe(y, comparisonType).Should().BeSameAs(x);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustBe(y, StringComparison.Ordinal, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValuesNotEqualException>(message => Metasyntactic.Foo.MustBe(Metasyntactic.Bar, StringComparison.Ordinal, message: message));

        [Fact]
        public static void CustomExceptionIgnoreWhiteSpace() => 
            Test.CustomException("Foo",
                                 "   foo",
                                 (x, y, exceptionFactory) => x.MustBe(y, StringComparisonType.OrdinalIgnoreWhiteSpace, exceptionFactory));

        [Fact]
        public static void CustomMessageIgnoreWhiteSpace() => 
            Test.CustomMessage<ValuesNotEqualException>(message => "foo".MustBe("bar", StringComparisonType.Ordinal, message: message));
    }
}