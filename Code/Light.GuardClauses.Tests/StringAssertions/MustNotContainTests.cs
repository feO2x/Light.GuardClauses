using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustNotContainTests
{
    [Theory]
    [InlineData("Foo", "o")]
    [InlineData("bar", "ba")]
    [InlineData("baz", "az")]
    [InlineData("qux", "qux")]
    [InlineData("quux", "")]
    public static void ContainsValue(string @string, string value)
    {
        Action act = () => @string.MustNotContain(value, nameof(@string));

        var assertion = act.Should().Throw<SubstringException>().Which;
        assertion.Message.Should().Contain($"{nameof(@string)} must not contain {value.ToStringOrNull()} as a substring, but it actually is {@string.ToStringOrNull()}.");
        assertion.ParamName.Should().BeSameAs(nameof(@string));
    }

    [Theory]
    [InlineData("Foo", "O", StringComparison.OrdinalIgnoreCase)]
    [InlineData("Bar", "ar", StringComparison.Ordinal)]
    [InlineData("Baz", "Baz", StringComparison.CurrentCulture)]
    [InlineData("Qux", "", StringComparison.Ordinal)]
    public static void ContainsValueCustomSearch(string x, string y, StringComparison comparisonType)
    {
        Action act = () => x.MustNotContain(y, comparisonType, nameof(x));

        var assertion = act.Should().Throw<SubstringException>().Which;
        assertion.Message.Should().Contain($"{nameof(x)} must not contain {y.ToStringOrNull()} as a substring ({comparisonType}), but it actually is {x.ToStringOrNull()}.");
        assertion.ParamName.Should().BeSameAs(nameof(x));
    }

    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData("Baz", "zab")]
    [InlineData("Qux", "Quux")]
    public static void NotContainsValue(string @string, string value) =>
        @string.MustNotContain(value).Should().BeSameAs(@string);

    [Theory]
    [InlineData("Foo", "Bar", StringComparison.OrdinalIgnoreCase)]
    [InlineData("Baz", "BAZ", StringComparison.Ordinal)]
    public static void NotContainsValueCustomSearch(string x, string y, StringComparison comparisonType) =>
        x.MustNotContain(y, comparisonType).Should().BeSameAs(x);


    [Theory]
    [InlineData("Foo", "o")]
    [InlineData(null, "Bar")]
    [InlineData("Baz", null)]
    public static void CustomException(string first, string second) =>
        Test.CustomException(first,
                             second,
                             (x, y, exceptionFactory) => x.MustNotContain(y, exceptionFactory));

    [Theory]
    [InlineData("Foo", "O", StringComparison.OrdinalIgnoreCase)]
    [InlineData("Bar", null, StringComparison.CurrentCulture)]
    [InlineData(null, "Baz", StringComparison.CurrentCultureIgnoreCase)]
    public static void CustomExceptionCustomSearch(string first, string second, StringComparison comparisonType) =>
        Test.CustomException(first,
                             second,
                             comparisonType,
                             (x, y, ct, exceptionFactory) => x.MustNotContain(y, ct, exceptionFactory));

    [Fact]
    public static void NoCustomExceptionThrown() =>
        "Foo".MustNotContain("Bar", (_, _) => new Exception()).Should().BeSameAs("Foo");

    [Fact]
    public static void NoCustomExceptionThrownOnCustomSearch() =>
        "Foo".MustNotContain("FOO", StringComparison.CurrentCulture, (_, _, _) => new Exception()).Should().BeSameAs("Foo");

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<SubstringException>(message => "Foo".MustNotContain("o", message: message));

    [Fact]
    public static void CustomMessageCustomSearch() =>
        Test.CustomMessage<SubstringException>(message => "Foo".MustNotContain("o", StringComparison.CurrentCulture, message: message));

    [Fact]
    public static void CustomMessageParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotContain("Foo", message: message));

    [Fact]
    public static void CustomMessageCustomSearchParameterNull() =>
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustNotContain("Foo", StringComparison.Ordinal, message: message));

    [Fact]
    public static void CustomMessageValueNull() =>
        Test.CustomMessage<ArgumentNullException>(message => "Foo".MustNotContain(null!, message: message));

    [Fact]
    public static void CustomMessageCustomSearchValueNull() =>
        Test.CustomMessage<ArgumentNullException>(message => "Foo".MustNotContain(null!, StringComparison.Ordinal, message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string bar = "Bar";

        var act = () => bar.MustNotContain("ar");

        act.Should().Throw<SubstringException>()
           .WithParameterName(nameof(bar));
    }
}