using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustEndWithTests
{
    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData("When you play the game of thrones you win, or you die.", "you live")]
    public static void DoesNotEndWith(string x, string y)
    {
        var act = () => x.MustEndWith(y);

        var exception = act.Should().Throw<SubstringException>().Which;
        exception.Message.Should().StartWith($"{nameof(x)} must end with \"{y}\" (CurrentCulture), but it actually is \"{x}\".");
        exception.ParamName.Should().BeSameAs(nameof(x));
    }
    
    [Theory]
    [InlineData("FooBar", "Bar", StringComparison.Ordinal)]
    [InlineData("12345678", "5678", StringComparison.InvariantCultureIgnoreCase)]
    public static void EndsWith(string x, string y, StringComparison comparisonType) =>
        x.MustEndWith(y, comparisonType).Should().BeSameAs(x);

    [Fact]
    public static void ParameterNull()
    {
        var act = () => ((string) null).MustEndWith("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void ValueNull()
    {
        var act = () => "Foo".MustEndWith(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<SubstringException>(message => "Foo".MustEndWith("Bar", message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustEndWith("Bar", message: message));

    [Fact]
    public static void EndsWithCustomException() =>
        "Foo".MustEndWith("o", (_, _) => new Exception()).Should().Be("Foo");

    [Fact]
    public static void EndsWithCustomExceptionAndComparisonType() =>
        "Foo".MustEndWith("O", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception()).Should().Be("Foo");

    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData("Foo", null)]
    [InlineData(null, "Baz")]
    public static void CustomException(string first, string second) =>
        Test.CustomException(first, second, (s1, s2, exceptionFactory) => s1.MustEndWith(s2, exceptionFactory));

    [Theory]
    [InlineData("Foo", "Bar", StringComparison.Ordinal)]
    [InlineData(null, "Bar", StringComparison.Ordinal)]
    [InlineData("Baz", null, StringComparison.Ordinal)]
    public static void CustomExceptionWithComparisonType(string a, string b, StringComparison comparisonType) =>
        Test.CustomException(a, b, comparisonType, (s1, s2, ct, exceptionFactory) => s1.MustEndWith(s2, ct, exceptionFactory));
}
