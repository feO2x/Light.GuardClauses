using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustStartWithTests
{
    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData("When you play the game of thrones you win, or you die. There is no middle ground.", "Love and peace")]
    public static void DoesNotStartWith(string x, string y)
    {
        var act = () => x.MustStartWith(y);

        var exception = act.Should().Throw<SubstringException>().Which;
        exception.Message.Should().StartWith($"{nameof(x)} must start with \"{y}\" (CurrentCulture), but it actually is \"{x}\".");
        exception.ParamName.Should().BeSameAs(nameof(x));
    }
    
    [Theory]
    [InlineData("FooBar", "Foo", StringComparison.Ordinal)]
    [InlineData("12345678", "1234", StringComparison.InvariantCultureIgnoreCase)]
    public static void StartsWith(string x, string y, StringComparison comparisonType) =>
        x.MustStartWith(y, comparisonType).Should().BeSameAs(x);

    [Fact]
    public static void ParameterNull()
    {
        var act = () => ((string) null).MustStartWith("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void ValueNull()
    {
        var act = () => "Foo".MustStartWith(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<SubstringException>(message => "Foo".MustStartWith("Bar", message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustStartWith("Bar", message: message));

    [Fact]
    public static void StartsWithCustomException() =>
        "Foo".MustStartWith("F", (_, _) => new Exception()).Should().Be("Foo");

    [Fact]
    public static void StartsWithCustomExceptionAndComparisonType() =>
        "Foo".MustStartWith("f", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception()).Should().Be("Foo");

    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData("Foo", null)]
    [InlineData(null, "Baz")]
    public static void CustomException(string first, string second) =>
        Test.CustomException(first, second, (s1, s2, exceptionFactory) => s1.MustStartWith(s2, exceptionFactory));

    [Theory]
    [InlineData("Foo", "Bar", StringComparison.Ordinal)]
    [InlineData(null, "Bar", StringComparison.Ordinal)]
    [InlineData("Baz", null, StringComparison.Ordinal)]
    public static void CustomExceptionWithComparisonType(string a, string b, StringComparison comparisonType) =>
        Test.CustomException(a, b, comparisonType, (s1, s2, ct, exceptionFactory) => s1.MustStartWith(s2, ct, exceptionFactory));
}