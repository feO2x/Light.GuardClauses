using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustNotStartWithTests
{
    [Theory]
    [InlineData("Bar", "Foo")]
    [InlineData("When you play the game of thrones you win, or you die.", "Love and peace")]
    public static void DoesNotStartWith(string x, string y) =>
        x.MustNotStartWith(y).Should().BeSameAs(x);

    [Theory]
    [InlineData("FooBar", "Foo")]
    [InlineData("12345678", "1234")]
    public static void StartsWith(string x, string y)
    {
        var act = () => x.MustNotStartWith(y);

        var exception = act.Should().Throw<SubstringException>().Which;
        exception.Message.Should().StartWith($"{nameof(x)} must not start with \"{y}\" (CurrentCulture), but it actually is \"{x}\"");
        exception.ParamName.Should().BeSameAs(nameof(x));
    }

    [Theory]
    [InlineData("FooBar", "Foo", StringComparison.Ordinal)]
    [InlineData("12345678", "1234", StringComparison.InvariantCultureIgnoreCase)]
    public static void StartsWithComparisonType(string x, string y, StringComparison comparisonType)
    {
        var act = () => x.MustNotStartWith(y, comparisonType);

        var exception = act.Should().Throw<SubstringException>().Which;
        exception.Message.Should().StartWith($"{nameof(x)} must not start with \"{y}\" ({comparisonType}), but it actually is \"{x}\"");
        exception.ParamName.Should().BeSameAs(nameof(x));
    }

    [Fact]
    public static void ParameterNull()
    {
        var act = () => ((string)null).MustNotStartWith("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void ValueNull()
    {
        var act = () => "Foo".MustNotStartWith(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<SubstringException>(message => "FooBar".MustNotStartWith("Foo", message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string)null).MustNotStartWith("Bar", message: message));

    [Fact]
    public static void NotStartsWithCustomException() =>
        "Bar".MustNotStartWith("Foo", (_, _) => new Exception()).Should().Be("Bar");

    [Fact]
    public static void NotStartsWithCustomExceptionAndComparisonType() =>
        "Bar".MustNotStartWith("foo", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception()).Should().Be("Bar");

    [Theory]
    [InlineData("Foo", "Foo")]
    [InlineData("Foo", null)]
    [InlineData(null, "Baz")]
    public static void CustomException(string first, string second) =>
        Test.CustomException(first, second, (s1, s2, exceptionFactory) => s1.MustNotStartWith(s2, exceptionFactory));

    [Theory]
    [InlineData("Foo", "foo", StringComparison.OrdinalIgnoreCase)]
    [InlineData(null, "Bar", StringComparison.Ordinal)]
    [InlineData("Baz", null, StringComparison.Ordinal)]
    public static void CustomExceptionWithComparisonType(string a, string b, StringComparison comparisonType) =>
        Test.CustomException(a, b, comparisonType, (s1, s2, ct, exceptionFactory) => s1.MustNotStartWith(s2, ct, exceptionFactory));
}
