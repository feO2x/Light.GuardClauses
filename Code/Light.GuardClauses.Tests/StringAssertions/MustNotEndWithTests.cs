using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustNotEndWithTests
{
    [Theory]
    [InlineData("FooBar", "Bar")]
    [InlineData("When you play the game of thrones you win, or you die.", "die.")]
    public static void DoesEndWith(string x, string y)
    {
        var act = () => x.MustNotEndWith(y);

        var exception = act.Should().Throw<SubstringException>().Which;
        exception.Message.Should().StartWith($"{nameof(x)} must not end with \"{y}\" (CurrentCulture), but it actually is \"{x}\".");
        exception.ParamName.Should().BeSameAs(nameof(x));
    }
    
    [Theory]
    [InlineData("Foo", "Bar", StringComparison.Ordinal)]
    [InlineData("12345", "1234", StringComparison.InvariantCultureIgnoreCase)]
    public static void DoesNotEndWith(string x, string y, StringComparison comparisonType) =>
        x.MustNotEndWith(y, comparisonType).Should().BeSameAs(x);

    [Fact]
    public static void ParameterNull()
    {
        var act = () => ((string) null).MustNotEndWith("Foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void ValueNull()
    {
        var act = () => "Foo".MustNotEndWith(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<SubstringException>(message => "FooBar".MustNotEndWith("Bar", message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotEndWith("Bar", message: message));

    [Fact]
    public static void DoesNotEndWithCustomException() =>
        "Foo".MustNotEndWith("r", (_, _) => new Exception()).Should().Be("Foo");

    [Fact]
    public static void DoesNotEndWithCustomExceptionAndComparisonType() =>
        "Foo".MustNotEndWith("R", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception()).Should().Be("Foo");

    [Theory]
    [InlineData("FooBar", "Bar")]
    [InlineData("Foo", null)]
    [InlineData(null, "Baz")]
    public static void CustomException(string first, string second) =>
        Test.CustomException(first, second, (s1, s2, exceptionFactory) => s1.MustNotEndWith(s2, exceptionFactory));

    [Theory]
    [InlineData("FooBar", "Bar", StringComparison.Ordinal)]
    [InlineData(null, "Bar", StringComparison.Ordinal)]
    [InlineData("Baz", null, StringComparison.Ordinal)]
    public static void CustomExceptionWithComparisonType(string a, string b, StringComparison comparisonType) =>
        Test.CustomException(a, b, comparisonType, (s1, s2, ct, exceptionFactory) => s1.MustNotEndWith(s2, ct, exceptionFactory));
}
