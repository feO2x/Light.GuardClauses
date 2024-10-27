using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustNotBeTests
{
    [Theory]
    [InlineData("Foo", "Foo", StringComparison.Ordinal)]
    [InlineData("Bar", "bar", StringComparison.OrdinalIgnoreCase)]
    [InlineData(null, null, StringComparison.CurrentCulture)]
    public static void ValuesEqual(string x, string y, StringComparison comparisonType)
    {
        Action act = () => x.MustNotBe(y, comparisonType, nameof(x));

        act.Should().Throw<ValuesEqualException>()
           .And.Message.Should().Contain($"{nameof(x)} must not be equal to {y.ToStringOrNull()}, but it actually is {x.ToStringOrNull()}");
    }

    [Theory]
    [InlineData("Foo", "Bar", StringComparison.CurrentCultureIgnoreCase)]
    [InlineData("Baz", "BAZ", StringComparison.CurrentCulture)]
    [InlineData(null, "Qux", StringComparison.Ordinal)]
    [InlineData("Quux", null, StringComparison.OrdinalIgnoreCase)]
    public static void ValuesNotEqual(string x, string y, StringComparison comparisonType) => x.MustNotBe(y, comparisonType).Should().Be(x);

    [Theory]
    [InlineData("Foo", "  Foo  ", StringComparisonType.OrdinalIgnoreWhiteSpace)]
    [InlineData("\tBar", "bar\r\n", StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
    [InlineData(null, null, StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
    public static void StringsEqualIgnoreWhiteSpace(string x, string y, StringComparisonType comparisonType)
    {
        Action act = () => x.MustNotBe(y, comparisonType, nameof(x));

        act.Should().Throw<ValuesEqualException>()
           .And.Message.Should().Contain($"{nameof(x)} must not be equal to {y.ToStringOrNull()}, but it actually is {x.ToStringOrNull()}");
    }

    [Theory]
    [InlineData("Foo", "Bar", StringComparisonType.OrdinalIgnoreWhiteSpace)]
    [InlineData("Baz", "", StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
    [InlineData(null, "Qux", StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace)]
    public static void StringsNotEqualIgnoreWhiteSpace(string x, string y, StringComparisonType comparisonType) => 
        x.MustNotBe(y, comparisonType).Should().BeSameAs(x);

    [Fact]
    public static void CustomException() =>
        Test.CustomException("Foo",
                             "Foo",
                             (x, y, exceptionFactory) => x.MustNotBe(y, StringComparison.CurrentCulture, exceptionFactory));

    [Fact]
    public static void CustomExceptionIgnoreWhiteSpace() => 
        Test.CustomException("Foo",
                             "  Foo",
                             StringComparisonType.OrdinalIgnoreWhiteSpace,
                             (x, y, ct, exceptionFactory) => x.MustNotBe(y, ct, exceptionFactory));

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ValuesEqualException>(message => "Foo".MustNotBe("Foo", StringComparison.CurrentCulture, message: message));

    [Fact]
    public static void CustomMessageIgnoreWhiteSpace() => 
        Test.CustomMessage<ValuesEqualException>(message => "Bar".MustNotBe("bar  ", StringComparisonType.OrdinalIgnoreCaseIgnoreWhiteSpace, message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string message = "Foo";

        var act = () => message.MustNotBe("Foo");

        act.Should().Throw<ValuesEqualException>()
           .WithParameterName(nameof(message));
    }
}