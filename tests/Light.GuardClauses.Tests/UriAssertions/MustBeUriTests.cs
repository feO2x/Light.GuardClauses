#nullable enable

using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions;

public static class MustBeUriTests
{
    [Theory(DisplayName = "MustBeUri must return the original string when it can be parsed with the supplied URI kind.")]
    [MemberData(nameof(ParsableUriData))]
    public static void ParsableUri(string value, UriKind uriKind)
    {
        var result = value.MustBeUri(uriKind);

        result.Should().BeSameAs(value);
    }

    public static readonly TheoryData<string, UriKind> ParsableUriData =
        new ()
        {
            { "https://example.com/api/orders", UriKind.Absolute },
            { "api/orders/42", UriKind.Relative },
            { "https://example.com/api/orders", UriKind.RelativeOrAbsolute },
            { "api/orders/42", UriKind.RelativeOrAbsolute },
        };

    [Fact(DisplayName = "MustBeUri must use RelativeOrAbsolute by default and accept an empty relative reference.")]
    public static void DefaultUriKind()
    {
        var value = new string([]);

        value.MustBeUri().Should().BeSameAs(value);
    }

    [Theory(DisplayName = "MustBeUri must throw InvalidUriException when parsing fails for the supplied URI kind.")]
    [MemberData(nameof(UnparsableUriData))]
    public static void UnparsableUri(string value, UriKind uriKind)
    {
        Action act = () => value.MustBeUri(uriKind);

        act.Should().Throw<InvalidUriException>();
    }

    public static readonly TheoryData<string, UriKind> UnparsableUriData =
        new ()
        {
            { "api/orders/42", UriKind.Absolute },
            { "https://example.com/api/orders", UriKind.Relative },
            { "http://[::1", UriKind.RelativeOrAbsolute },
        };

    [Fact]
    public static void NullParameter()
    {
        string? value = null;

        Action act = () => value.MustBeUri();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName(nameof(value));
    }

    [Fact]
    public static void ParameterName()
    {
        Action act = () => "api/orders".MustBeUri(UriKind.Absolute, "endpoint");

        act.Should().Throw<InvalidUriException>()
           .WithParameterName("endpoint")
           .WithMessage("*endpoint must be a valid URI (Absolute)*");
    }

    [Fact]
    public static void CallerArgumentExpression()
    {
        var endpoint = "api/orders";

        Action act = () => endpoint.MustBeUri(UriKind.Absolute);

        act.Should().Throw<InvalidUriException>()
           .WithParameterName(nameof(endpoint));
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<InvalidUriException>(
            message => "api/orders".MustBeUri(UriKind.Absolute, message: message)
        );

    [Fact]
    public static void CustomMessageForNullParameter() =>
        Test.CustomMessage<ArgumentNullException>(
            message => ((string?) null).MustBeUri(message: message)
        );

    [Theory]
    [MemberData(nameof(UnparsableUriData))]
    public static void CustomExceptionFactoryReceivesParameter(string value, UriKind uriKind) =>
        Test.CustomException<string?>(
            value,
            (invalidValue, exceptionFactory) => invalidValue.MustBeUri(uriKind, exceptionFactory)
        );

    [Theory]
    [MemberData(nameof(UnparsableUriData))]
    public static void CustomExceptionFactoryReceivesParameterAndUriKind(string value, UriKind uriKind) =>
        Test.CustomException<string?, UriKind>(
            value,
            uriKind,
            (invalidValue, kind, exceptionFactory) => invalidValue.MustBeUri(kind, exceptionFactory)
        );

    [Fact]
    public static void NullParameterIsPassedToCustomExceptionFactory() =>
        Test.CustomException<string?>(
            null,
            (value, exceptionFactory) => value.MustBeUri(UriKind.RelativeOrAbsolute, exceptionFactory)
        );

    [Fact]
    public static void NullParameterAndUriKindArePassedToCustomExceptionFactory() =>
        Test.CustomException(
            (string?) null,
            UriKind.Absolute,
            (value, uriKind, exceptionFactory) => value.MustBeUri(uriKind, exceptionFactory)
        );

    [Fact]
    public static void ParameterOnlyFactoryIsNotInvokedOnSuccess()
    {
        var value = new string("https://example.com".ToCharArray());

        var result = value.MustBeUri(UriKind.Absolute, (Func<string?, Exception>) null!);

        result.Should().BeSameAs(value);
    }

    [Fact]
    public static void ParameterAndUriKindFactoryIsNotInvokedOnSuccess()
    {
        var value = new string("api/orders".ToCharArray());

        var result = value.MustBeUri(UriKind.Relative, (Func<string?, UriKind, Exception>) null!);

        result.Should().BeSameAs(value);
    }

    [Fact]
    public static void NullParameterOnlyFactoryThrowsArgumentNullException()
    {
        Action act = () => "api/orders".MustBeUri(UriKind.Absolute, (Func<string?, Exception>) null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");
    }

    [Fact]
    public static void NullParameterAndUriKindFactoryThrowsArgumentNullException()
    {
        Action act = () => "api/orders".MustBeUri(
            UriKind.Absolute,
            (Func<string?, UriKind, Exception>) null!
        );

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("exceptionFactory");
    }
}
