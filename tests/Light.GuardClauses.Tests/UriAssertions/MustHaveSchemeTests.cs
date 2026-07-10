using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions;

public static class MustHaveSchemeTests
{
    [Theory(DisplayName = "MustHaveScheme must throw an InvalidUriSchemeException when the URI does not have the specified scheme.")]
    [MemberData(nameof(InvalidSchemeData))]
    public static void InvalidScheme(Uri uri, string scheme)
    {
        Action act = () => uri.MustHaveScheme(scheme, nameof(uri));

        act.Should().Throw<InvalidUriSchemeException>()
           .And.Message.Should().Contain($"{nameof(uri)} must use the scheme \"{scheme}\"");
    }

    public static readonly TheoryData<Uri, string> InvalidSchemeData =
        new()
        {
            { new Uri("http://localhost:8080"), "https" },
            { new Uri("http://my.service.com/upload"), "ftp" }
        };

    [Theory(DisplayName = "MustHaveScheme must not throw an exception when the specified scheme is used by the URI.")]
    [MemberData(nameof(ValidSchemeData))]
    public static void ValidScheme(Uri uri, string scheme)
    {
        var result = uri.MustHaveScheme(scheme);

        result.Should().BeSameAs(uri);
    }

    public static readonly TheoryData<Uri, string> ValidSchemeData =
        new()
        {
            { new Uri("https://www.google.com"), "https" },
            { new Uri("ftps://192.168.177.2"), "ftps" }
        };

    [Fact(DisplayName = "MustHaveScheme must throw an ArgumentNullException when the specified URI is null.")]
    public static void UriNull()
    {
        Action act = () => ((Uri) null).MustHaveScheme("foo");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact(DisplayName = "MustHaveScheme must throw an RelativeUriException when the specified URI is relative.")]
    public static void UriRelative()
    {
        Action act = () => new Uri("/api/foo", UriKind.Relative).MustHaveScheme("https");

        act.Should().Throw<RelativeUriException>();
    }

    [Theory]
    [MemberData(nameof(CustomExceptionData))]
    public static void CustomException(Uri uri, string urlScheme) =>
        Test.CustomException(uri,
                             urlScheme,
                             (url, scheme, exceptionFactory) => url.MustHaveScheme(scheme, exceptionFactory));

    public static readonly TheoryData<Uri, string> CustomExceptionData =
        new()
        {
            { new Uri("https://www.microsoft.com"), "http" },
            { null, "ftp" },
            { new Uri("https://duckduckgo.com/", UriKind.Absolute), null }
        };

    [Fact]
    public static void CustomExceptionNoScheme() => 
        Test.CustomException(new Uri("/contact", UriKind.Relative),
                             (url, exceptionFactory) => url.MustHaveScheme("ssl", exceptionFactory));

    [Fact]
    public static void CustomExceptionNoSchemeUriValid()
    {
        var url = new Uri("https://www.hbo.com/westworld");
        url.MustHaveScheme("https", _ => new Exception()).Should().BeSameAs(url);
    }

    [Fact]
    public static void CustomExceptionSchemeIsValid()
    {
        var uri = new Uri("https://github.com/feO2x/Light.GuardClauses");
        uri.MustHaveScheme("https", (_, _) => null).Should().BeSameAs(uri);
    }

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<InvalidUriSchemeException>(message => new Uri("ftp://foo.com").MustHaveScheme("https", message: message));

    [Fact]
    public static void CustomMessageUriNull() => 
        Test.CustomMessage<ArgumentNullException>(message => ((Uri) null).MustHaveScheme("http", message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var myUrl = new Uri("https://microsoft.com", UriKind.Absolute);

        var act = () => myUrl.MustHaveScheme("ftps");

        act.Should().Throw<InvalidUriSchemeException>()
           .WithParameterName(nameof(myUrl));
    }
}